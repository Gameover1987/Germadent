using System;
using System.Linq;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Notifications;
using Germadent.Model;
using Germadent.Model.Pricing;

namespace Germadent.Rma.App.ServiceClient.Repository
{
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
           OnRepositoryChanged(this, e);
        }

        protected override PriceGroupDto[] GetItems()
        {
            var priceGroupForMc = _rmaServiceClient.GetPriceGroups(BranchType.MillingCenter).ToArray();
            var priceGroupForLab = _rmaServiceClient.GetPriceGroups(BranchType.Laboratory).ToArray();

            return priceGroupForMc.Concat(priceGroupForLab).ToArray();
        }
    }
}