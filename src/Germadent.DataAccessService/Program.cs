using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Configuration;
using Nancy.Hosting.Self;

namespace Germadent.DataAccessService
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = new Uri("http://localhost:4444");
            using (var host = new NancyHost(url))
            {
                host.Start();
                Console.WriteLine("Service started at {0}", url);
                Console.ReadKey();
            }
        }
    }
}
