using System.Linq;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Notifications;
using Germadent.Client.Common.ServiceClient.Repository;
using Germadent.Model.Pricing;

namespace Germadent.Rma.App.ServiceClient.Repository
{
    public class ProductRepository : Repository<ProductDto>, IProductRepository
    {
        private readonly IRmaServiceClient _rmaServiceClient;
        private readonly ISignalRClient _signalRClient;

        public ProductRepository(IRmaServiceClient rmaServiceClient, ISignalRClient signalRClient)
        {
            _rmaServiceClient = rmaServiceClient;
            _signalRClient = signalRClient;
            _signalRClient.ProductRepositoryChanged += SignalRClientOnProductRepositoryChanged;
        }

        private void SignalRClientOnProductRepositoryChanged(object sender, RepositoryChangedEventArgs<ProductDto> e)
        {
            OnRepositoryChanged(this, e);
        }

        protected override ProductDto[] GetItems()
        {
            var products = _rmaServiceClient.GetProducts().ToArray();

            return products;
        }
    }
}