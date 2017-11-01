using System.Web.Http;

namespace PunchClock
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{project}/{year}/{month}/{day}",
                defaults: new
                {
                    project = RouteParameter.Optional,
                    year = RouteParameter.Optional,
                    month = RouteParameter.Optional,
                    day = RouteParameter.Optional
                }
            );
        }
    }
}
