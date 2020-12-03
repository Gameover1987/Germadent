namespace Germadent.WebApi.Entities
{
    public class ToothEntity
    {
        public int WorkOrderId { get; set; }

        public int ToothNumber { get; set; }

        public int PricePositionId { get; set; }

        public string PricePositionCode { get; set; }

        public string PricePositionName { get; set; }

        public int PriceGroupId { get; set; }

        public string ConditionName { get; set; }

        public int ConditionId { get; set; }

        public string MaterialName { get; set; }

        public int? MaterialId { get; set; }

        public string ProstheticsName { get; set; }

        public int ProductId { get; set; }

        public decimal Price { get; set; }

        public bool HasBridge { get; set; }
    }
}