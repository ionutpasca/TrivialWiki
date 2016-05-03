using Microsoft.Owin.Hosting;
using System;

namespace TrivialWikiAPI.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://localhost:4606/";
            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine($"Running on {url}");
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
        }
    }
}
