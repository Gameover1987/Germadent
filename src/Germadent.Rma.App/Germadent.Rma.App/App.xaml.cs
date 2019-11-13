using System;
using System.Windows;
using Germadent.Rma.App.Model;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.Views;
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

            _container.RegisterType<IRmaOperations, RmaOperations>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IShowDialogAgent, ShowDialogAgent>(new ContainerControlledLifetimeManager());
            _container.RegisterType<MainViewModel, MainViewModel>(new ContainerControlledLifetimeManager());

            _container.RegisterInstance(typeof(IDispatcher), dispatcher);
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var dialogAgent = _container.Resolve<IShowDialogAgent>();
            var authorizationViewModel = new AuthorizationViewModel(
                dialogAgent,                
                _container.Resolve<IRmaOperations>());
            var authorized = dialogAgent.ShowDialog<AuthorizationWindow>(authorizationViewModel);
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
