using System;
using Germadent.UI.ViewModels;

namespace Germadent.Client.Common.Views.DesignMock
{
    public class DesignMockSplashScreenViewModel : ISplashScreenViewModel
    {
        public DesignMockSplashScreenViewModel()
        {
            Text = "Loading something ...";
        }


        public string Text { get; }
        public event EventHandler InitializationCompleted;
        public event EventHandler InitializationFailed;
    }
}