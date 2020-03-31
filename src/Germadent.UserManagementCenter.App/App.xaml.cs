using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Germadent.UserManagementCenter.App.ServiceClient;
using Germadent.UserManagementCenter.App.ViewModels;
using Germadent.UserManagementCenter.App.Views;
using Unity;

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
            _container.RegisterSingleton<IMainViewModel, MainViewModel>();
            _container.RegisterSingleton<IUsersManagerViewModel, UsersManagerViewModel>();
            _container.RegisterSingleton<IUserManagementCenterOperations, UserManagementCenterOperations>();
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
