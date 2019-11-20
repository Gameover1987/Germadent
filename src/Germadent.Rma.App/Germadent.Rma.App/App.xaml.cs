using System;
using System.Windows;
using Germadent.Rma.App.Configuration;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.ViewModels.Wizard;
using Germadent.Rma.App.Views;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Germadent.UI.Windows;
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

            if (_configuration.WorkMode == WorkMode.Server)
                InitilizeBattle();
            else
                InitializeMock();

            var dispatcher = new DispatcherAdapter(Application.Current.Dispatcher);
            _container.RegisterInstance(typeof(IDispatcher), dispatcher);
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            DelegateCommand.CommandException += CommandException;

            var dialogAgent = _container.Resolve<IShowDialogAgent>();
            var authorizationViewModel = new AuthorizationViewModel(
                dialogAgent,
                _container.Resolve<IRmaAuthorizer>());
            bool? authorized= true;
            if (_configuration.WorkMode == WorkMode.Mock)
            {
                authorized = true;
            }
            else
            {
                //authorized= dialogAgent.ShowDialog<AuthorizationWindow>(authorizationViewModel);
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

        private void InitializeMock()
        {
            _container.RegisterType<IRmaAuthorizer, MockRmaAuthorizer>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRmaOperations, MockRmaOperations>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IShowDialogAgent, ShowDialogAgent>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IMainViewModel, MainViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILabWizardStepsProvider, LabWizardStepsProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IMillingCenterWizardStepsProvider, MillingCenterWizardStepsProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IWindowManager, WindowManager>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOrdersFilterViewModel, OrdersFilterViewModel>(new ContainerControlledLifetimeManager());
        }

        private void InitilizeBattle()
        {
            _container.RegisterType<IRmaAuthorizer, MockRmaAuthorizer>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRmaOperations, RmaOperations>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IShowDialogAgent, ShowDialogAgent>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IMainViewModel, MainViewModel>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILabWizardStepsProvider, LabWizardStepsProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IMillingCenterWizardStepsProvider, MillingCenterWizardStepsProvider>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IWindowManager, WindowManager>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOrdersFilterViewModel, OrdersFilterViewModel>(new ContainerControlledLifetimeManager());
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
