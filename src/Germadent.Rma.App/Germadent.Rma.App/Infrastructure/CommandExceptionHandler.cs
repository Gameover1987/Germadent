﻿using System;
using Germadent.Common.Logging;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.Infrastructure
{
    public interface ICommandExceptionHandler
    {
        void HandleCommandException(Exception exception);
    }

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
