HttpClient Demo ReadMe

This demonstrates usage of HttpClient to consume a Web API service.

NOTE: First install Fiddler.

1. Create a Console app, add the following NuGet packages:
   - Microsoft.AspNet.WebApi.Client

2. Prompt user to optionally format host name for Fiddler

    Console.WriteLine("Use Fiddler?");
    bool useFiddler = Console.ReadLine().ToUpper() == "Y";
    string address = string.Format("http://localhost{0}:57974/api/values/",
        useFiddler ? ".fiddler" : string.Empty);

3. Create an HttpClient with base address uri

	var client = new HttpClient { BaseAddress = new Uri(address) };

	- Call GetAsync, then read the content into a List<string> and print

    var response = client.GetAsync(string.Empty).Result;
	response.EnsureSuccessStatusCode();
    var values = response.Content.ReadAsAsync<List<string>>().Result;
    values.ForEach(s => Console.WriteLine(s));

4. Add code to create a new value

    Console.WriteLine("\nEnter new value:");
    var value = Console.ReadLine();
    response = client.PostAsJsonAsync(string.Empty, value).Result;
    response.EnsureSuccessStatusCode();
    value = response.Content.ReadAsAsync<string>().Result;
    Console.WriteLine(value);

5. Add code to update an existing value

    Console.WriteLine("\nId of item to update (int):");
    int id = int.Parse(Console.ReadLine());
    Console.WriteLine("\nEnter updated value:");
    value = Console.ReadLine();
    response = client.PutAsJsonAsync(id.ToString(), value).Result;
    response.EnsureSuccessStatusCode();
    value = response.Content.ReadAsAsync<string>().Result;
    Console.WriteLine(value);

6. Add code to delete a value

    Console.WriteLine("\nId of item to delete (int):");
    id = int.Parse(Console.ReadLine());
    response = client.DeleteAsync(id.ToString()).Result;
    response.EnsureSuccessStatusCode();

7. Add code to verify item was deleted

    response = client.GetAsync(id.ToString()).Result;
    bool deleted = response.StatusCode == HttpStatusCode.NotFound;
    Console.WriteLine("Item deleted: {0}", deleted);

8. Run the service, then the client
   - Inspect traffic using Fiddler

