using System;

namespace Germadent.Common.Logging
{
    public interface ILog
    {
        void Info(string format, params object[] objectArgs);
        void Debug(string format, params object[] objectArgs);
        void Error(string message, Exception exception = null);
        void Fatal(string message, Exception exception = null);

        void Test(string message, params object[] objectArgs);
    }
}