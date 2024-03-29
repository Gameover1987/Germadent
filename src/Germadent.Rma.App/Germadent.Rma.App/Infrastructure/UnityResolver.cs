﻿using System;
using System.Windows;
using Germadent.Client.Common.Configuration;
using Germadent.Client.Common.Infrastructure;
using Germadent.Client.Common.Reporting;
using Germadent.Client.Common.Reporting.TemplateProcessing;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Notifications;
using Germadent.Client.Common.ServiceClient.Repository;
using Germadent.Client.Common.ViewModels;
using Germadent.Client.Common.ViewModels.Salary;
using Germadent.Common;
using Germadent.Common.FileSystem;
using Germadent.Common.Logging;
using Germadent.Model;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.Operations;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.ViewModels.Pricing;
using Germadent.Rma.App.ViewModels.Salary;
using Germadent.Rma.App.ViewModels.TechnologyOperation;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.App.ViewModels.Wizard;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.App.Views.DesignMock;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;
using Unity;
using Unity.Lifetime;
using ISplashScreenViewModel = Germadent.UI.ViewModels.ISplashScreenViewModel;
using OrdersFilterViewModel = Germadent.Rma.App.ViewModels.OrdersFilterViewModel;

namespace Germadent.Rma.App.Infrastructure
{
    public class UnityResolver : IDisposable
    {
        private IUnityContainer _container;
        private IClientConfiguration _configuration;

        public UnityResolver()
        {
            FillContainer();
        }

        public ISplashScreenViewModel GetSplashScreenViewModel()
        {
            return _container.Resolve<ISplashScreenViewModel>();
        }

        public IAuthorizationViewModel GetAuthorizationViewModel()
        {
            return _container.Resolve<IAuthorizationViewModel>();
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

            _configuration = new ClientConfiguration();
            _container.RegisterInstance<IClientConfiguration>(_configuration, new ContainerControlledLifetimeManager());

            var dispatcher = new DispatcherAdapter(Application.Current.Dispatcher);
            _container.RegisterInstance(typeof(IDispatcher), dispatcher);
            _container.RegisterType<IAppInitializer, AppInitializer>();
            _container.RegisterType<ISplashScreenViewModel, SplashScreenViewModel>();
            _container.RegisterType<IAuthorizationViewModel, AuthorizationViewModel>();
            _container.RegisterType<IUserManager, RmaUserManager>();
            _container.RegisterType<IRmaUserSettingsManager, RmaUserSettingsManager>(new ContainerControlledLifetimeManager());
            
            RegisterServiceClient();

            RegisterCommonComponents();
            RegisterPrintModule();
            RegisterViewModels();
        }

        private void RegisterViewModels()
        {
            _container.RegisterType<IDateTimeProvider, DateTimeProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IUiTimer, UiTimer>(new TransientLifetimeManager());

            _container.RegisterType<IShowDialogAgent, ShowDialogAgent>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IEnvironment, WpfEnvironment>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOrderUIOperations, OrderUIOperations>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICatalogUIOperations, CatalogUIOperations>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICatalogSelectionUIOperations, CatalogSelectionUIOperations>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IMainViewModel, MainViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILabWizardStepsProvider, LabWizardStepsProvider>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IAddCustomerViewModel, AddCustomerViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAddResponsiblePersonViewModel, AddResponsiblePersonViewModel>(new ContainerControlledLifetimeManager());

            _container.RegisterType<ICustomerCatalogViewModel, CustomerCatalogViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IResponsiblePersonCatalogViewModel, ResponsiblePersonCatalogViewModel>(new ContainerControlledLifetimeManager());

            _container.RegisterType<ICustomerSuggestionProvider, CustomerSuggestionProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IResponsiblePersonsSuggestionsProvider, ResponsiblePersonSuggestionProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IMillingCenterWizardStepsProvider, MillingCenterWizardStepsProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IToothCardViewModel, ToothCardViewModel>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IOrdersFilterViewModel, OrdersFilterViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IPriceListViewModel, PriceListViewModel>(new TransientLifetimeManager());
            _container.RegisterType<IPriceListEditorContainerViewModel, PriceListEditorContainerViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAddPricePositionViewModel, AddPricePositionViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAddPriceViewModel, AddPriceViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IPriceListEditorFactory, PriceListEditorFactory>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ITechnologyOperationsEditorViewModel, TechnologyOperationsEditorViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAddTechnologyOperationViewModel, AddTechnologyOperationViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAddRateViewModel, AddRateViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISalaryCalculationViewModel, SalaryCalculationViewModel>(new ContainerControlledLifetimeManager());
        }

        private void RegisterCommonComponents()
        {
            _container.RegisterType<IFileManager, FileManager>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILogger, Logger>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IClipboardHelper, ClipboardHelper>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommandExceptionHandler, CommandExceptionHandler>(new ContainerControlledLifetimeManager());
        }

        private void RegisterServiceClient()
        {
            _container.RegisterType<ICustomerRepository, CustomerRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IResponsiblePersonRepository, ResponsiblePersonRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IDictionaryRepository, DictionaryRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IPriceGroupRepository, PriceGroupRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IPricePositionRepository, PricePositionRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProductRepository, ProductRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAttributeRepository, AttributeRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IEmployeePositionRepository, EmployeePositionRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ITechnologyOperationRepository, TechnologyOperationRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISignalRClient, SignalRClient>(new ContainerControlledLifetimeManager());

            var signalR = _container.Resolve<ISignalRClient>();
            var rmaServiceClient = new RmaServiceClient(_configuration, signalR);
            _container.RegisterInstance<IRmaServiceClient>(rmaServiceClient);
            _container.RegisterInstance<IBaseClientOperationsServiceClient>(rmaServiceClient);
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
