namespace Germadent.Rma.Model.Pricing
{
    public class ProductDto
    {
        public int PricePositionId { get; set; }

        public int? MaterialId { get; set; }

        public int ProstheticTypeId { get; set; }

        public string ProstheticTypeName { get; set; }
    }
}