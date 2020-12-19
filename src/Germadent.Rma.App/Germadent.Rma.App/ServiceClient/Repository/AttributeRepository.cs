using Germadent.Rma.Model;

namespace Germadent.Rma.App.ServiceClient.Repository
{
    public interface IAttributeRepository : IRepository<AttributeDto> { }

    public class AttributeRepository : Repository<AttributeDto>, IAttributeRepository
    {
        private readonly IRmaServiceClient _rmaServiceClient;

        public AttributeRepository(IRmaServiceClient rmaServiceClient)
        {
            _rmaServiceClient = rmaServiceClient;
        }

        protected override AttributeDto[] GetItems()
        {
            var attributes = _rmaServiceClient.GetAttributes();
            return attributes;
        }
    }
}