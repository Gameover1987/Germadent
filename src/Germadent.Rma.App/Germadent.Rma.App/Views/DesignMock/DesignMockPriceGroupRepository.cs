using System;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Repository;
using Germadent.Model;
using Germadent.Model.Pricing;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;

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