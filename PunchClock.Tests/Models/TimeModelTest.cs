using NUnit.Framework;
using System;
using PunchClock.Models;

using System.Data;
using Mono.Data.Sqlite;

namespace PunchClock.Tests.Models
{
	[TestFixture]
	public class TimeModelTest
	{
		private SqliteConnection conn;
        private const string connectionString = TimeModel.connectionString;

		[SetUp]
		public void Init()
		{
			conn = new SqliteConnection(connectionString);
			conn.Open();

            using (SqliteCommand cmd = conn.CreateCommand())
			{
				cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Slot (
                id INTEGER PRIMARY KEY AUTOINCREMENT, start NUMERIC, end NUMERIC, description TEXT, project TEXT)";
				cmd.CommandType = CommandType.Text;
				cmd.ExecuteNonQuery();
			}

		}

		[TestCase]
		public void SlotStorage()
		{
			SqliteCommand cmd;
			SqliteDataReader reader;

			cmd = new SqliteCommand(this.conn);
			cmd.CommandText = @"INSERT INTO Slot 
                (start, end, description, project) 
                VALUES(:start, :end ,:description, :project)";
			cmd.Parameters.Add(new SqliteParameter(":start", DateTime.Today));
			cmd.Parameters.Add(new SqliteParameter(":end", DateTime.Now));
			cmd.Parameters.Add(new SqliteParameter(":description", "Two (unicode test: \u05D1)"));
			cmd.Parameters.Add(new SqliteParameter(":project", "Test"));
			cmd.ExecuteNonQuery();

			const string sqlSelect =
			   "SELECT start, end, description, project FROM Slot";
			cmd = new SqliteCommand(sqlSelect, this.conn);
			reader = cmd.ExecuteReader();
			Console.WriteLine(reader.Read());
			cmd.Dispose();
		}

		[TearDown]
		public void stop()
		{
			conn.Close();
		}
	}
}
