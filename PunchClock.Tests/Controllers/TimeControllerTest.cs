using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Web.Http;
using PunchClock.Controllers;
using PunchClock.Models;
using System.Net.Http.Formatting;

namespace PunchClock.Tests.Controllers
{
    [TestFixture]
    public class TimeControllerTest
    {
        [Test]
        public void SingleSlotOperations()
        {
            // Arrange
            TimeController c = new TimeController();
            HttpResponseMessage response;
            long currentId;
            Slot foundSlot;

            DateTime start = DateTime.Today;
            DateTime end = DateTime.Now;
            string desc = "A daily dummy job";
            string project = "Test";

            // Act
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                {"ID", "#"},
                {"Start", start.ToString("yyyy-MM-ddTHH:mm:ss")}, // Firefox
				{"End", end.ToString("yyyy-MM-ddTHH:mm")}, // Chrome
                {"Description", desc},
                {"Project", project},
            };
            response = c.PostSlot(new FormDataCollection(data));
            foundSlot = c.GetSlots().First();
            currentId = foundSlot.ID;

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsTrue(Math.Abs(foundSlot.Start.Subtract(start).TotalMinutes) < 1);
            Assert.IsTrue(Math.Abs(foundSlot.End.Subtract(end).TotalMinutes) < 1);
            Assert.AreEqual(foundSlot.Description, desc);
            Assert.AreEqual(foundSlot.Project, project);

            // Act
            foundSlot = c.GetSlotById(currentId);

            // Assert
            Assert.IsTrue(Math.Abs(foundSlot.Start.Subtract(start).TotalMinutes) < 1);
            Assert.IsTrue(Math.Abs(foundSlot.End.Subtract(end).TotalMinutes) < 1);
            Assert.AreEqual(foundSlot.Description, desc);
            Assert.AreEqual(foundSlot.Project, project);

            // Act
            string newDesc = "An updated activity description";
            data["ID"] = currentId.ToString();
            data["Description"] = newDesc;
            response = c.PostSlot(new FormDataCollection(data));
            foundSlot = c.GetSlotById(currentId);

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsTrue(Math.Abs(foundSlot.Start.Subtract(start).TotalMinutes) < 1);
            Assert.IsTrue(Math.Abs(foundSlot.End.Subtract(end).TotalMinutes) < 1);
            Assert.AreEqual(foundSlot.Description, newDesc);
            Assert.AreEqual(foundSlot.Project, project);

            // Act
            response = c.DeleteSlot(currentId);

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);

            // Act
            try
            {
                foundSlot = c.GetSlotById(currentId);
                Assert.Fail();
            }
            catch (HttpResponseException hre)
            {
                Assert.Pass();
            }
        }
    }
}