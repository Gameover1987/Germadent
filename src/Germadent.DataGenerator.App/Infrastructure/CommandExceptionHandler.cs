using System;
using Germadent.UI.Infrastructure;

namespace Germadent.DataGenerator.App.Infrastructure
{
    public interface ICommandExceptionHandler
    {
        void HandleCommandException(Exception exception);
    }

    public class CommandExceptionHandler : ICommandExceptionHandler
    {
        private readonly IShowDialogAgent _dialogAgent;

        public CommandExceptionHandler(IShowDialogAgent dialogAgent)
        {
            _dialogAgent = dialogAgent;
        }

        public void HandleCommandException(Exception exception)
        {
            _dialogAgent.ShowErrorMessageDialog(exception.Message, exception.StackTrace);
        }
    }
}