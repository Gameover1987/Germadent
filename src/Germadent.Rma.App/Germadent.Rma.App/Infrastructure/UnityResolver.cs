using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Germadent.Common.FileSystem;
using Germadent.Common.Logging;
using Germadent.Rma.App.Configuration;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.Operations;
using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.Reporting.TemplateProcessing;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.App.ViewModels.Wizard;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.App.Views.DesignMock;
using Germadent.UI.Infrastructure;
using Unity;
using Unity.Lifetime;

namespace Germadent.Rma.App.Infrastructure
{
    public class UnityResolver :IDisposable
    {
        private IUnityContainer _container;
        private IConfiguration _configuration;

        public UnityResolver()
        {
            FillContainer();
        }

        public ISplashScreenViewModel GetSplashScreenViewModel()
        {
            return _container.Resolve<ISplashScreenViewModel>();
        }

        public IMainViewModel GetMainViewModel()
        {
            return _container.Resolve<IMainViewModel>();
        }

        public ICommandExceptionHandler GetCommandExceptionHandler()
        {
            return _container.Resolve<ICommandExceptionHandler>();
        }

        public ILogger GetLogger()
        {
            return _container.Resolve<ILogger>();
        }

        private void FillContainer()
        {
            _container = new UnityContainer();

            _configuration = new RmaConfiguration();
            _container.RegisterInstance<IConfiguration>(_configuration, new ContainerControlledLifetimeManager());

            var dispatcher = new DispatcherAdapter(Application.Current.Dispatcher);
            _container.RegisterInstance(typeof(IDispatcher), dispatcher);
            _container.RegisterType<IAppInitializer, AppInitializer>();
            _container.RegisterType<ISplashScreenViewModel, SplashScreenViewModel>();

            if (_configuration.WorkMode == WorkMode.Server)
            {
                RegisterServiceClient();
            }
            else
            {
                _container.RegisterType<IRmaServiceClient, DesignMockRmaServiceClient>(new ContainerControlledLifetimeManager());
                _container.RegisterType<ICustomerRepository, DesignMockCustomerRepository>(new ContainerControlledLifetimeManager());
            }

            RegisterCommonComponents();
            RegisterPrintModule();
            RegisterViewModels();
        }

        private void RegisterViewModels()
        {
            _container.RegisterType<IShowDialogAgent, ShowDialogAgent>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOrderUIOperations, OrderUIOperations>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICatalogUIOperations, CatalogUIOperations>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICatalogSelectionUIOperations, CatalogSelectionUIOperations>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IMainViewModel, MainViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILabWizardStepsProvider, LabWizardStepsProvider>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IAddCustomerViewModel, AddCustomerViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAddResponsiblePersonViewModel, AddResponsiblePersonViewModel>(new ContainerControlledLifetimeManager());

            _container.RegisterType<ICustomerCatalogViewModel, CustomerCatalogViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IResponsiblePersonCatalogViewModel, ResponsiblePersonsCatalogViewModel>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IOrderFilesContainerViewModel, OrderFilesContainerViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICustomerSuggestionProvider, CustomerSuggestionProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IResponsiblePersonsSuggestionsProvider, ResponsiblePersonSuggestionProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IMillingCenterWizardStepsProvider, MillingCenterWizardStepsProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IToothCardViewModel, ToothCardViewModel>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IOrdersFilterViewModel, OrdersFilterViewModel>(new ContainerControlledLifetimeManager());
        }

        private void RegisterCommonComponents()
        {
            _container.RegisterType<IFileManager, FileManager>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILogger, Logger>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IClipboardHelper, ClipboardHelper>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IReporter, ClipboardReporter>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommandExceptionHandler, CommandExceptionHandler>(new ContainerControlledLifetimeManager());
        }

        private void RegisterServiceClient()
        {
            _container.RegisterType<IRmaServiceClient, RmaServiceClient>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICustomerRepository, CustomerRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IResponsiblePersonRepository, ResponsiblePersonRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IDictionaryRepository, DictionaryRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRepositoryContainer, RepositoryContainer>(new ContainerControlledLifetimeManager());
        }

        private void RegisterPrintModule()
        {
            _container.RegisterType<IWordAssembler, WordJsonAssembler>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IPrintModule, PrintModule>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IPrintableOrderConverter, PrintableOrderConverter>(new ContainerControlledLifetimeManager());
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
