using System;
using System.Windows;
using Germadent.CorrectionConstructionFile.App.Model;
using Germadent.CorrectionConstructionFile.App.View;
using Germadent.CorrectionConstructionFile.App.ViewModel;
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

            MainWindow = new MainWindow();
            MainWindow.Closed += MainWindowOnClosed;
            MainWindow.DataContext = _container.Resolve<IMainViewModel>();
            MainWindow.Show();
        }

        private static IUnityContainer RegisterTypes()
        {
            var container = new UnityContainer();
            container.RegisterSingleton<IMainViewModel, CorconViewModel>();
            container.RegisterSingleton<IXmlDocumentProcessor, XmlDocumentProcessor>();

            return container;
        }


        private void MainWindowOnClosed(object? sender, EventArgs e)
        {
            App.Current.Shutdown(0);
        }
    }
}
