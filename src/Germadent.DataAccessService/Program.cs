using Germadent.DataAccessService.Infrastructure;
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
            var bootstrapper = new DataAccessServiceBootstrapper();
            var serviceConfiguration = bootstrapper.GetServiceConfiguration();

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
