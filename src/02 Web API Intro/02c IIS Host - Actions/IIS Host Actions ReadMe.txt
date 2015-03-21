IIS Hosting, Web API Actions Demo ReadMe

This demonstrates IIS hosting with OWIN and Katana.

PART A: IIS Hosting with OWIN

1. Create an empty web app, add the following NuGet packages:
   - Microsoft.Owin.Host.SystemWeb

2. Add a Startup class to the project.
   - Select OWIN Startup item template
     > Inserts OwinStartup attribute

   - Add to the Configuration method:

    app.UseWelcomePage();

3. Press F5 to run the app
   - You should see the welcome page
   - Inspect the output windows for logging messages

Part B: Web API Actions

1. Enable Web API middleware by adding the following NuGet package:
   - Microsoft.AspNet.WebApi.Owin

2. Add web api to Startup.Configuration:

    // Configure web api routing
    var config = new HttpConfiguration();
    config.MapHttpAttributeRoutes();
    config.Routes.MapHttpRoute(
        "DefaultApi", 
        "api/{controller}/{id}",
        new { id = RouteParameter.Optional });
    app.UseWebApi(config);

3. Add a controller
    public class ValuesController : ApiController {

    private static readonly Dictionary<int, string> Values = 
        new Dictionary<int, string>
    {
        { 1, "value1"}, { 2, "value2"}, { 3, "value3"}, { 4, "value4"}, { 5, "value5"},
    }; 

    // GET api/values
    public HttpResponseMessage Get()
    {
        IEnumerable<string> values = Values.Values.AsEnumerable();
        return Request.CreateResponse(HttpStatusCode.OK, values);
    }

    // GET api/values/5
    public HttpResponseMessage Get(int id)
    {
        if (!Values.ContainsKey(id))
            throw new HttpResponseException(HttpStatusCode.NotFound);
        return Request.CreateResponse(HttpStatusCode.OK, Values[id]);
    }

    // POST api/values
    public HttpResponseMessage Post([FromBody]string value)
    {
        var id = Values.Count + 1;
        Values.Add(id, value);
        var response = Request.CreateResponse(HttpStatusCode.Created, Values[id]);
        response.Headers.Location = new Uri(Request.RequestUri, "values/" + id);
        return response;
    }

    // PUT api/values/5
    public HttpResponseMessage Put(int id, [FromBody]string value)
    {
        Values[id - 1] = value;
        return Request.CreateResponse(HttpStatusCode.OK);
    }

    // DELETE api/values/5
    public HttpResponseMessage Delete(int id)
    {
        if (!Values.ContainsKey(id))
            throw new HttpResponseException(HttpStatusCode.NotFound);
        Values.Remove(id - 1);
        return Request.CreateResponse(HttpStatusCode.OK);
    } }

4. Run the app and execute actions
   - Use Fiddler or Google REST client

   GET api/values: get values
   GET api/values/1: get value
   POST api/values "value6"
   PUT api/values/6 "value6a"
   DELETE api/values/6

5. Refactor actions to use IHttpActionResult
   - Change return type of each method to IHttpActionResult
   - Add ResponseType attribute with return type
   - Return NotFound() instead of throwing HttpResponseException
   - Return Ok, Ok<T>
   - POST: return CreatedAtRoute("DefaultApi", new { id }, Values[id])

6. Run the app again and test each method