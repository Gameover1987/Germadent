using Germadent.Rma.Model.Production;

namespace Germadent.Rma.App.ServiceClient.Repository
{
    public interface ITechnologyOperationRepository : IRepository<TechnologyOperationDto>
    {

    }

    public class TechnologyOperationRepository : Repository<TechnologyOperationDto>, ITechnologyOperationRepository
    {
        private readonly IRmaServiceClient _rmaServiceClient;

        public TechnologyOperationRepository(IRmaServiceClient rmaServiceClient)
        {
            _rmaServiceClient = rmaServiceClient;
        }

        protected override TechnologyOperationDto[] GetItems()
        {
            return _rmaServiceClient.GetTechnologyOperations();
        }
    }
}