using System;

namespace Germadent.Model
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

        public string CreatorFullName { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime StatusChanged { get; set; }

        public UserDto LockedBy { get; set; }

        public DateTime? LockDate { get; set; }

        public string Modeller { get; set; }

        public string Technician { get; set; }

        public string Operator { get; set; }
    }
}
