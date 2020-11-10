namespace Germadent.Rma.Model.Pricing
{
    public class PriceGroupDto
    {
        public int PriceGroupId { get; set; }

        public string Name { get; set; }

        public BranchType BranchType { get; set; }

        public PricePositionDto[] Positions { get; set; }

        public override string ToString()
        {
            return string.Format("PriceGroupId={0}, Name={1}", PriceGroupId, Name);
        }
    }

    public class DeleteResult
    {
        public int Id { get; set; }

        public int Count { get; set; }
    }
}