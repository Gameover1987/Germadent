namespace Germadent.Rma.Model.Pricing
{
    public class ProductDto
    {
        public int ProductId { get; set; }

        public int PricePositionId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public override string ToString()
        {
            return string.Format("Id={0}, Name={1}, Price={2}", ProductId, Name, Price);
        }
    }
}
