using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.Rma.Model.Production
{
    public class TechnologyOperationDto
    {
        public int TechnologyOperationId { get; set; }

        public int EmployeePositionId { get; set; }

        public string UserCode { get; set; }

        public string Name { get; set; }

        public decimal Rate { get; set; }
    }

    public class EmployeePositionDto
    {
        public int EmployeePositionId { get; set; }

        public string Name { get; set; }
    }
}
