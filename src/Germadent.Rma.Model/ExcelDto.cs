using System;

namespace Germadent.Rma.Model
{
    public class ReportListDto
    {
        public DateTime Created { get; set; }
        public string DocNumber { get; set; }
        public string Customer { get; set; }
        public string EquipmentSubstring { get; set; }
        public string Patient { get; set; }
        public string ProstheticSubstring { get; set; }
        public string MaterialsStr { get; set; }
        public string ColorAndFeatures { get; set; }
        public string CarcassColor { get; set; }
        public string ImplantSystem { get; set; }
        public int Quantity { get; set; }
        public string ProstheticArticul { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TotalPriceCashless { get; set; }

    }
}
