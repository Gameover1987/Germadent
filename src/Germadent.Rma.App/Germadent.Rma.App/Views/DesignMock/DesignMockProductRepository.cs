using System;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.Model.Pricing;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockProductRepository : IProductRepository
    {
        public void Initialize()
        {
            
        }

        public ProductDto[] Items { get{return new ProductDto[0];} }

        public event EventHandler<RepositoryChangedEventArgs<ProductDto>> Changed;
    }
}