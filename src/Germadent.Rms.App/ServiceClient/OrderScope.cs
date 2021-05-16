using System;
using Germadent.Model;

namespace Germadent.Rms.App.ServiceClient
{
    public class OrderScope : IDisposable
    {
        private readonly IRmsServiceClient _rmsServiceClient;

        public OrderScope(IRmsServiceClient rmsServiceClient, OrderDto order)
        {
            _rmsServiceClient = rmsServiceClient;
            Order = order;
        }

        public OrderDto Order { get; }

        public void Dispose()
        {
            _rmsServiceClient.UnLockOrder(Order.WorkOrderId);
        }
    }
}