using System;
using Germadent.Common.Extensions;
using Germadent.Rma.App.Infrastructure;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public interface ISplashScreenViewModel
    {
        string Text { get; }

        event EventHandler InitializationCompleted;

        event EventHandler InitializationFailed;
    }

    public class SplashScreenViewModel : ViewModelBase, ISplashScreenViewModel
    {
        private string _text;

        public SplashScreenViewModel(IAppInitializer appInitializer)
        {
            appInitializer.InitializationProgress += AppInitializerOnInitializationProgress;
            appInitializer.InitializationCompleted += AppInitializerOnInitializationCompleted;
            appInitializer.InitializationFailed += AppInitializerOnInitializationFailed;
            ThreadTaskExtensions.Run(appInitializer.Initialize);
        }

        public string Text
        {
            get => _text;
            private set
            {
                _text = value;
                OnPropertyChanged(() => Text);
            }
        }

        public event EventHandler InitializationCompleted;
        public event EventHandler InitializationFailed;

        private void AppInitializerOnInitializationFailed(object sender, EventArgs e)
        {
            ExecuteInUIThread(() =>
            {
                InitializationFailed?.Invoke(this, EventArgs.Empty);
            });
        }

        private void AppInitializerOnInitializationCompleted(object sender, EventArgs e)
        {
            ExecuteInUIThread(() =>
            {
                InitializationCompleted?.Invoke(this, EventArgs.Empty);
            });
        }

        private void AppInitializerOnInitializationProgress(object sender, InitalizationStepEventArgs e)
        {
            ExecuteInUIThread(() =>
            {
                Text = e.Message;
            });
        }
    }
}
