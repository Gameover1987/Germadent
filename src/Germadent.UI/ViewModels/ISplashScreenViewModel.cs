using System;

namespace Germadent.UI.ViewModels
{
    public interface ISplashScreenViewModel
    {
        string Text { get; }

        event EventHandler InitializationCompleted;

        event EventHandler InitializationFailed;
    }
}