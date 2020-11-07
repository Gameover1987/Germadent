namespace Germadent.Rma.Model.Pricing
{
    public class PriceGroupDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public BranchType BranchType { get; set; }

        public PricePositionDto[] Positions { get; set; }

        public override string ToString()
        {
            return string.Format("Id={0}, Name={1}", Id, Name);
        }
    }

    public class PriceGroupDeleteResult
    {
        public int PriceGroupId { get; set; }

        public int Count { get; set; }
    }
}