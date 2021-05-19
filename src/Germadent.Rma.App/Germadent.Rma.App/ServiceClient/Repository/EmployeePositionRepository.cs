using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Repository;
using Germadent.Model.Production;

namespace Germadent.Rma.App.ServiceClient.Repository
{
    public class EmployeePositionRepository : Repository<EmployeePositionDto>, IEmployeePositionRepository
    {
        private readonly IRmaServiceClient _rmaServiceClient;

        public EmployeePositionRepository(IRmaServiceClient rmaServiceClient)
        {
            _rmaServiceClient = rmaServiceClient;
        }

        protected override EmployeePositionDto[] GetItems()
        {
            return _rmaServiceClient.GetEmployeePositions();
        }
    }
}