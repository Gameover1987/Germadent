using System;
using DocumentFormat.OpenXml.Drawing;
using Germadent.Common.FileSystem;
using Germadent.Common.Web;
using Germadent.Rma.App.Configuration;
using Germadent.Rma.Model;
using RestSharp;

namespace Germadent.Rma.App.ServiceClient
{
    public class RmaServiceClient : ServiceClientBase, IRmaServiceClient
    {
        private readonly IConfiguration _configuration;
        private readonly IFileManager _fileManager;

        public RmaServiceClient(IConfiguration configuration, IFileManager fileManager)
        {
            _configuration = configuration;
            _fileManager = fileManager;
        }

        public void Authorize(string user, string password)
        {

        }

        public OrderLiteDto[] GetOrders(OrdersFilter ordersFilter)
        {
            return ExecuteHttpPost<OrderLiteDto[]>(_configuration.DataServiceUrl + "/api/Rma/Orders/getByFilter", ordersFilter);
        }

        public OrderDto GetOrderById(int id)
        {
            return ExecuteHttpGet<OrderDto>(_configuration.DataServiceUrl + $"/api/Rma/orders/{id}");
        }

        public byte[] GetDataFileByWorkOrderId(int id)
        {
            var apiUrl = _configuration.DataServiceUrl + string.Format("/api/Rma/orders/fileDownload/{0}", id);
            return ExecuteFileDownload(apiUrl);
        }

        public OrderDto AddOrder(OrderDto order)
        {
            var addedOrder = ExecuteHttpPost<OrderDto>(_configuration.DataServiceUrl + "/api/Rma/orders", order);
            if (order.DataFileName != null)
            {
                var api = string.Format("{0}/api/Rma/orders/fileUpload/{1}/{2}", _configuration.DataServiceUrl, addedOrder.WorkOrderId, _fileManager.GetShortFileName(order.DataFileName));
                ExecuteFileUpload(api, order.DataFileName);
            }

            return addedOrder;
        }

        public OrderDto UpdateOrder(OrderDto order)
        {
            var updatedOrder =  ExecuteHttpPut<OrderDto>(_configuration.DataServiceUrl + "/api/Rma/orders", order);
            if (order.DataFileName != null)
            {
                var api = string.Format("{0}/api/Rma/orders/fileUpload/{1}/{2}", _configuration.DataServiceUrl, order.WorkOrderId, _fileManager.GetShortFileName(order.DataFileName));
                ExecuteFileUpload(api, order.DataFileName);
            }

            return updatedOrder;
        }

        public OrderDto CloseOrder(int id)
        {
            return ExecuteHttpDelete<OrderDto>(_configuration.DataServiceUrl + $"/api/Rma/orders/{id}");
        }

        public ReportListDto[] GetWorkReport(int id)
        {
            return ExecuteHttpGet<ReportListDto[]>(_configuration.DataServiceUrl + $"/api/Rma/reports/{id}");
        }

        public CustomerDto[] GetCustomers(string mask)
        {
            return ExecuteHttpGet<CustomerDto[]>(_configuration.DataServiceUrl + $"/api/Rma/Customers?mask={mask}");
        }

        public CustomerDto UpdateCustomer(CustomerDto customerDto)
        {
            var updatedCustomer = ExecuteHttpPut<CustomerDto>(_configuration.DataServiceUrl + "/api/Rma/customers", customerDto);
            CustomerRepositoryChanged?.Invoke(this, new CustomerRepositoryChangedEventArgs(new[] { updatedCustomer }, null));
            return updatedCustomer;
        }

        public CustomerDeleteResult DeleteCustomer(int customerId)
        {
            return ExecuteHttpDelete<CustomerDeleteResult>(_configuration.DataServiceUrl + $"/api/Rma/Customers/{customerId}");
        }

        public ResponsiblePersonDto[] GetResponsiblePersons()
        {
            return ExecuteHttpGet<ResponsiblePersonDto[]>(_configuration.DataServiceUrl + "/api/Rma/responsiblePersons");
        }

        public ResponsiblePersonDto AddResponsiblePerson(ResponsiblePersonDto responsiblePersonDto)
        {
            var addedResponsiblePerson = ExecuteHttpPost<ResponsiblePersonDto>(_configuration.DataServiceUrl + "/api/Rma/responsiblePersons", responsiblePersonDto);
            ResponsiblePersonRepositoryChanged?.Invoke(this, new ResponsiblePersonRepositoryChangedEventArgs(new[] { addedResponsiblePerson }, null));
            return addedResponsiblePerson;
        }

        public ResponsiblePersonDto UpdateResponsiblePerson(ResponsiblePersonDto responsiblePersonDto)
        {
            var updatedResponsiblePerson = ExecuteHttpPut<ResponsiblePersonDto>(_configuration.DataServiceUrl + "/api/Rma/responsiblePersons", responsiblePersonDto);
            ResponsiblePersonRepositoryChanged?.Invoke(this, new ResponsiblePersonRepositoryChangedEventArgs(new[] { responsiblePersonDto }, null));
            return updatedResponsiblePerson;
        }

        public ResponsiblePersonDeleteResult DeleteResponsiblePerson(int responsiblePersonId)
        {
            return ExecuteHttpDelete<ResponsiblePersonDeleteResult>(_configuration.DataServiceUrl + $"/api/Rma/responsiblePersons/{responsiblePersonId}");
        }

        public CustomerDto AddCustomer(CustomerDto сustomerDto)
        {
            var addedCustomer = ExecuteHttpPost<CustomerDto>(_configuration.DataServiceUrl + "/api/Rma/customers", сustomerDto);
            CustomerRepositoryChanged?.Invoke(this, new CustomerRepositoryChangedEventArgs(new[] { addedCustomer }, null));
            return addedCustomer;
        }

        public DictionaryItemDto[] GetDictionary(DictionaryType dictionaryType)
        {
            return ExecuteHttpGet<DictionaryItemDto[]>(_configuration.DataServiceUrl + $"/api/Rma/Dictionaries/{dictionaryType}");
        }

        public event EventHandler<CustomerRepositoryChangedEventArgs> CustomerRepositoryChanged;
        
        public event EventHandler<ResponsiblePersonRepositoryChangedEventArgs> ResponsiblePersonRepositoryChanged;

        protected override void HandleError(IRestResponse response)
        {
            throw new ServerSideException(response);
        }
    }
}