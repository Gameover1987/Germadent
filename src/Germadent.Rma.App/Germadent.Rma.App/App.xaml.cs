using System;
using System.Windows;
using Germadent.Common.FileSystem;
using Germadent.Common.Logging;
using Germadent.Rma.App.Configuration;
using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.Reporting.TemplateProcessing;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.App.ViewModels.Wizard;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.App.Views;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Unity;
using Unity.Lifetime;

namespace Germadent.Rma.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IUnityContainer _container;
        private readonly IConfiguration _configuration;

        public App()
        {
            _container = new UnityContainer();

            _configuration = new RmaConfiguration();
            _container.RegisterInstance<IConfiguration>(_configuration, new ContainerControlledLifetimeManager());

            var dispatcher = new DispatcherAdapter(Application.Current.Dispatcher);
            _container.RegisterInstance(typeof(IDispatcher), dispatcher);

            FillContainer();
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            DelegateCommand.CommandException += CommandException;

            var dialogAgent = _container.Resolve<IShowDialogAgent>();
            var authorizationViewModel = new AuthorizationViewModel(dialogAgent, _container.Resolve<IRmaAuthorizer>());
            bool? authorized = true;
            if (_configuration.WorkMode == WorkMode.Mock)
            {
                authorized = true;
            }
            else
            {
                //authorized = dialogAgent.ShowDialog<AuthorizationWindow>(authorizationViewModel);
            }

            if (authorized == true)
            {
                MainWindow = new MainWindow();
                MainWindow.Closed += MainWindowOnClosed;
                MainWindow.DataContext = _container.Resolve<MainViewModel>();
                MainWindow.Show();
            }
            else
            {
                Current.Shutdown(0);
            }
        }

        private void FillContainer()
        {
            if (_configuration.WorkMode == WorkMode.Server)
            {
                RegisterServiceComponents();
            }
            else
            {
                _container.RegisterType<IRmaAuthorizer, DesignMockRmaAuthorizer>(new ContainerControlledLifetimeManager());
                _container.RegisterType<IRmaOperations, DesignMockRmaOperations>(new ContainerControlledLifetimeManager());
            }

            RegisterViewModels();
            RegisterCommonComponents();
            RegisterPrintModule();
        }

        private void RegisterViewModels()
        {
            _container.RegisterType<IShowDialogAgent, ShowDialogAgent>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IWindowManager, WindowManager>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IMainViewModel, MainViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILabWizardStepsProvider, LabWizardStepsProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IMillingCenterWizardStepsProvider, MillingCenterWizardStepsProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOrdersFilterViewModel, OrdersFilterViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IToothCardViewModel, ToothCardViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOrderFilesContainerViewModel, OrderFilesContainerViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICustomerCatalogViewModel, CustomerCatalogViewModel>(new ContainerControlledLifetimeManager());

        }

        private void RegisterCommonComponents()
        {
            _container.RegisterType<IFileManager, FileManager>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILogger, Logger>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IReporter, ClipboardReporter>(new ContainerControlledLifetimeManager());
        }

        private void RegisterServiceComponents()
        {
            _container.RegisterType<IRmaAuthorizer, DesignMockRmaAuthorizer>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRmaOperations, RmaOperations>(new ContainerControlledLifetimeManager());
        }

        private void RegisterPrintModule()
        {
            _container.RegisterType<IWordAssembler, WordJsonAssembler>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IPrintModule, PrintModule>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IPrintableOrderConverter, PrintableOrderConverter>(
                new ContainerControlledLifetimeManager());
            _container.RegisterType<IClipboardWorks, ClipboardWorks>(new ContainerControlledLifetimeManager());
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
