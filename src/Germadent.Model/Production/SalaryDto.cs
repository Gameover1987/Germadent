using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.Model.Production
{
    public class SalaryDto
    {
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string DocNumber { get; set; }
        public string CustomerName { get; set; }
        public string PatientFullName { get; set; }
        public string ProductName { get; set; }
        public string TechnologyOperationUserCode { get; set; }
        public string TechnologyOperationName { get; set; }
        public decimal Rate { get; set; }
        public int Count { get; set; }
        public float UrgencyRatio { get; set; }
        public decimal OperationCost { get; set; }
        public DateTime WorkStarted { get; set; }
        public DateTime? WorkCompleted { get; set; }
    }
}
