using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;

namespace PunchClock
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            // remove XML formatter so JSON is served
            var formatters = GlobalConfiguration.Configuration.Formatters;
            formatters.Remove(formatters.XmlFormatter);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
