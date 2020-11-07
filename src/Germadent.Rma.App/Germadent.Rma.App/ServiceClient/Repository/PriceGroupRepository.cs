using System.Linq;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;

namespace Germadent.Rma.App.ServiceClient.Repository
{
    public interface IPriceGroupRepository : IRepository<PriceGroupDto>
    {

    }

    public class PriceGroupRepository : Repository<PriceGroupDto>, IPriceGroupRepository
    {
        private readonly IRmaServiceClient _rmaServiceClient;

        public PriceGroupRepository(IRmaServiceClient rmaServiceClient)
        {
            _rmaServiceClient = rmaServiceClient;
        }

        protected override PriceGroupDto[] GetItems()
        {
            var priceGroupForMc = _rmaServiceClient.GetPriceGroups(BranchType.MillingCenter).ToArray();
            var priceGroupForLab = _rmaServiceClient.GetPriceGroups(BranchType.Laboratory).ToArray();

            return priceGroupForMc.Concat(priceGroupForLab).ToArray();
        }
    }

    public interface IPricePositionRepository : IRepository<PricePositionDto>
    {

    }

    public class PricePositionRepository : Repository<PricePositionDto>, IPricePositionRepository
    {
        private readonly IRmaServiceClient _rmaServiceClient;

        public PricePositionRepository(IRmaServiceClient rmaServiceClient)
        {
            _rmaServiceClient = rmaServiceClient;
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

        public ProductRepository(IRmaServiceClient rmaServiceClient)
        {
            _rmaServiceClient = rmaServiceClient;
        }
        protected override ProductDto[] GetItems()
        {
            return _rmaServiceClient.GetProducts();
        }
    }
}
