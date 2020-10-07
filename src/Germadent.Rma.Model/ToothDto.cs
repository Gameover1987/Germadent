namespace Germadent.Rma.Model
{
    public class ToothDto
    {
        public int WorkOrderId { get; set; }

        public int ToothNumber { get; set; }

        public int ConditionId { get; set; }

        public string ConditionName { get; set; }

        public int ProstheticsId { get; set; }

        public string ProstheticsName { get; set; }

        public int MaterialId { get; set; }

        public string MaterialName { get; set; }

        public int PricePositionId { get; set; }

        public string PricePositionCode { get; set; }

        public string PricePositionName { get; set; }

        public int Price { get; set; }

        public bool HasBridge { get; set; }

        public override string ToString()
        {
            return string.Format(
                "WorkOrderId={0}, TootoNumber={1}, MaterialId={2}, MaterialName={3}, ProstheticsId={4}, ProstheticsName={5}, PricePositionId{6}, PricePositionCode{7}, PricePositionName{8}, Price{9}, HasBridge={10}",
                WorkOrderId, ToothNumber, MaterialId, MaterialName, ProstheticsId, ProstheticsName, PricePositionId, PricePositionCode, PricePositionName, Price, HasBridge);
        }
    }
}
