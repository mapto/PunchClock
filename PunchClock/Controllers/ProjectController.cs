using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

using PunchClock.Models;

namespace PunchClock.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class ProjectController : ApiController
    {
        public IEnumerable<string> GetProjects()
        {
            return TimeModel.getService().GetProjects();
        }
    }
}
