using System;
using Germadent.Client.Common.ServiceClient;
using Germadent.Model;
using Germadent.Model.Pricing;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockPricePositionRepository : IPricePositionRepository
    {
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        

        public PricePositionDto[] Items
        {
            get { return new DesignMockRmaServiceClient().GetPricePositions(BranchType.Laboratory); }
        }

        public event EventHandler<RepositoryChangedEventArgs<PricePositionDto>> Changed;
    }
}