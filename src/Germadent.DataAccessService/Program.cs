using System;
using Germadent.DataAccessService.Configuration;
using Nancy.Hosting.Self;

namespace Germadent.DataAccessService
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ServiceConfiguration();
            using (var host = new NancyHost(new Uri(configuration.Url)))
            {
                host.Start();
                Console.WriteLine("Service started at {0}", configuration.Url);
                Console.ReadKey();
            }
        }
    }
}
