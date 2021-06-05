using System;
using Germadent.Client.Common.Configuration;
using Germadent.Client.Common.ServiceClient.Notifications;
using Germadent.Model;

namespace Germadent.Client.Common.ServiceClient
{
    public abstract class BaseClientOperationsServiceClient : AuthSupportableServiceClient, IBaseClientOperationsServiceClient
    {
        private Guid _guid = Guid.NewGuid();

        protected BaseClientOperationsServiceClient(IClientConfiguration clientConfiguration, ISignalRClient signalRClient)
            : base(clientConfiguration, signalRClient)
        {
        }

        public OrderLiteDto[] GetOrders(OrdersFilter filter)
        {
            var api = Configuration.DataServiceUrl + "/api/Rma/Orders/getByFilter";
            return ExecuteHttpPost<OrderLiteDto[]>(api, filter);
        }

        public OrderScope GetOrderById(int workOrderId)
        {
            var order = ExecuteHttpGet<OrderDto>(Configuration.DataServiceUrl + $"/api/Rma/orders/{workOrderId}/{AuthorizationInfo.UserId}");
            return new OrderScope(this, order);
        }

        public DictionaryItemDto[] GetDictionary(DictionaryType dictionaryType)
        {
            return ExecuteHttpGet<DictionaryItemDto[]>(Configuration.DataServiceUrl + $"/api/Rma/Dictionaries/{dictionaryType}");
        }

        public byte[] GetTemplate(DocumentTemplateType documentTemplateType)
        {
            return ExecuteHttpGet<byte[]>(Configuration.DataServiceUrl + $"/api/Common/DocumentTemplate/GetDocumentTemplate/{documentTemplateType}");
        }

        public void UnLockOrder(int workOrderId)
        {
            ExecuteHttpGet<OrderDto>(Configuration.DataServiceUrl + $"/api/Rma/orders/UnlockWorkOrder/{workOrderId}");
        }
    }
}