using System;
using DocumentFormat.OpenXml.Drawing;
using Germadent.Common.FileSystem;
using Germadent.Common.Web;
using Germadent.Rma.App.Configuration;
using Germadent.Rma.Model;

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
            return ExecuteHttpPost<OrderLiteDto[]>(_configuration.DataServiceUrl + "/api/OrdersList", ordersFilter);
        }

        public OrderDto GetOrderById(int id)
        {
            return ExecuteHttpGet<OrderDto>(_configuration.DataServiceUrl + $"/api/orders/{id}");
        }

        public byte[] GetDataFileByWorkOrderId(int id)
        {
            var apiUrl = _configuration.DataServiceUrl + string.Format("/api/orders/fileDownload/{0}", id);
            return ExecuteFileDownload(apiUrl);
        }

        public OrderDto AddOrder(OrderDto order)
        {
            var addedOrder = ExecuteHttpPost<OrderDto>(_configuration.DataServiceUrl + "/api/orders", order);
            if (order.DataFileName != null)
            {
                var api = string.Format("{0}/api/orders/fileUpload/{1}/{2}", _configuration.DataServiceUrl, addedOrder.WorkOrderId, System.IO.Path.GetFileName(order.DataFileName));
                ExecuteFileUpload(api, order.DataFileName);
            }

            return addedOrder;
        }

        public OrderDto UpdateOrder(OrderDto order)
        {
            return ExecuteHttpPut<OrderDto>(_configuration.DataServiceUrl + "/api/orders", order);
        }

        public OrderDto CloseOrder(int id)
        {
            return ExecuteHttpDelete<OrderDto>(_configuration.DataServiceUrl + $"/api/orders/{id}");
        }

        public ReportListDto[] GetWorkReport(int id)
        {
            return ExecuteHttpGet<ReportListDto[]>(_configuration.DataServiceUrl + $"/api/rma/reports/{id}");
        }

        public CustomerDto[] GetCustomers(string mask)
        {
            return ExecuteHttpGet<CustomerDto[]>(_configuration.DataServiceUrl + $"/api/Customers?mask={mask}");
        }

        public CustomerDto UpdateCustomer(CustomerDto customerDto)
        {
            var updatedCustomer = ExecuteHttpPut<CustomerDto>(_configuration.DataServiceUrl + "/api/customers", customerDto);
            CustomerRepositoryChanged?.Invoke(this, new CustomerRepositoryChangedEventArgs(new[] { updatedCustomer }, null));
            return updatedCustomer;
        }

        public CustomerDeleteResult DeleteCustomer(int customerId)
        {
            return ExecuteHttpDelete<CustomerDeleteResult>(_configuration.DataServiceUrl + $"/api/Customers/{customerId}");
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
            var addedCustomer = ExecuteHttpPost<CustomerDto>(_configuration.DataServiceUrl + "/api/customers", сustomerDto);
            CustomerRepositoryChanged?.Invoke(this, new CustomerRepositoryChangedEventArgs(new[] { addedCustomer }, null));
            return addedCustomer;
        }

        public DictionaryItemDto[] GetDictionary(DictionaryType dictionaryType)
        {
            return ExecuteHttpGet<DictionaryItemDto[]>(_configuration.DataServiceUrl + $"/api/Dictionaries/{dictionaryType}");
        }

        public event EventHandler<CustomerRepositoryChangedEventArgs> CustomerRepositoryChanged;

        public event EventHandler<ResponsiblePersonRepositoryChangedEventArgs> ResponsiblePersonRepositoryChanged;
    }
}