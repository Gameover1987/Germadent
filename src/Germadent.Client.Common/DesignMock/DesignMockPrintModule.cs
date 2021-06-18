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
        public void PrintOrder(OrderDto order)
        {
            throw new System.NotImplementedException();
        }

        public void PrintSalaryReport(SalaryReport salaryReport)
        {
            throw new NotImplementedException();
        }
    }
}
