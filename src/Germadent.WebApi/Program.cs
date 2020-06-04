using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Hosting;

namespace Germadent.WebApi
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Contains("c"))
            {
                RunAsConsoleApp();
            }
            else
            {
                RunAsService();
            }
        }

        private static void RunAsConsoleApp()
        {
            CreateHostBuilder().Build().Run();
        }

        private static void RunAsService()
        {
            // получаем путь к файлу 
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;

            // путь к каталогу проекта
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);

            // создаем хост
            var host = WebHost.CreateDefaultBuilder()
                .UseContentRoot(pathToContentRoot)
                .UseStartup<Startup>()
                .Build();

            // запускаем в виде службы
            host.RunAsService();
        }

        private static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder().ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}
