using Germadent.DataAccessService.Configuration;
using Germadent.DataAccessService.Entities.Conversion;
using Germadent.DataAccessService.Repository;
using Microsoft.Practices.Unity;
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
        }

        protected override IUnityContainer GetApplicationContainer()
        {
            return _container;
        }

        public IServiceConfiguration GetServiceConfiguration()
        {
            return _container.Resolve<IServiceConfiguration>();
        }
    }
}