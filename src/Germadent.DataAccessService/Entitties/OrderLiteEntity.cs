using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.DataAccessService.Entitties
{
    /// <summary>
    /// Класс для элемента списка, содержащий минимум инфы о заказ наряде
    /// </summary>
    public class OrderLiteEntity
    {  
        public int BranchTypeId { get; set; }

        public string  BranchType { get; set; }

        public string DocNumber { get; set; }

        public string CustomerName { get; set; }

        public string ResponsiblePerson { get; set; }

        public string PatientFnp { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Closed { get; set; }

        public int Status { get; set; }
    }
}
