using System;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.Model.Pricing;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockProductRepository : IProductRepository
    {
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<EventArgs> Changed;

        public ProductDto[] Items { get{return new DesignMockRmaServiceClient().GetProducts();} }
    }
}