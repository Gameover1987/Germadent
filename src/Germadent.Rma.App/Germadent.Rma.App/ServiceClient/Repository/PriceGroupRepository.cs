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
        private readonly ISignalRClient _signalRClient;

        public PriceGroupRepository(IRmaServiceClient rmaServiceClient, ISignalRClient signalRClient)
        {
            _rmaServiceClient = rmaServiceClient;
            _signalRClient = signalRClient;
            _signalRClient.PriceGroupRepositoryChanged += SignalRClientOnPriceGroupRepositoryChanged;
        }

        private void SignalRClientOnPriceGroupRepositoryChanged(object sender, RepositoryChangedEventArgs<PriceGroupDto> e)
        {
           ReLoad();
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
        private readonly ISignalRClient _signalRClient;

        public PricePositionRepository(IRmaServiceClient rmaServiceClient, ISignalRClient signalRClient)
        {
            _rmaServiceClient = rmaServiceClient;
            _signalRClient = signalRClient;
            _signalRClient.PricePositionRepositoryChanged += SignalRClientOnPricePositionRepositoryChanged;
            
        }

        private void SignalRClientOnPricePositionRepositoryChanged(object? sender, RepositoryChangedEventArgs<PricePositionDto> e)
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
}