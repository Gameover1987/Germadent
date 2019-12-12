using System;
using Germadent.DataAccessService.Configuration;
using Germadent.DataAccessService.Entities.Conversion;
using Germadent.DataAccessService.Repository;
using Microsoft.Practices.Unity;
using Nancy.Bootstrappers.Unity;
using Nancy.Configuration;
using Nancy.Hosting.Self;

namespace Germadent.DataAccessService
{
    //public class MyBootstrapper : UnityNancyBootstrapper
    //{
    //    protected override INancyEnvironmentConfigurator GetEnvironmentConfigurator()
    //    {
    //        return this.ApplicationContainer.Resolve<INancyEnvironmentConfigurator>();
    //    }

    //    public override INancyEnvironment GetEnvironment()
    //    {
    //        return this.ApplicationContainer.Resolve<INancyEnvironment>();
    //    }

    //    protected override void RegisterNancyEnvironment(IUnityContainer container, INancyEnvironment environment)
    //    {
    //        container.RegisterInstance(environment);
    //    }

    //    protected override void RegisterBootstrapperTypes(IUnityContainer applicationContainer)
    //    {
    //        applicationContainer.RegisterType<IServiceConfiguration, ServiceConfiguration>(new ContainerControlledLifetimeManager());
    //        applicationContainer.RegisterType<IRmaRepository, RmaRepository>(new ContainerControlledLifetimeManager());
    //        applicationContainer.RegisterType<IEntityToDtoConverter, EntityToDtoConverter>(new ContainerControlledLifetimeManager());
    //    }
    //}

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
