using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

using PunchClock.Controllers;
using PunchClock.Models;

namespace PunchClock.Tests.Controllers
{
	[TestFixture]
	public class FilterControllerTest
    {
		[Test]
		public void ProjectFilters()
        {
			// Arrange
			ProjectController pc = new ProjectController();
            FilterController fc = new FilterController();
            Slot slot;

			// Act
			foreach (var project in pc.GetProjects())
            {
				slot = fc.GetSlots(project).Last();
				int year = slot.Start.Year;
				int month = slot.Start.Month;
				int day = slot.Start.Day;

				// Assert
				Assert.AreEqual(slot.Project, project);

				Assert.IsTrue(fc.GetSlots(project, year).Contains(slot));
				Assert.IsTrue(fc.GetSlots(project, year, month).Contains(slot));
				Assert.IsTrue(fc.GetSlots(project, year, month, day).Contains(slot));

				Assert.IsTrue(fc.GetSlots("All", year).Contains(slot));
				Assert.IsTrue(fc.GetSlots("All", year, month).Contains(slot));
				Assert.IsTrue(fc.GetSlots("All", year, month, day).Contains(slot));
			}
		}

	}
}
