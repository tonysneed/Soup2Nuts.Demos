using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Owin_IIS_Host.Startup))]

namespace Owin_IIS_Host
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure web api routing
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}",
                new { id = RouteParameter.Optional });
            app.UseWebApi(config);

            // Welcome page
            app.UseWelcomePage();
        }
    }
}
