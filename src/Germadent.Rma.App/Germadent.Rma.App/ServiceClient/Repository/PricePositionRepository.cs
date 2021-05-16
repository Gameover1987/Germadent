using System.Linq;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Notifications;
using Germadent.Model;
using Germadent.Model.Pricing;

namespace Germadent.Rma.App.ServiceClient.Repository
{
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
            OnRepositoryChanged(this, e);
        }

        protected override PricePositionDto[] GetItems()
        {
            var pricePositionsForMc = _rmaServiceClient.GetPricePositions(BranchType.MillingCenter).ToArray();
            var pricePositionsForLab = _rmaServiceClient.GetPricePositions(BranchType.Laboratory).ToArray();

            return pricePositionsForMc.Concat(pricePositionsForLab).ToArray();
        }
    }
}