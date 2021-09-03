using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using Germadent.Common.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;

namespace Germadent.WebApi
{
    internal class CustomWebHostService : WebHostService
    {
        private readonly ILogger _logger;

        public CustomWebHostService(IWebHost host) 
            : base(host)
        {
            _logger = (ILogger)host.Services.GetService(typeof(ILogger));
        }

        protected override void OnStarting(string[] args)
        {
            _logger.Info($"------- Запуск (версия {Assembly.GetExecutingAssembly().GetName().Version}) -------");
            base.OnStarting(args);
        }

        protected override void OnStopping()
        {
            _logger.Info("------- Закрытие -------");
            base.OnStopping();
        }
    }

    public static class CustomWebHostWindowsServiceExtensions
    {
        public static void RunAsCustomService(this IWebHost host)
        {
            var webHostService = new CustomWebHostService(host);
            ServiceBase.Run(webHostService);
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            var webHost = BuildWebHost(args);
            var logger = (ILogger)webHost.Services.GetService(typeof(ILogger));
            try
            {

                var isService = !(Debugger.IsAttached || args.Contains("c"));
                if (isService)
                {
                    webHost.RunAsCustomService();
                }
                else
                {
                    webHost.Run();
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception);
            }
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var path = "";
#if DEBUG
            path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

#else
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            path = Path.GetDirectoryName(pathToExe);
            Directory.SetCurrentDirectory(path);
#endif

            var appConfiguration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            return new WebHostBuilder()
                .UseConfiguration(appConfiguration)
                .UseKestrel(options => options.Limits.KeepAliveTimeout = new TimeSpan(1, 0, 0))
                .UseContentRoot(path)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
        }
    }
}
