using System;
using System.Collections.Generic;
using System.Text;
using Germadent.Rma.App.Infrastructure;
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
    }

    
}
