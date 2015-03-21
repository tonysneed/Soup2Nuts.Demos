using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set base address to optionally use Fiddler
            Console.WriteLine("Use Fiddler?");
            bool useFiddler = Console.ReadLine().ToUpper() == "Y";
            string address = string.Format("http://localhost{0}:57974/api/values/",
                useFiddler ? ".fiddler" : string.Empty);

            // Create client
            var client = new HttpClient { BaseAddress = new Uri(address) };

            // Get values
            var response = client.GetAsync(string.Empty).Result;
            response.EnsureSuccessStatusCode();
            var values = response.Content.ReadAsAsync<List<string>>().Result;
            values.ForEach(s => Console.WriteLine(s));

            // Create value
            Console.WriteLine("\nEnter new value:");
            var value = Console.ReadLine();
            response = client.PostAsJsonAsync(string.Empty, value).Result;
            response.EnsureSuccessStatusCode();
            value = response.Content.ReadAsAsync<string>().Result;
            Console.WriteLine(value);

            // Update value
            Console.WriteLine("\nId of item to update (int):");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("\nEnter updated value:");
            value = Console.ReadLine();
            response = client.PutAsJsonAsync(id.ToString(), value).Result;
            response.EnsureSuccessStatusCode();
            value = response.Content.ReadAsAsync<string>().Result;
            Console.WriteLine(value);

            // Delete value
            Console.WriteLine("\nId of item to delete (int):");
            id = int.Parse(Console.ReadLine());
            response = client.DeleteAsync(id.ToString()).Result;
            response.EnsureSuccessStatusCode();

            // Verify delete
            response = client.GetAsync(id.ToString()).Result;
            bool deleted = response.StatusCode == HttpStatusCode.NotFound;
            Console.WriteLine("Item deleted: {0}", deleted);
        }
    }
}
