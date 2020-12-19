﻿using System.Linq;
using Germadent.Rma.Model.Pricing;

namespace Germadent.Rma.App.ServiceClient.Repository
{
    public interface IProductRepository : IRepository<ProductDto>
    {

    }

    public class ProductRepository : Repository<ProductDto>, IProductRepository
    {
        private readonly IRmaServiceClient _rmaServiceClient;
        private readonly ISignalRClient _signalRClient;

        public ProductRepository(IRmaServiceClient rmaServiceClient, ISignalRClient signalRClient)
        {
            _rmaServiceClient = rmaServiceClient;
            _signalRClient = signalRClient;
            _signalRClient.ProductRepositoryChanged += SignalRClientOnProductRepositoryChanged;
        }

        private void SignalRClientOnProductRepositoryChanged(object? sender, RepositoryChangedEventArgs<ProductDto> e)
        {
            ReLoad();
        }

        protected override ProductDto[] GetItems()
        {
            var products = _rmaServiceClient.GetProducts().ToArray();

            return products;
        }
    }

    //public class AttributeRepository : 
}