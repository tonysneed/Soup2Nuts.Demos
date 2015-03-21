using System;
using System.Net.Http;

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
        }
    }
}
