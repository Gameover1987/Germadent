using System;
using NLog;

namespace Germadent.Common.Logging
{
    public interface ILogger
    {
        void Info(string message);

        void Error(Exception exception);

        void Fatal(Exception exception);
    }

    public class Logger : ILogger
    {
        private readonly NLog.Logger _logger = LogManager.GetCurrentClassLogger();


        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Error(Exception exception)
        {
            _logger.Error(exception);
        }

        public void Fatal(Exception exception)
        {
            _logger.Fatal(exception);
        }
    }
}