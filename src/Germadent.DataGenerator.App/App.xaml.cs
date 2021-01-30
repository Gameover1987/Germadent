using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Germadent.DataGenerator.App.Infrastructure;
using Germadent.DataGenerator.App.Views;
using Germadent.UI.Commands;

namespace Germadent.DataGenerator.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private UnityResolver _resolver;

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            _resolver = new UnityResolver();

            DelegateCommand.CommandException += CommandException;
            Current.DispatcherUnhandledException += CurrentOnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            var connectionWindow = new ConnectionWindow();
            connectionWindow.DataContext = _resolver.GetConnectionViewModel();
            if (connectionWindow.ShowDialog() == false)
            {
                Current.Shutdown(0);
                return;
            }

            var mainWindow = new MainWindow();
            mainWindow.Closed += MainWindowOnClosed;
            mainWindow.DataContext = _resolver.GetMainViewModel();
            mainWindow.Show();
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Current.Shutdown(-1);
        }

        private void CurrentOnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
       
            Current.Shutdown(-1);
        }

        private void CommandException(object sender, ExceptionEventArgs e)
        {
            var commandExceptionHandler = _resolver.GetCommandExceptionHandler();
            commandExceptionHandler.HandleCommandException(e.Exception);
        }

        private void MainWindowOnClosed(object sender, EventArgs e)
        {
            Current.Shutdown(0);
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            _resolver?.Dispose();
        }
    }
}
