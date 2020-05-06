using System.Linq;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ServiceClient.Repository
{
    public interface ICustomerRepository : IRepository<CustomerDto>
    {

    }

    public class CustomerRepository : Repository<CustomerDto>, ICustomerRepository
    {
        private readonly IRmaServiceClient _rmaOperations;

        public CustomerRepository(IRmaServiceClient rmaOperations)
        {
            //TODO Nekrasov:нул
            _rmaOperations = rmaOperations;
            _rmaOperations.CustomerRepositoryChanged += RmaOperationsOnCustomerRepositoryChanged;
        }

        private void RmaOperationsOnCustomerRepositoryChanged(object sender, CustomerRepositoryChangedEventArgs e)
        {
            //TODO Nekrasov:ReLoad?)))
            ReLoad();
        }

        protected override CustomerDto[] GetItems()
        {
            return _rmaOperations.GetCustomers("").ToArray();
        }
    }
}