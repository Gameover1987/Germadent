using System;
using System.Windows;
using Germadent.Common.Extensions;
using Germadent.Common.FileSystem;
using Germadent.Common.Logging;
using Germadent.Rma.App.Configuration;
using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.Reporting.TemplateProcessing;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.App.ViewModels.Wizard;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.App.Views;
using Germadent.Rma.App.Views.DesignMock;
using Germadent.UI.Commands;
using Germadent.UI.Controls;
using Germadent.UI.Infrastructure;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace Germadent.Rma.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private IUnityContainer _container;
        private IConfiguration _configuration;

        public App()
        {
            FillContainer();
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            DelegateCommand.CommandException += CommandException;

            var dialogAgent = _container.Resolve<IShowDialogAgent>();
            var authorizationViewModel = new AuthorizationViewModel(dialogAgent, _container.Resolve<IRmaServiceClient>());
            bool? authorized = true;
            if (_configuration.WorkMode == WorkMode.Mock)
            {
                authorized = true;
            }
            else
            {
                //authorized = dialogAgent.ShowDialog<AuthorizationWindow>(authorizationViewModel);
            }

            if (authorized == false)
                Current.Shutdown(0);

            var splashScreenWindow = new SplashScreenWindow();
            splashScreenWindow.DataContext = _container.Resolve<ISplashScreenViewModel>();
            splashScreenWindow.ShowDialog();

            MainWindow = new MainWindow();
            MainWindow.Closed += MainWindowOnClosed;
            MainWindow.DataContext = _container.Resolve<MainViewModel>();
            MainWindow.Show();
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

        private void CommandException(object sender, ExceptionEventArgs e)
        {
            var dialogAgent = _container.Resolve<IShowDialogAgent>();
            dialogAgent.ShowErrorMessageDialog(e.Exception.Message, e.Exception.StackTrace);
        }

        private void MainWindowOnClosed(object sender, EventArgs e)
        {
            Current.Shutdown(0);
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            _container.Dispose();
        }
    }
}
