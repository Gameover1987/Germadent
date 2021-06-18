using System;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Repository;
using Germadent.Model.Pricing;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;

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