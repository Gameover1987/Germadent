using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Client.Common.Reporting;
using Germadent.Model;

namespace Germadent.Client.Common.DesignMock
{
    public class DesignMockPrintModule : IPrintModule
    {
        public void Print(OrderDto order)
        {
            throw new System.NotImplementedException();
        }
    }
}
