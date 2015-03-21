POCO Entities Demo

NOTE: For generating entities it's convenient to use SQL Server
(Express or Developer/Standard). Use SQL Mant Studio to create
a new database named NorthwindSlim. Then download and run the db
script from http://bit.ly/northwindslim.

A local version of the NorthwindSlim database has been placed
in the App_Data folder of the Web project.

NOTE: Install the Tangible T4 Editor (Tools, Extensions)
      Download and install Fiddler4:
      http://www.telerik.com/download/fiddler

Part A: Use EF with Web API

1. Add a new Class Library project to the solution
   - Name it PocoDemo.Data
   - Add an ADO.NET Entity Data Model
     > Name it NorthwindSlim
     > Select Code First from Database
     > Add a connection to .\sqlexpress, NorthwindSlim database
     > Select Category and Product tables
     
2. Add EF NuGet package to the Web project
   - Reference the Data project from the Web project
     
3. Add a ProductsController to the Web project
   - Right-click the Controllers folder,
     select Add Controller
   - Select Web API 2 Controller - Empty
   
4. Add Get action to retrieve all products,
   sorted by ProductName
   - Include ResponseType attribute
   - Return Task<IHttpActionResult> with async
   - Await call to ToListAsync
   
	[ResponseType(typeof(IEnumerable<Product>))]
	public async Task<IHttpActionResult> Get()
	{
		using (var dbContext = new NorthwindSlim())
		{
			var products = await dbContext.Products
				.OrderBy(p => p.ProductName)
				.ToListAsync();
			return Ok(products);
		}
	}

5. View Web app in browser
   - Test api/Products
   - Notice the error due to dynamic proxies
   - Turn off proxy creation in ctor of NorthwindSlim DbContext class
     > Configuration.ProxyCreationEnabled = false;
     
Part B: Customize T4 Code Gen Templates

1. Add EF T4 Templates to the Data project
   - Add Soup2Nuts.CodeTemplates.EF.CSharp NuGet package
     > Contains modifications to both context and entity templates:
	   + Context class has two partial methods:
	     Initialize
		 ModelCreating
	   + These exist so that config code may be added to a separate
	     partial class
   
2. Add NorthwindSlim.Extensions.cs to the Data project
   - Implement Initialize and ModelCreating methods
   
    public partial class NorthwindSlim
    {
        partial void Initialize()
        {
            // Explicitly disable dynamic proxy generation
            Configuration.ProxyCreationEnabled = false;

            // Instruct Code First to use an existing database
            Database.SetInitializer(new NullDatabaseInitializer<NorthwindSlim>());
        }

        partial void ModelCreating(DbModelBuilder modelBuilder)
        {
            // Remove the pluralizing table name convention
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }

3. Remove classes, then re-add the Data Model
   - Notice that the DbContext and entity classes reflect
     changes to the T4 templates

Part C: Separating Entities from DbContext

1. Add a Portable Class Library project to the solution
   - Name it PocoDemo.Entities
   - Set targets: .NET 4, Win 8, Win Phone SL 8, SL5, Win Phone 8
   - Remove Class1.cs from the project
   
2. Exclude Category and Product classes from the Data project
   - Reference the Entities project from the Data project
   - Reference the Entities project from the Web project
   
3. Build the solution and test the Products controller
   - Go to the api/Products help page and click Test API button

Part D: Handle Cyclical References

1. Modify the Get action of the Products controller
   to include the Category property
   - Test the Products controller again
   - You should receive an exception stating:
     Self referencing loop detected
     
2. Use Attributes to resolve cyclical references
   - Add the Newtonsoft.Json NuGet package to the Entities 
   - Add the following attribute to Category, Products classes:
   [JsonObject(IsReference = true)]
   - Retest the Products controller
     > There should be no exception this time
     
3. Use code to resolve Json cyclical references
   - Remove the Json.Net package and the attributes
     from the Entities project
   - Insert the following code in WebApiConfig.Register:
	 config.Formatters.JsonFormatter.SerializerSettings
		.PreserveReferencesHandling = PreserveReferencesHandling.All;
   - Re-test the Products controller

4. Add a helper library for configuring serializers
   - AspNetWebApi2Helpers.Serialization
   - Replace the above code to use the helper library
     config.Formatters.JsonPreserveReferences();
   
5. Re-test the Products controller by adding an Accept header
   for application/xml
   - The Data Contract Serializer will complain about cycles
   - Add the following code to WebApiConfig:
     config.Formatters.XmlPreserveReferences();

Part E: Binary wire format

1. Test the products controller with fiddler running
   - note the size of the json body payload
   
2. Configure a protobuf formatter to handle cycles:
   - Add NuGet package: AspNetWebApi2Helpers.Serialization.Protobuf
   - Configure the protobuf formatter as follows:
   
	var protoFormatter = new ProtoBufFormatter();
	protoFormatter.ProtobufPreserveReferences(typeof(Category)
		.Assembly.GetTypes());
	config.Formatters.Add(protoFormatter);
	
Part F: Client
   
1. Add a new console project: PocoDemo.Client
   - Add the NuGet packages:
     AspNetWebApi2Helpers.Serialization
	 AspNetWebApi2Helpers.Serialization.Protobuf
   - Reference the Entities project
   
2. Add the following code:

    // Set base address to optionally use Fiddler
    Console.WriteLine("Use Fiddler?");
    bool useFiddler = Console.ReadLine().ToUpper() == "Y";
    string address = string.Format("http://localhost{0}:51245/api/",
        useFiddler ? ".fiddler" : string.Empty);

    // Create an http client with service base address
    var client = new HttpClient { BaseAddress = new Uri(address) };
	HttpResponseMessage response = client.GetAsync("products").Result;
	response.EnsureSuccessStatusCode();
	var products = response.Content.ReadAsAsync<List<Product>>().Result;
	foreach (var p in products)
	{
		Console.WriteLine("{0} {1} {2} {3}",
			p.ProductId,
			p.ProductName,
			p.UnitPrice.GetValueOrDefault().ToString("C"),
			p.Category.CategoryName);
	}

   - Run the browser client to test it
   
3. Prompt the user for a media type
   - Set the media type formatter and accept header value

	// Prompt user for media type
	Console.WriteLine("Select media type: {1} Xml, {2} Json, {3} Bson, {4} Protobuf");
	int selection = int.Parse(Console.ReadLine());

	// Configure accept header and media type formatter
	MediaTypeFormatter formatter;
	string acceptHeader;
	switch (selection)
	{
        case 1:
            formatter = new XmlMediaTypeFormatter();
            ((XmlMediaTypeFormatter)formatter).XmlPreserveReferences
                (typeof(Category), typeof(List<Product>));
            acceptHeader = "application/xml";
            break;
        case 2:
            formatter = new JsonMediaTypeFormatter();
            ((JsonMediaTypeFormatter)formatter).JsonPreserveReferences();
            acceptHeader = "application/json";
            break;
        case 3:
            formatter = new ProtoBufFormatter();
            ((ProtoBufFormatter)formatter).ProtobufPreserveReferences
                (typeof(Category).Assembly.GetTypes());
            acceptHeader = "application/x-protobuf";
            break;
		default:
			Console.WriteLine("Invalid selection: {0}", selection);
			return;
	}
   
