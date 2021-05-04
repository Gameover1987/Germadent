using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Germadent.Client.Common.Views;
using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.Views;
using Germadent.UI.Commands;
using Germadent.UI.Windows;

namespace Germadent.Rma.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private UnityResolver _resolver;

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            _resolver = new UnityResolver();

            DelegateCommand.CommandException += CommandException;
            Current.DispatcherUnhandledException += CurrentOnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException+= CurrentDomainOnUnhandledException;

            var authorizationViewModel = _resolver.GetAuthorizationViewModel();
            var authorizationWindow = new AuthorizationWindow();
            authorizationWindow.DataContext = authorizationViewModel;
            if (authorizationWindow.ShowDialog() == false)
            {
                Current.Shutdown(-1);
                return;
            }

            var splashScreenWindow = new SplashScreenWindow();
            splashScreenWindow.DataContext = _resolver.GetSplashScreenViewModel();
            if (splashScreenWindow.ShowDialog() == false)
            {
                Current.Shutdown(-1);
                return;
            }

            MainWindow = new MainWindow();
            MainWindow.Closed += MainWindowOnClosed;
            MainWindow.DataContext = _resolver.GetMainViewModel();
            MainWindow.Show();
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception exception)
            {
                var logger = _resolver.GetLogger();
                logger.Fatal(exception);
            }
            
            Current.Shutdown(-1);
        }

        private void CurrentOnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var logger = _resolver.GetLogger();
            logger.Fatal(e.Exception);
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
