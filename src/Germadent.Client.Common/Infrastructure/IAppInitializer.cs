using System;

namespace Germadent.Client.Common.Infrastructure
{
    public interface IAppInitializer
    {
        void Initialize();

        event EventHandler<InitalizationStepEventArgs> InitializationProgress;

        event EventHandler InitializationCompleted;

        event EventHandler InitializationFailed;
    }
}