using System;
using Microsoft.Owin.Hosting;

namespace OwinSelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:12345/"))
            {
                Console.WriteLine("Web app started ...");
                Console.ReadLine();
            }
        }
    }
}
