using Germadent.Model.Pricing;

namespace Germadent.Model
{
    public class ToothDto
    {
        public int WorkOrderId { get; set; }

        public int ToothNumber { get; set; }

        public int ConditionId { get; set; }

        public string ConditionName { get; set; }

        public bool HasBridge { get; set; }

        public ProductDto[] Products { get; set; }

        public override string ToString()
        {
            return string.Format("WorkOrderId={0}, ToothNumber={1}, HasBridge={2}", WorkOrderId, ToothNumber, HasBridge);
        }
    }
}
