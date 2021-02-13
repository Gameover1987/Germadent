using System;
using System.Windows;
using Germadent.Common.FileSystem;
using Germadent.Common.Logging;
using Germadent.CorrectionConstructionFile.App.Model;
using Germadent.CorrectionConstructionFile.App.View;
using Germadent.CorrectionConstructionFile.App.ViewModel;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Unity;

namespace Germadent.CorrectionConstructionFile.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IUnityContainer _container;

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            _container = RegisterTypes();

            DelegateCommand.CommandException += CommandException;

            MainWindow = new MainWindow();
            MainWindow.Closed += MainWindowOnClosed;
            MainWindow.DataContext = _container.Resolve<IMainViewModel>();
            MainWindow.Show();
        }

        private void CommandException(object? sender, ExceptionEventArgs e)
        {
            var dialogAgent = _container.Resolve<IShowDialogAgent>();
            dialogAgent.ShowErrorMessageDialog(e.Exception.Message, e.Exception.StackTrace);
        }

        private static IUnityContainer RegisterTypes()
        {
            var container = new UnityContainer();
            container.RegisterSingleton<IMainViewModel, MainViewModel>();
            container.RegisterSingleton<IFileManager, FileManager>();

            var dispatcher = new DispatcherAdapter(Application.Current.Dispatcher);
            container.RegisterInstance(typeof(IDispatcher), dispatcher);

            container.RegisterSingleton<IShowDialogAgent, ShowDialogAgent>();
            container.RegisterSingleton<IAddDictionaryItemViewModel, AddDictionaryItemViewModel>();
            container.RegisterSingleton<IXmlDocumentProcessor, XmlDocumentProcessor>();

            return container;
        }


        private void MainWindowOnClosed(object? sender, EventArgs e)
        {
            App.Current.Shutdown(0);
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            _container?.Dispose();
        }
    }
}
