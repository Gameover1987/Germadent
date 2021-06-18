using System.Linq;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Notifications;
using Germadent.Client.Common.ServiceClient.Repository;
using Germadent.Model;

namespace Germadent.Rma.App.ServiceClient.Repository
{
    public class CustomerRepository : Repository<CustomerDto>, ICustomerRepository
    {
        private readonly IRmaServiceClient _rmaServiceClient;
        private readonly ISignalRClient _signalRClient;

        public CustomerRepository(IRmaServiceClient rmaServiceClient, ISignalRClient signalRClient)
        {
            _rmaServiceClient = rmaServiceClient;
            _signalRClient = signalRClient;
            _signalRClient.CustomerRepositoryChanged += SignalRClientOnCustomerRepositoryChanged;
        }

        private void SignalRClientOnCustomerRepositoryChanged(object sender, RepositoryChangedEventArgs<CustomerDto> e)
        {
            OnRepositoryChanged(this, e);
        }

        protected override CustomerDto[] GetItems()
        {
            return _rmaServiceClient.GetCustomers("").ToArray();
        }
    }
}