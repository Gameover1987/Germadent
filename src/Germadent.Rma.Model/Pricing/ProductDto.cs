namespace Germadent.Rma.Model.Pricing
{
    public class ProductDto
    {
        public int PriceGroupId { get; set; }

        public int PricePositionId { get; set; }

        public string UserCode { get; set; }

        public int? MaterialId { get; set; }

        public int ProstheticTypeId { get; set; }

        public decimal PriceStl { get; set; }

        public decimal PriceModel { get; set; }
    }
}