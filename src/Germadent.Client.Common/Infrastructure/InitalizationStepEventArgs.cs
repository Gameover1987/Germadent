using System;

namespace Germadent.Client.Common.Infrastructure
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