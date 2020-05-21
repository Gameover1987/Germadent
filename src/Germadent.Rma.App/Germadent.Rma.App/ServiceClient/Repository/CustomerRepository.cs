using System.Linq;
using System.Threading;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ServiceClient.Repository
{
    public interface ICustomerRepository : IRepository<CustomerDto>
    {

    }

    public class CustomerRepository : Repository<CustomerDto>, ICustomerRepository
    {
        private readonly IRmaServiceClient _rmaServiceClient;

        public CustomerRepository(IRmaServiceClient rmaServiceClient)
        {
            _rmaServiceClient = rmaServiceClient;
            _rmaServiceClient.CustomerRepositoryChanged += RmaServiceClientOnCustomerRepositoryChanged;
        }

        private void RmaServiceClientOnCustomerRepositoryChanged(object sender, CustomerRepositoryChangedEventArgs e)
        {
            ReLoad();
        }

        protected override CustomerDto[] GetItems()
        {
            Thread.Sleep(3000);
            return _rmaServiceClient.GetCustomers("").ToArray();
        }
    }
}