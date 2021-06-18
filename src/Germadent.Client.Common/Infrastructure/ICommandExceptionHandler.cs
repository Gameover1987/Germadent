using System;

namespace Germadent.Client.Common.Infrastructure
{
    public interface ICommandExceptionHandler
    {
        void HandleCommandException(Exception exception);
    }
}