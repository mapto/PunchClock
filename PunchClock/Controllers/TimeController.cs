using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;


using PunchClock.Models;

namespace PunchClock.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
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
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return new HttpResponseMessage(result ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
        }


        public IEnumerable<Slot> GetSlots()
        {
            TimeModel timeService = TimeModel.getService();
            var slots = timeService.GetSlots();

            return slots;
        }

        public HttpResponseMessage PostSlot(FormDataCollection formData)
        {
            DateTime start, end;
            long id;
            bool success = true;
            success &= DateTime.TryParse(formData["Start"], out start);
            success &= DateTime.TryParse(formData["End"], out end);
            if (!success)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            string description = formData["Description"];
            string project = formData["Project"];

            TimeModel timeService = TimeModel.getService();
            Slot storedSlot = null;
            if (Int64.TryParse(formData["ID"], out id))
            {
                storedSlot = timeService.GetSlotByID(id);
            }
            if (storedSlot != null)
            {
                success = timeService.UpdateSlot(new Slot(id, start, end, description, project));
                return new HttpResponseMessage(success ? HttpStatusCode.OK : HttpStatusCode.InternalServerError);
            }
            else
            {
                success = timeService.InsertSlot(new Slot(start, end, description, project));
                return new HttpResponseMessage(success ? HttpStatusCode.Created : HttpStatusCode.InternalServerError);
            }
        }
    }
}
