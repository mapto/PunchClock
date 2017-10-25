using NUnit.Framework;
using System;
using System.Collections.Generic;
using PunchClock.Models;

using System.Data;
using Mono.Data.Sqlite;

namespace PunchClock.Tests.Models
{
    [TestFixture]
    public class TimeModelTest
    {
        private SqliteConnection conn;

        [SetUp]
        public void Init()
        {
            conn = TimeModel.getConnection();

            using (SqliteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = TimeModel.tableCreateCommand;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }

        }

        [Test]
        public void SlotStorageContainer()
        {
            // Arrange
            const string sqlInsert = @"INSERT INTO Slot 
                (start, end, description, project) 
                VALUES(:start, :end ,:description, :project)";
            const string sqlSelect =
               "SELECT start, end, description, project FROM Slot";

            SqliteCommand cmd;
            SqliteDataReader reader;

            // Act
            cmd = new SqliteCommand(this.conn);
            cmd.CommandText = sqlInsert;
            cmd.Parameters.Add(new SqliteParameter(":start", new DateTime(2018, 5, 2, 9, 00, 00)));
            cmd.Parameters.Add(new SqliteParameter(":end", new DateTime(2018, 5, 2, 18, 00, 00)));
            cmd.Parameters.Add(new SqliteParameter(":description", "A test task"));
            cmd.Parameters.Add(new SqliteParameter(":project", "Test"));
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            cmd = new SqliteCommand(sqlSelect, this.conn);
            reader = cmd.ExecuteReader();
            cmd.Dispose();

            //Assert
            Assert.Pass(); // unnecessary
        }

        [Test]
        // Warning: Assumes that is run after the previous test.
        // TODO: Remove when lifecycle can be completed.
        public void SlotStorageData()
        {
            // Arrange
            TimeModel timeRepository = new TimeModel();
            var lastEntry = new Slot(1, new DateTime(2018, 5, 2, 9, 00, 00), new DateTime(2018, 5, 2, 18, 00, 00),
                                     "Just a timeslot", "Test");

            // Act
            var slots = timeRepository.GetSlots();

            // Assert
            //Assert.AreEqual(expectedType, slots.ToString());
            IEnumerator<Slot> iter = slots.GetEnumerator();
            iter.MoveNext();
            Assert.AreEqual(lastEntry, iter.Current);
        }

        [Test]
        public void SlotStorageOperations()
        {
            // Arrange
            TimeModel timeService = new TimeModel();
            DateTime start = DateTime.Today;
            DateTime end = DateTime.Now;
            string desc = "A daily dummy job";
            string project = "Test";
            Slot createdSlot = new Slot(start, end, desc, project);

            // Act
            timeService.InsertSlot(createdSlot);
            long id = timeService.GetSlotIdByData(start, end, desc, project);
            Slot foundSlot = timeService.GetSlotByID(id);

            // Assert
            Assert.AreEqual(createdSlot, foundSlot);

            // Act
            createdSlot.Description = "An updated activity description";
            timeService.UpdateSlot(createdSlot);
            foundSlot = timeService.GetSlotByID(id);

            // Assert
            Assert.AreEqual(foundSlot.Description, createdSlot.Description);

            // Act
            timeService.DeleteSlot(id);
            foundSlot = timeService.GetSlotByID(id);
            long noId = timeService.GetSlotIdByData(createdSlot.Start, createdSlot.End, createdSlot.Description, createdSlot.Project);

            // Assert
            Assert.IsNull(foundSlot);
            Assert.AreEqual(noId, TimeModel.NON_VALUE);
        }

        [TearDown]
        public void stop()
        {
            conn.Close();
        }
    }
}
