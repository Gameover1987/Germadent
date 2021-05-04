using System;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockPriceGroupRepository : IPriceGroupRepository
    {
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        

        public PriceGroupDto[] Items
        {
            get { return new DesignMockRmaServiceClient().GetPriceGroups(BranchType.Laboratory); }
        }

        public event EventHandler<RepositoryChangedEventArgs<PriceGroupDto>> Changed;
    }
}