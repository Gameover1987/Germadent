namespace Germadent.Rma.Model.Pricing
{
    public class PriceGroupForToothCardDto
    {
        public int PriceGroupId { get; set; }

        public string Name { get; set; }

        public BranchType BranchType { get; set; }

        public ProductSetForPriceGroupDto[] ProductSetForPriceGroup { get; set; }
    }
}
