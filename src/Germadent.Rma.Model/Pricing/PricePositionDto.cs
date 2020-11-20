namespace Germadent.Rma.Model.Pricing
{
    public class PricePositionDto : IIdentityDto
    {
        public int PricePositionId { get; set; }

        public int PriceGroupId { get; set; }

        public string UserCode { get; set; }

        public string Name { get; set; }

        public int? MaterialId { get; set; }

        public int ProstheticTypeId { get; set; }

        public BranchType BranchType { get; set; }

        public PriceDto[] Prices { get; set; }

        public override string ToString()
        {
            return string.Format("Id={0}, UserCode={1}, Name={2}", PricePositionId, UserCode, Name);
        }

        public int GetIdentificator()
        {
            return PricePositionId;
        }
    }
}