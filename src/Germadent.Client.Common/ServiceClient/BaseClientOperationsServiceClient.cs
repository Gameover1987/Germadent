using System;
using Germadent.Client.Common.Configuration;
using Germadent.Client.Common.ServiceClient.Notifications;
using Germadent.Model;
using Germadent.Model.Production;

namespace Germadent.Client.Common.ServiceClient
{
    public abstract class BaseClientOperationsServiceClient : AuthSupportableServiceClient, IBaseClientOperationsServiceClient
    {
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

        public WorkDto[] GetWorksByWorkOrder(int workOrderId)
        {
            return ExecuteHttpGet<WorkDto[]>(Configuration.DataServiceUrl + $"/api/Rma/orders/GetWorksByWorkOrder/{workOrderId}");
        }

        public void UnLockOrder(int workOrderId)
        {
            ExecuteHttpGet<OrderDto>(Configuration.DataServiceUrl + $"/api/Rma/orders/UnlockWorkOrder/{workOrderId}");
        }

        public UserDto[] GetAllUsers()
        {
            return ExecuteHttpGet<UserDto[]>(Configuration.DataServiceUrl + "/api/userManagement/users/GetUsers");
        }

        public WorkDto[] GetSalaryReport(int? userId, DateTime dateFrom, DateTime dateTo)
        {
            var api = Configuration.DataServiceUrl + "/api/Rma/Reports/GetSalaryReport";
            return ExecuteHttpPost<WorkDto[]>(api, new SalaryFilter { UserId = userId, DateFrom = dateFrom, DateTo = dateTo });
        }
    }
}