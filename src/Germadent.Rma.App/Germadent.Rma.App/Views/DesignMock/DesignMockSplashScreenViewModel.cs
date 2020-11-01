using System;
using Germadent.Rma.App.ViewModels;

namespace Germadent.Rma.App.Views.DesignMock
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