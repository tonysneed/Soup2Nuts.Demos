using System;
using System.Web.Http;
using Owin;

namespace OwinSelfHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Custom middleware
            app.Use<LoggingComponent>();

            // Configure web api routing
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional });
            app.UseWebApi(config);

            // Show welcome page
            app.UseWelcomePage();
        }
    }
}
