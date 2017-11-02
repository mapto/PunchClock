using System;
using System.Collections.Generic;
using PunchClock.Models;

namespace PunchClock.Models
{
    public interface ITimeModel : IDisposable
    {
        IEnumerable<string> GetProjects();

		IEnumerable<Slot> GetSlots();
        IEnumerable<Slot> GetSlots(string project);
        IEnumerable<Slot> GetSlots(DateTime start, DateTime end);
        IEnumerable<Slot> GetSlots(string project, DateTime start, DateTime end);

		Slot GetSlotByID(long id);
        long GetSlotIdByData(DateTime start, DateTime end, string description, string project);
        bool InsertSlot(Slot slot);
		bool DeleteSlot(long id);
        bool UpdateSlot(Slot slot);
	}
}
