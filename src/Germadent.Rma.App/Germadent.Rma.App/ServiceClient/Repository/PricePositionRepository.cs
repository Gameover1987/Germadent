using System.Linq;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;

namespace Germadent.Rma.App.ServiceClient.Repository
{
    public interface IPricePositionRepository : IRepository<PricePositionDto>
    {

    }

    public class PricePositionRepository : Repository<PricePositionDto>, IPricePositionRepository
    {
        private readonly IRmaServiceClient _rmaServiceClient;
        private readonly ISignalRClient _signalRClient;

        public PricePositionRepository(IRmaServiceClient rmaServiceClient, ISignalRClient signalRClient)
        {
            _rmaServiceClient = rmaServiceClient;
            _signalRClient = signalRClient;
            _signalRClient.PricePositionRepositoryChanged += SignalRClientOnPricePositionRepositoryChanged;
            
        }

        private void SignalRClientOnPricePositionRepositoryChanged(object sender, RepositoryChangedEventArgs<PricePositionDto> e)
        {
            ReLoad();
        }

        protected override PricePositionDto[] GetItems()
        {
            var pricePositionsForMc = _rmaServiceClient.GetPricePositions(BranchType.MillingCenter).ToArray();
            var pricePositionsForLab = _rmaServiceClient.GetPricePositions(BranchType.Laboratory).ToArray();

            return pricePositionsForMc.Concat(pricePositionsForLab).ToArray();
        }
    }

    public interface IProductRepository : IRepository<ProductDto>
    {

    }

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

        private void SignalRClientOnProductRepositoryChanged(object? sender, RepositoryChangedEventArgs<ProductDto> e)
        {
            ReLoad();
        }

        protected override ProductDto[] GetItems()
        {
            var products = _rmaServiceClient.GetProducts().ToArray();

            return products;
        }
    }
}