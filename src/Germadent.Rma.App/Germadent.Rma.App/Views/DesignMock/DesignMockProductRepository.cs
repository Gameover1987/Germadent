using System;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.Model.Pricing;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockProductRepository : IProductRepository
    {
        public void Initialize()
        {
            
        }

        public event EventHandler<EventArgs> Changed;
        public ProductDto[] Items { get; }
    }
}