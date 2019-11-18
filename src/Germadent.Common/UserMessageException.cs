using System;

namespace Germadent.Common
{
    /// <summary>
    /// Обычное информационное пользовательское исключение
    /// </summary>
    public class UserMessageException : ApplicationException
    {
        public UserMessageException(string message)
            : base(message)
        {
        }

        public UserMessageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}