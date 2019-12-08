using System;

namespace Germadent.Rma.Model
{
    public class OrderLite
    {
        public int BranchTypeId { get; set; }

        public BranchType BranchType { get; set; }

        public string DocNumber { get; set; }

        public string CustomerName { get; set; }

        public string ResponsiblePerson { get; set; }

        public string PatientFnp { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Closed { get; set; }

        public int Status { get; set; }
    }
}
