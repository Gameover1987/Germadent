using System;

namespace Germadent.Rma.Model
{
    public class Order
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public BranchType BranchType { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Closed { get; set; }

        public string Customer { get; set; }

        public string Patient { get; set; }

        public string Employee { get; set; }

        public string Material { get; set; }
        public string ResponsiblePerson { get; set; }
        public string RespPersPhone { get; set; }
        public string AdditionalInfo { get; set; }
        public string CarcassColor { get; set; }
        public string ImplantSystem { get; set; }
        public string IndividualAbutmentProcessing { get; set; }
        public string WorkDescription { get; set; }
    }
}
