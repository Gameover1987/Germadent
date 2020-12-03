using System.Collections.Generic;
using Germadent.Rma.Model.Pricing;

namespace Germadent.Rma.Model
{
    public class ToothDto
    {
        public int WorkOrderId { get; set; }

        public int ToothNumber { get; set; }

        public int ConditionId { get; set; }

        public string ConditionName { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int? MaterialId { get; set; }

        public string MaterialName { get; set; }

        public int Price { get; set; }

        public bool HasBridge { get; set; }

        public ProductDto[] Products { get; set; }

        public override string ToString()
        {
            return string.Format(
                "WorkOrderId={0}, TootoNumber={1}, MaterialId={2}, MaterialName={3}, ProductId={4}, ProductName={5}, Price{6}, HasBridge={7}",
                WorkOrderId, ToothNumber, MaterialId, MaterialName, ProductId, ProductName, Price, HasBridge);
        }
    }
}
