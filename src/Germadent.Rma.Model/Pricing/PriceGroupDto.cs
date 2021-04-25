namespace Germadent.Rma.Model.Pricing
{
    public class PriceGroupDto : IIdentityDto
    {
        public int PriceGroupId { get; set; }

        public string Name { get; set; }

        public BranchType BranchType { get; set; }

        public override string ToString()
        {
            return string.Format("PriceGroupId={0}, Name={1}", PriceGroupId, Name);
        }

        public int GetIdentificator()
        {
            return PriceGroupId;
        }
    }

    public class DeleteResult : IIdentityDto
    {
        public int Id { get; set; }

        public int Count { get; set; }

        public string Error { get; set; }

        public int GetIdentificator()
        {
            return Id;
        }
    }
}