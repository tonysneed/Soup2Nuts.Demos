﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using AspnetWebApi2Helpers.Serialization;
using AspnetWebApi2Helpers.Serialization.Protobuf;
using PocoDemo.Data;
using WebApiContrib.Formatting;
using System.Net.Http.Headers;

namespace PocoDemo.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // Prompt user for media type
            Console.WriteLine("Select media type: {1} Xml, {2} Json, {3} Protobuf");
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

            // Set base address to optionally use Fiddler
            Console.WriteLine("\nUse Fiddler?");
            bool useFiddler = Console.ReadLine().ToUpper() == "Y";
            string address = string.Format("http://localhost{0}:51246/api/",
                useFiddler ? ".fiddler" : string.Empty);

            // Create an http client with service base address
            var client = new HttpClient { BaseAddress = new Uri(address) };

            // Set request accept header
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptHeader));

            // Get values response
            HttpResponseMessage response = client.GetAsync("products").Result;
            response.EnsureSuccessStatusCode();

            // Read response content as string array
            var products = response.Content.ReadAsAsync<List<Product>>
                (new[] { formatter }).Result;
            foreach (var p in products)
            {
                Console.WriteLine("{0} {1} {2} {3}",
                    p.ProductId,
                    p.ProductName,
                    p.UnitPrice.GetValueOrDefault().ToString("C"),
                    p.Category.CategoryName);
            }
        }
    }
}
