using System;
using Germadent.Model;

namespace Germadent.Client.Common.ServiceClient
{
    public class OrderScope : IDisposable
    {
        private readonly IBaseClientOperationsServiceClient _serviceClient;

        public OrderScope(IBaseClientOperationsServiceClient serviceClient, OrderDto order)
        {
            _serviceClient = serviceClient;
            Order = order;
        }

        public OrderDto Order { get; }

        public void Dispose()
        {
            _serviceClient.UnLockOrder(Order.WorkOrderId);
        }
    }
}