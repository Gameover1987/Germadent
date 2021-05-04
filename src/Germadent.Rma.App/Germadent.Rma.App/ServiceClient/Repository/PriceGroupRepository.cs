using System;
using System.Linq;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;
using Germadent.Rma.Model.Production;

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