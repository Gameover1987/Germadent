using System;
using Germadent.Client.Common.Infrastructure;
using Germadent.Common.Logging;
using Germadent.UI.Infrastructure;

namespace Germadent.Rms.App.Infrastructure
{
    public class CommandExceptionHandler : ICommandExceptionHandler
    {
        private readonly IShowDialogAgent _dialogAgent;
        private readonly ILogger _logger;

        public CommandExceptionHandler(IShowDialogAgent dialogAgent, ILogger logger)
        {
            _dialogAgent = dialogAgent;
            _logger = logger;
        }

        public void HandleCommandException(Exception exception)
        {
            _logger.Error(exception);
            _dialogAgent.ShowErrorMessageDialog(exception.Message, exception.StackTrace);
        }
    }
}
