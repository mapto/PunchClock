using System;
using System.Runtime.Serialization;

namespace PunchClock.Models
{
    [DataContract]
	public class Slot
	{
		[DataMember]
		public long ID { get; set; } // database index
		[DataMember]
		public DateTime Start { get; set; }
		[DataMember]
		public DateTime End { get; set; }
		[DataMember]
		public string Description { get; set; } // free-text description that allows human interpretation
		[DataMember]
		public string Project { get; set; } // a grouping mechanism, potentially could grow into a referenced entity

        public Slot(DateTime start, DateTime end, string description, string project) : this(TimeModel.NON_VALUE, start, end, description, project)
        {
		}

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
			return this.Project + "[" + this.ID + "] (" + this.Start + ", " + this.End + ")";
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
