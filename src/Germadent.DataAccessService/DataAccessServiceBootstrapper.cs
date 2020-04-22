using Germadent.Common.FileSystem;
using Germadent.Common.Logging;
using Germadent.DataAccessService.Configuration;
using Germadent.DataAccessService.Entities.Conversion;
using Germadent.DataAccessService.Repository;
using Microsoft.Practices.Unity;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Unity;
using Nancy.Configuration;

namespace Germadent.DataAccessService
{
    public class DataAccessServiceBootstrapper : UnityNancyBootstrapper
    {
        private readonly IUnityContainer _container = new UnityContainer();

        public DataAccessServiceBootstrapper()
        {
            _container.RegisterType<IServiceConfiguration, ServiceConfiguration>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRmaDbOperations, RmaDbOperations>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IEntityToDtoConverter, EntityToDtoConverter>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IFileManager, FileManager>(new ContainerControlledLifetimeManager());

            _container.RegisterType<ILogger, Logger>(new ContainerControlledLifetimeManager());
        }

        protected override INancyEnvironmentConfigurator GetEnvironmentConfigurator()
        {
            throw new System.NotImplementedException();
        }

        public override INancyEnvironment GetEnvironment()
        {
            throw new System.NotImplementedException();
        }

        protected override void RegisterNancyEnvironment(IUnityContainer container, INancyEnvironment environment)
        {
            throw new System.NotImplementedException();
        }
    }
}