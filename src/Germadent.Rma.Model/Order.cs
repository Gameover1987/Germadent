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
    }
}
