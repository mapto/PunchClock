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
    [EnableCors(origins: "*", headers: "*", methods: "GET,DELETE,POST")]
    public class TimeController : ApiController
    {
        private const string UNSPECIFIED_PROJECT = "Unspecified";
        /*
        public IEnumerable<Slot> GetAllSlots() {
            FilterController filter = new FilterController();
            return filter.GetSlots();
        }
        */

        [AcceptVerbs("OPTIONS")]
        public HttpResponseMessage Options()
        {
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Headers.Add("Access-Control-Allow-Origin", "*");
            resp.Headers.Add("Access-Control-Allow-Methods", "GET,DELETE,POST");

            return resp;
        }

        public Slot Get(long id)
        {
            var slot = TimeModel.getService().GetSlotByID(id);
            if (slot == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return slot;
        }

        public HttpResponseMessage Delete(long id)
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

        public HttpResponseMessage Post(FormDataCollection formData)
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

            if (start > end)
            {
                DateTime tmp = end;
                end = start;
                start = tmp;
            }
            string description = formData["Description"];
            string project = formData["Project"].Trim().Length == 0 ? UNSPECIFIED_PROJECT : formData["Project"];

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
