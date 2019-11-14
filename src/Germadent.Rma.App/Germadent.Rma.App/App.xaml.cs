using System;
using System.Windows;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.Views;
using Germadent.ServiceClient.Model;
using Germadent.ServiceClient.Operation;
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

        public App()
        {
            _container = new UnityContainer();

            var dispatcher = new DispatcherAdapter(Application.Current.Dispatcher);

            _container.RegisterType<IRmaAuthorizer, MockRmaAuthorizer>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRmaOperations, MockRmaOperations>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IWindowManager, WindowManager>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IShowDialogAgent, ShowDialogAgent>(new ContainerControlledLifetimeManager());
            _container.RegisterType<MainViewModel, MainViewModel>(new ContainerControlledLifetimeManager());

            _container.RegisterInstance(typeof(IDispatcher), dispatcher);
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            DelegateCommand.CommandException += CommandException;

            var dialogAgent = _container.Resolve<IShowDialogAgent>();
            var authorizationViewModel = new AuthorizationViewModel(
                dialogAgent,                
                _container.Resolve<IRmaAuthorizer>());
            var authorized = true; //dialogAgent.ShowDialog<AuthorizationWindow>(authorizationViewModel);
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
