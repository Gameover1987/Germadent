using System;
using System.Linq;
using Germadent.Model.Production;

namespace Germadent.Client.Common.Reporting
{
    public class SalaryReport
    {
        public string EmployeeFullName { get; set; }
        
        public DateTime DateFrom { get; set; }
        
        public DateTime DateTo { get; set; }
        
        public WorkDto[] Works { get; set; }

        public decimal Salary
        {
            get { return Works.Sum(x => x.OperationCost); }
        }
    }
}