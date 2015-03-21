OWIN Self Host Demo ReadMe

This demonstrates self-hosting with OWIN and Katana.
A console app is used for demo purposes, but this could
also be a Windows Service.  Unlike normal Web API self-hosting,
this creates an HttpListener without dependency on WCF.

NOTE: If there is a URL reservation, you'll need to remove it.
Open admin command prompt and enter:
netsh http delete urlacl url=http://+:12345/

PART A: OWIN Self Hosting

1. Create a console app, add the following NuGet packages:
   - Microsoft.Owin.SelfHost

2. Add a Startup class to the project.
   - Add the following method:

    public void Configuration(IAppBuilder app)
    {
        app.UseWelcomePage();
    }


3. Add the following to Program.Main:

	using (WebApp.Start<Startup>("http://localhost:12345/"))
	{
        Console.WriteLine("Web app started ...");
		Console.ReadLine();
	}

4. Run the console app, then open a browser and navigate to:
   http://localhost:12345/
   - You should see the welcome page.

PART B: Middleware

1. Add a custom middleware logging component

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

2. Update Startup.Configuration to include logging middleware

   app.Use<LoggingComponent>();

   - Run the app to see logging messages written to the console

3. Enable Web API middleware by adding the following NuGet package:
   - Microsoft.AspNet.WebApi.OwinSelfHost

4. Add a controller
   - GreetingController : ApiController

    readonly string[] _greetings = { "Hello", "Howdy", "Ciao", "Yo" };

    // GET api/greeting
    public IEnumerable<string> Get()
    {
        return _greetings;
    }

    // GET api/greeting/1 
    public string Get(int id)
    {
        return _greetings[id - 1];
    }

5. Add web api to Startup.Configuration:

    // Configure web api routing
    var config = new HttpConfiguration();
    config.MapHttpAttributeRoutes();
    config.Routes.MapHttpRoute(
        "DefaultApi", 
        "api/{controller}/{id}",
        new { id = RouteParameter.Optional });
    app.UseWebApi(config);

6. Run the app and browse to:
   http://localhost:12345/api/greeting
   - You should see a json or xml response

