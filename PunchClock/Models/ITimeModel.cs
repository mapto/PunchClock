using System;
using System.Collections.Generic;
using PunchClock.Models;

namespace PunchClock.Models
{
    public interface ITimeModel : IDisposable
    {
		IEnumerable<Slot> GetSlots();
		Slot GetSlotByID(long id);
        long GetSlotIdByData(DateTime start, DateTime end, string description, string project);
        bool InsertSlot(Slot slot);
		bool DeleteSlot(long id);
        bool UpdateSlot(Slot slot);
	}
}
