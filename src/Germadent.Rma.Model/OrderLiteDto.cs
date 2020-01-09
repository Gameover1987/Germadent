using System;

namespace Germadent.Rma.Model
{
    public class OrderLiteDto
    {
        public int WorkOrderId { get; set; }

        public BranchType BranchType { get; set; }

        public string DocNumber { get; set; }

        public string CustomerName { get; set; }

        public string DoctorFullName { get; set; }

        public string TechnicFullName { get; set; }

        public string PatientFnp { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Closed { get; set; }

        public int Status { get; set; }
    }
}
