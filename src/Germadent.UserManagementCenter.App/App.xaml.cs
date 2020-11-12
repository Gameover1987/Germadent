using System;
using System.Windows;
using Germadent.Common.FileSystem;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Germadent.UserManagementCenter.App.Configuration;
using Germadent.UserManagementCenter.App.ServiceClient;
using Germadent.UserManagementCenter.App.UIOperations;
using Germadent.UserManagementCenter.App.ViewModels;
using Germadent.UserManagementCenter.App.Views;
using Unity;
using Unity.Lifetime;

namespace Germadent.UserManagementCenter.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IUnityContainer _container = new UnityContainer();

        public App()
        {
            var dispatcher = new DispatcherAdapter(Application.Current.Dispatcher);
            _container.RegisterInstance(typeof(IDispatcher), dispatcher);

            _container.RegisterSingleton<IUmcConfiguration, UmcConfiguration>();
            _container.RegisterSingleton<IMainViewModel, MainViewModel>();
            _container.RegisterSingleton<IUsersManagerViewModel, UsersManagerViewModel>();
            _container.RegisterSingleton<IUmcServiceClient, UmcServiceClient>();
            _container.RegisterSingleton<IRolesManagerViewModel, RolesManagerViewModel>();
            _container.RegisterSingleton<IShowDialogAgent, ShowDialogAgent>();
            _container.RegisterSingleton<IUserManagementUIOperations, UserManagementUIOperations>();
            _container.RegisterType<IAddUserViewModel, AddUserViewModel>(new TransientLifetimeManager());
            _container.RegisterSingleton<IShowDialogAgent, ShowDialogAgent>();
            _container.RegisterSingleton<IFileManager, FileManager>();
            _container.RegisterType<IAddRoleViewModel, AddRoleViewModel>(new TransientLifetimeManager());
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            DelegateCommand.CommandException += CommandException;

            MainWindow = new MainWindow();
            MainWindow.Closed += MainWindowOnClosed;
            MainWindow.DataContext = _container.Resolve<IMainViewModel>();
            MainWindow.Show();
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
