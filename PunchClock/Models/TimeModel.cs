using System;
using System.Collections.Generic;

using Mono.Data.Sqlite;

namespace PunchClock.Models
{
    public class TimeModel : ITimeModel, IDisposable
    {
        public static readonly long NON_VALUE = -1;
        // TODO: Find way to export this in Web.config
        public static readonly string connectionString = "URI=file:TimeStorage.db";
        private static SqliteConnection conn = null;

        // Was unable to identify a way to derive DAO mapping from code with Mono.Data.Sqlite
        public const string tableCreateCommand = @"
CREATE TABLE IF NOT EXISTS Slot (
id INTEGER PRIMARY KEY AUTOINCREMENT,
start NUMERIC,
end NUMERIC,
description TEXT,
project TEXT)";
        private const string selectAllCommand = "SELECT * FROM Slot ORDER BY id DESC";
        private const string selectByIdCommand = "SELECT * FROM Slot WHERE id=:id";
        private const string selectIdByDataCommand = @"SELECT id FROM Slot 
                WHERE start=:start AND end=:end AND
                description=:description AND project=:project
                ORDER BY id DESC LIMIT 1";
        private const string deleteCommand = "DELETE FROM Slot WHERE id=:id";
        private const string insertCommand = @"INSERT INTO Slot 
                (start, end, description, project) 
                VALUES(:start, :end ,:description, :project)";
        private const string updateCommand = @"UPDATE Slot
                SET start=:start, end=:end,
                description=:description, project=:project
                WHERE id=:id";

        // This might be problematic with parallel server instances
        // Was not able to identify DBContext to work with Mono.Data.Sqlite.
        // As a consequence pass connection instead.
        // Made it singleton to avoid managing DB logic in controller.
        public static SqliteConnection getConnection()
        {
            if (conn == null)
            {
                conn = new SqliteConnection(connectionString);
            }
            if (conn.State.Equals(System.Data.ConnectionState.Broken))
            {
                conn.Close();
            }
            if (conn.State.Equals(System.Data.ConnectionState.Closed))
            {
                conn.Open();
            }
            return conn;
        }

        public static TimeModel getService()
        {
            return new TimeModel();
        }

        public TimeModel()
        {
            conn = getConnection();
            using (SqliteCommand cmd = new SqliteCommand(tableCreateCommand, conn))
            {
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
        }

        public IEnumerable<Slot> GetSlots()
        {
            List<Slot> result = new List<Slot>();
            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = selectAllCommand;
                using (SqliteDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        // Was unable to identify a way to automate the DAO mapping with Mono.Data.Sqlite
                        yield return new Slot(rdr.GetInt64(rdr.GetOrdinal("id")),
                                              rdr.GetDateTime(rdr.GetOrdinal("start")),
                                              rdr.GetDateTime(rdr.GetOrdinal("end")),
                                              rdr.GetString(rdr.GetOrdinal("description")),
                                              rdr.GetString(rdr.GetOrdinal("project")));
                    }
                }
                cmd.Dispose();
            }
        }

        public Slot GetSlotByID(long id)
        {
            Slot result = null;
            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = selectByIdCommand;
                cmd.Parameters.Add(new SqliteParameter(":id", id));
                using (SqliteDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        result = new Slot(rdr.GetInt64(rdr.GetOrdinal("id")),
                            rdr.GetDateTime(rdr.GetOrdinal("start")),
                            rdr.GetDateTime(rdr.GetOrdinal("end")),
                            rdr.GetString(rdr.GetOrdinal("description")),
                            rdr.GetString(rdr.GetOrdinal("project")));
                    }
                }
                cmd.Dispose();
            }
            return result;
        }

        public long GetSlotIdByData(DateTime start, DateTime end, string description, string project)
        {
            long result = NON_VALUE;
            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = selectIdByDataCommand;
                cmd.Parameters.Add(new SqliteParameter(":start", start));
                cmd.Parameters.Add(new SqliteParameter(":end", end));
                cmd.Parameters.Add(new SqliteParameter(":description", description));
                cmd.Parameters.Add(new SqliteParameter(":project", project));
                using (SqliteDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        result = rdr.GetInt64(rdr.GetOrdinal("id"));
                    }
                }
                cmd.Dispose();
            }
            return result;
        }

        public bool InsertSlot(Slot slot)
        {
            bool success = true;
            if (slot.ID != NON_VALUE)
            {
                // Existing ID implies already stored
                return false;
            }
            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = insertCommand;
                cmd.Parameters.Add(new SqliteParameter(":start", slot.Start));
                cmd.Parameters.Add(new SqliteParameter(":end", slot.End));
                cmd.Parameters.Add(new SqliteParameter(":description", slot.Description));
                cmd.Parameters.Add(new SqliteParameter(":project", slot.Project));
                int changed = cmd.ExecuteNonQuery();
                cmd.Dispose();
                if (changed != 1)
                {
                    success = false;
                }
            }

            long newId = GetSlotIdByData(slot.Start, slot.End, slot.Description, slot.Project);
            if (newId == NON_VALUE)
            {
                return false;
            }
            slot.ID = newId;

            return success;
        }
        public bool DeleteSlot(long id)
        {
            bool success = true;
            Slot item = GetSlotByID(id);
            if (item == null)
            {
                return false;
            }

            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = deleteCommand;
                cmd.Parameters.Add(new SqliteParameter(":id", id));
                int changed = cmd.ExecuteNonQuery();
                cmd.Dispose();
                if (changed != 1)
                {
                    success = false;
                }
            }
            return success;
        }

        public bool UpdateSlot(Slot newSlot)
        {
            bool success = true;
            if (newSlot.ID == NON_VALUE)
            {
                return false;
            }
            Slot storedSlot = GetSlotByID(newSlot.ID);
            if (storedSlot == null)
            {
                return false;
            }
            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = updateCommand;
                cmd.Parameters.Add(new SqliteParameter(":id", newSlot.ID));
                cmd.Parameters.Add(new SqliteParameter(":start", newSlot.Start));
                cmd.Parameters.Add(new SqliteParameter(":end", newSlot.End));
                cmd.Parameters.Add(new SqliteParameter(":description", newSlot.Description));
                cmd.Parameters.Add(new SqliteParameter(":project", newSlot.Project));
                int changed = cmd.ExecuteNonQuery();
                cmd.Dispose();
                if (changed != 1)
                {
                    success = false;
                }
            }

            return success;
        }

        public void Dispose()
        {
            conn.Close();
        }
    }
}