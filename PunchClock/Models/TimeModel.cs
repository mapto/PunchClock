using System;
using System.Collections.Generic;

using Mono.Data.Sqlite;

namespace PunchClock.Models
{
    public class TimeModel
    {
        public const string connectionString = "URI=file:TimeStorage.db";

        private const string createCommand = @"
CREATE TABLE IF NOT EXISTS Slot (
id INTEGER PRIMARY KEY AUTOINCREMENT,
start NUMERIC,
end NUMERIC,
description TEXT,
project TEXT)";
        
        private const string selectAllCommand = "SELECT * FROM Slot";

        public TimeModel()
        {
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                using (SqliteCommand cmd = new SqliteCommand(createCommand, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }

        }

        public IEnumerable<Slot> Slots()
        {
            List<Slot> result = new List<Slot>();
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = selectAllCommand;
                    using (SqliteDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            yield return new Slot(rdr.GetInt64(rdr.GetOrdinal("id")),
                                                  rdr.GetDateTime(rdr.GetOrdinal("start")),
                                                  rdr.GetDateTime(rdr.GetOrdinal("end")),
                                                  rdr.GetString(rdr.GetOrdinal("description")),
                                                  rdr.GetString(rdr.GetOrdinal("project")));
                        }
                    }
                }
                conn.Close();
            }
        }
    }

	public class Slot
	{
		public long ID { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public string Description { get; set; }
		public string Project { get; set; } // a grouping mechanism

		public Slot(long id, DateTime start, DateTime end, string description, string project)
		{
			this.ID = id;
			this.Start = start;
			this.End = end;
			this.Description = description;
			this.Project = project;
		}

		public override string ToString()
		{
			return this.Project + " (" + this.Start + ", " + this.End + ")";
		}

		public override bool Equals(Object obj)
		{
			Slot other = obj as Slot;
			if (other == null)
				return false;
			else
				return this.Start.Equals(other.Start) && this.End.Equals(other.End) && this.Project.Equals(other.Project);
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}
	}

}