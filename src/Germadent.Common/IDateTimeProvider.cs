using System;
using System.Collections.Generic;
using System.Text;

namespace Germadent.Common
{
    public interface IDateTimeProvider
    {
        DateTime GetDateTime();
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetDateTime()
        {
            return DateTime.Now;
        }
    }
}
