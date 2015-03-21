using System;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace OwinSelfHost
{
    public class LoggingComponent : OwinMiddleware
    {
        public LoggingComponent(OwinMiddleware next) : base(next) { }

        public async override Task Invoke(IOwinContext context)
        {
            // Process request
            Console.WriteLine("\nRequest Path: {0}", context.Request.Path);

            // Invoke next middleware component
            await Next.Invoke(context);

            // Process response
            Console.WriteLine("Response Status Code: {0}", context.Response.StatusCode);
        }
    }
}