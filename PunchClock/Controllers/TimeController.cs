using System;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Http;


using PunchClock.Models;

namespace PunchClock.Controllers
{
    public class TimeController : ApiController
    {
        public Slot GetSlotById(long id)
        {
            TimeModel timeService = TimeModel.getService();
            Slot slot = timeService.GetSlotByID(id);
            if (slot == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return slot;

        }

        public HttpResponseMessage DeleteSlot(long id)
        {
            bool result = false;
			TimeModel timeService = TimeModel.getService();
            if (timeService.GetSlotByID(id) != null)
            {
                result = timeService.DeleteSlot(id);
            } else {
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}
            return new HttpResponseMessage(result ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
        }


        public IEnumerable<Slot> GetSlots()
        {
            TimeModel timeService = new TimeModel();
            var slots = timeService.GetSlots();

            return slots;
        }

        public HttpResponseMessage PutSlot(DateTime start, DateTime end, string description, string project)
        {
            Slot slot = new Slot(start, end, description, project);
            bool result = TimeModel.getService().InsertSlot(slot);

            return new HttpResponseMessage(result ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
        }

        //public HttpResponseMessage PostSlot(NameValueCollection formData)
		public HttpResponseMessage PostSlot(FormDataCollection formData)
		{
			string utcFormat = "yyyy-MM-ddTHH:mm";
			DateTime start = DateTime.ParseExact(formData["start"], utcFormat, CultureInfo.InvariantCulture);
			DateTime end = DateTime.ParseExact(formData["end"], utcFormat, CultureInfo.InvariantCulture);
			string description = formData["description"];
            string project = formData["project"];

            bool result = false;
			TimeModel timeService = TimeModel.getService();
			result = timeService.InsertSlot(new Slot(start, end, description, project));
			return new HttpResponseMessage(result ? HttpStatusCode.Created : HttpStatusCode.InternalServerError);
		}

        public HttpResponseMessage PutSlot(long id, DateTime start, DateTime end, string description, string project)
        {
            // If slot is not pre-existing, id is ignored
            bool result = false;
            TimeModel timeService = TimeModel.getService();
            Slot storedSlot = timeService.GetSlotByID(id);
            if (storedSlot != null)
            {
                result = timeService.UpdateSlot(new Slot(id, start, end, description, project));
                return new HttpResponseMessage(result ? HttpStatusCode.OK : HttpStatusCode.InternalServerError);
            }
            else
            {
				result = timeService.InsertSlot(new Slot(start, end, description, project));
                return new HttpResponseMessage(result ? HttpStatusCode.Created : HttpStatusCode.InternalServerError);
            }
        }

    }
}
