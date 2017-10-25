using NUnit.Framework;
using System;
using System.Net.Http;
using System.Linq;
using System.Web.Http;
using PunchClock.Controllers;
using PunchClock.Models;

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
            response = c.PutSlot(start, end, desc, project);
            foundSlot = c.GetSlots().First();
            currentId = foundSlot.ID;


            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(foundSlot.Start, start);
            Assert.AreEqual(foundSlot.End, end);
            Assert.AreEqual(foundSlot.Description, desc);
            Assert.AreEqual(foundSlot.Project, project);

            // Act
            foundSlot = c.GetSlotById(currentId);

            // Assert
            Assert.AreEqual(foundSlot.Start, start);
            Assert.AreEqual(foundSlot.End, end);
            Assert.AreEqual(foundSlot.Description, desc);
            Assert.AreEqual(foundSlot.Project, project);

            // Act
            string newDesc = "An updated activity description";
            response = c.PutSlot(currentId, start, end, newDesc, project);
            foundSlot = c.GetSlotById(currentId);

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(foundSlot.Start, start);
            Assert.AreEqual(foundSlot.End, end);
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