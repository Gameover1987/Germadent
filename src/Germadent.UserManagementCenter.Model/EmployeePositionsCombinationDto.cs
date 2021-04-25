using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.UserManagementCenter.Model
{
    public class EmployeePositionsCombinationDto
    {
        public int EmployeeId { get; set; }
        public int EmployeePositionId { get; set; }
        public string EmployeePositionName { get; set; }
        public int QualifyingRank { get; set; }
    }
}
