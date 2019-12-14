using Germadent.DataAccessService.Configuration;
using Germadent.DataAccessService.Entities.Conversion;
using Germadent.DataAccessService.Repository;
using Microsoft.Practices.Unity;
using Nancy.Bootstrappers.Unity;

namespace Germadent.DataAccessService
{
    public class DataAccessServiceBootstrapper : UnityNancyBootstrapper
    {
        protected override void RegisterBootstrapperTypes(IUnityContainer applicationContainer)
        {
            base.RegisterBootstrapperTypes(applicationContainer);
            applicationContainer.RegisterType<IServiceConfiguration, ServiceConfiguration>(new ContainerControlledLifetimeManager());
            applicationContainer.RegisterType<IRmaRepository, RmaRepository>(new ContainerControlledLifetimeManager());
            applicationContainer.RegisterType<IEntityToDtoConverter, EntityToDtoConverter>(new ContainerControlledLifetimeManager());
        }
    }
}