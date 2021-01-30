using System;
using System.Collections.Generic;
using System.Text;
using Germadent.Common;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockDateTimeProvider : IDateTimeProvider
    {
        private readonly DateTime _dateTime;

        public DesignMockDateTimeProvider(DateTime dateTime)
        {
            _dateTime = dateTime;
        }

        public DateTime GetDateTime()
        {
            return _dateTime;
        }
    }
}
