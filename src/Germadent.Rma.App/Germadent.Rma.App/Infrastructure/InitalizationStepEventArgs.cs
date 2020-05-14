using System;

namespace Germadent.Rma.App.Infrastructure
{
    public class InitalizationStepEventArgs : EventArgs
    {
        public InitalizationStepEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}