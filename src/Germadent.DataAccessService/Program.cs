using System;
using Germadent.DataAccessService.Configuration;
using Germadent.DataAccessService.Infrastructure;
using Nancy.Hosting.Self;
using Topshelf;

namespace Germadent.DataAccessService
{
    public class SampleService
    {
        public bool Start()
        {
            return true;
        }

        public bool Stop()
        {
            return true;
        }
    }

    class Program
    {
        private const string ServiceName = "Germadent.DataAccessService";
        private const string ServiceDescription = "Сервис для доступа к данным РМА";

        static void Main(string[] args)
        {
            var resolver = new DataAccessServiceBootstrapper();
            var serviceConfiguration = resolver.GetServiceConfiguration();

            var host = HostFactory.New(hostConfigurator =>
            {
                hostConfigurator.Service<SampleService>(serviceConfigurator =>
                {
                    serviceConfigurator.ConstructUsing(settings => new SampleService());
                    serviceConfigurator.WhenStarted(service => service.Start());
                    serviceConfigurator.WhenStopped(service => service.Stop());
                    serviceConfigurator.WithNancyEndpoint(hostConfigurator, nancy =>
                    {
                        nancy.AddHost(port: serviceConfiguration.Port);
                        nancy.CreateUrlReservationsOnInstall();
                        nancy.OpenFirewallPortsOnInstall(firewallRuleName: ServiceName);
                        nancy.UseBootstrapper(new DataAccessServiceBootstrapper());
                    });
                });
                hostConfigurator.StartAutomatically();
                hostConfigurator.SetServiceName(ServiceName);
                hostConfigurator.SetDisplayName(ServiceName);
                hostConfigurator.SetDescription(ServiceDescription);
                hostConfigurator.RunAsNetworkService();
            });

            host.Run();
        }
    }
}
