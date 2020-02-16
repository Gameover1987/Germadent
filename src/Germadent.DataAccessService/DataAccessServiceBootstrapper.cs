using Germadent.Common.Logging;
using Germadent.DataAccessService.Configuration;
using Germadent.DataAccessService.Entities.Conversion;
using Germadent.DataAccessService.Repository;
using Microsoft.Practices.Unity;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Unity;

namespace Germadent.DataAccessService
{
    public class DataAccessServiceBootstrapper : UnityNancyBootstrapper
    {
        private readonly IUnityContainer _container = new UnityContainer();

        public DataAccessServiceBootstrapper()
        {
            _container.RegisterType<IServiceConfiguration, ServiceConfiguration>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRmaRepository, RmaRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IEntityToDtoConverter, EntityToDtoConverter>(new ContainerControlledLifetimeManager());

            _container.RegisterType<ILogger, Logger>(new ContainerControlledLifetimeManager());
        }

        protected override void ApplicationStartup(IUnityContainer container, IPipelines pipelines)
        {
            Nancy.Json.JsonSettings.MaxJsonLength = int.MaxValue;
            Nancy.Json.JsonSettings.MaxRecursions = 100;
            Nancy.Json.JsonSettings.RetainCasing = false;
            base.ApplicationStartup(container, pipelines);          
        }

        protected override IUnityContainer GetApplicationContainer()
        {
            return _container;
        }

        public IServiceConfiguration GetServiceConfiguration()
        {
            return _container.Resolve<IServiceConfiguration>();
        }

        public ILogger GetLogger()
        {
            return _container.Resolve<ILogger>();
        }
    }
}