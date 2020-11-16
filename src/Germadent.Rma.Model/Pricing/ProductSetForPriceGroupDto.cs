namespace Germadent.Rma.Model.Pricing
{
    public class ProductSetForPriceGroupDto
    {
        public int PriceGroupId { get; set; }

        public int PricePositionId { get; set; }

        public string PricePositionCode { get; set; }

        public int MaterialId { get; set; }

        public string MaterialName { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal PriceSTL { get; set; }

        public decimal PriceModel { get; set; }
    }
}
