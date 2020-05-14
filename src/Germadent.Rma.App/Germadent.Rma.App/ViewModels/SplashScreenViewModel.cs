using System;
using System.Collections.Generic;
using System.Text;
using Germadent.Common.Extensions;
using Germadent.Rma.App.Infrastructure;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public interface ISplashScreenViewModel
    {
        string Text { get; }

        event EventHandler InitializationCompleted;
    }

    public class SplashScreenViewModel : ViewModelBase, ISplashScreenViewModel
    {
        private string _text;

        public SplashScreenViewModel(IAppInitializer appInitializer)
        {
            appInitializer.InitializationProgress += AppInitializerOnInitializationProgress;
            appInitializer.InitializationCompleted += AppInitializerOnInitializationCompleted;
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
