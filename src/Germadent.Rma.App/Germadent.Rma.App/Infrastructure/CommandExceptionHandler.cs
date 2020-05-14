using System;
using System.Collections.Generic;
using System.Text;

namespace Germadent.Rma.App.Infrastructure
{
    public interface ICommandExceptionHandler
    {
        void HandleCommandException(Exception exception);
    }

    public class CommandExceptionHandler : ICommandExceptionHandler
    {
        public void HandleCommandException(Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
