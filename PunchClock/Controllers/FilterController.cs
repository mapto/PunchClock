using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

using PunchClock.Models;

namespace PunchClock.Controllers
{
	[EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
	public class FilterController : ApiController
    {
		public IEnumerable<Slot> GetSlots()
		{
            TimeModel timeService = TimeModel.getService();
			var slots = timeService.GetSlots();

			return slots;
		}

		public IEnumerable<Slot> GetSlots(string project)
		{
            TimeModel timeService = TimeModel.getService();
            if (!timeService.GetProjects().Contains(project))
            {
                return timeService.GetSlots();
			}
            return timeService.GetSlots(project);
		}

        public IEnumerable<Slot> GetSlots(string project, int year)
        {
			DateTime start = new DateTime();
			DateTime end = new DateTime();
			try
			{
				start = new DateTime(year, 1, 1);
				end = new DateTime(year + 1, 1, 1);
			}
            catch (ArgumentException)
			{
				return this.GetSlots(project);
			}
			return this.GetSlots(project, start, end);
		}

		public IEnumerable<Slot> GetSlots(string project, int year, int month)
		{
			DateTime start = new DateTime();
			DateTime end = new DateTime();
			try
			{
				start = new DateTime(year, month, 1);
			    end = new DateTime(year, month + 1, 1);
			}
			catch (ArgumentException)
			{
				return this.GetSlots(project);
			}
			return this.GetSlots(project, start, end);
		}

		public IEnumerable<Slot> GetSlots(string project, int year, int month, int day)
		{
            DateTime start = new DateTime();
			DateTime end = new DateTime();
			try
			{
				start = new DateTime(year, month, day);
			    end = new DateTime(year, month, day + 1);
			}
			catch (ArgumentException)
			{
				return this.GetSlots(project);
            }
			return this.GetSlots(project, start, end);
		}

        private IEnumerable<Slot> GetSlots(string project, DateTime start, DateTime end)
		{
            TimeModel timeService = TimeModel.getService();
			if (!timeService.GetProjects().Contains(project))
			{
				return timeService.GetSlots(start, end);
			}
			return timeService.GetSlots(project, start, end);
		}
	}
}
