using System;

namespace Germadent.Rma.App.Infrastructure
{
    public interface IAppInitializer
    {
        void Initialize();

        event EventHandler<InitalizationStepEventArgs> InitializationProgress;

        event EventHandler InitializationCompleted;

        event EventHandler InitializationFailed;
    }
}