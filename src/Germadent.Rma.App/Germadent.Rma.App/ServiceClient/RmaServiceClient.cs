using System;
using System.Linq;
using Germadent.Common;
using Germadent.Common.FileSystem;
using Germadent.Common.Web;
using Germadent.Rma.App.Configuration;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;
using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;
using Microsoft.AspNetCore.SignalR.Client;
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

        public async void Authorize(string login, string password)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl(new Uri(_configuration.DataServiceUrl + "/chathub"))
                .WithAutomaticReconnect()
                .Build();

            await connection.StartAsync();
            connection.On<string, string>("Send", Handler);

            var info = ExecuteHttpGet<AuthorizationInfoDto>(
                _configuration.DataServiceUrl + string.Format("/api/userManagement/authorization/authorize/{0}/{1}", login, password));

            AuthorizationInfo = info;

            if (AuthorizationInfo.IsLocked)
                throw new UserMessageException("Учетная запись заблокирована.");

            if (AuthorizationInfo.Rights.Count(x => x.RightName == RmaUserRights.RunApplication) == 0)
                throw new UserMessageException("Отсутствует право на запуск приложения");
        }

        private void Handler(string arg1, string arg2)
        {

        }

        public AuthorizationInfoDto AuthorizationInfo { get; private set; }

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
            var addedOrder = ExecuteHttpPost<OrderDto>(_configuration.DataServiceUrl + "/api/Rma/orders/add", order);
            if (order.DataFileName != null)
            {
                var api = string.Format("{0}/api/Rma/orders/fileUpload/{1}/{2}", _configuration.DataServiceUrl, addedOrder.WorkOrderId, _fileManager.GetShortFileName(order.DataFileName));
                ExecuteFileUpload(api, order.DataFileName);
            }

            return addedOrder;
        }

        public OrderDto UpdateOrder(OrderDto order)
        {
            var updatedOrder = ExecuteHttpPost<OrderDto>(_configuration.DataServiceUrl + "/api/Rma/orders/update", order);
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

        public CustomerDto AddCustomer(CustomerDto сustomerDto)
        {
            var addedCustomer = ExecuteHttpPost<CustomerDto>(_configuration.DataServiceUrl + "/api/Rma/customers/add", сustomerDto);
            CustomerRepositoryChanged?.Invoke(this, new CustomerRepositoryChangedEventArgs(new[] { addedCustomer }, null));
            return addedCustomer;
        }

        public CustomerDto UpdateCustomer(CustomerDto customerDto)
        {
            var updatedCustomer = ExecuteHttpPost<CustomerDto>(_configuration.DataServiceUrl + "/api/Rma/customers/update", customerDto);
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
            var addedResponsiblePerson = ExecuteHttpPost<ResponsiblePersonDto>(_configuration.DataServiceUrl + "/api/Rma/responsiblePersons/add", responsiblePersonDto);
            ResponsiblePersonRepositoryChanged?.Invoke(this, new ResponsiblePersonRepositoryChangedEventArgs(new[] { addedResponsiblePerson }, null));
            return addedResponsiblePerson;
        }

        public ResponsiblePersonDto UpdateResponsiblePerson(ResponsiblePersonDto responsiblePersonDto)
        {
            var updatedResponsiblePerson = ExecuteHttpPost<ResponsiblePersonDto>(_configuration.DataServiceUrl + "/api/Rma/responsiblePersons/update", responsiblePersonDto);
            ResponsiblePersonRepositoryChanged?.Invoke(this, new ResponsiblePersonRepositoryChangedEventArgs(new[] { responsiblePersonDto }, null));
            return updatedResponsiblePerson;
        }

        public ResponsiblePersonDeleteResult DeleteResponsiblePerson(int responsiblePersonId)
        {
            return ExecuteHttpDelete<ResponsiblePersonDeleteResult>(_configuration.DataServiceUrl + $"/api/Rma/responsiblePersons/{responsiblePersonId}");
        }

        public DictionaryItemDto[] GetDictionary(DictionaryType dictionaryType)
        {
            return ExecuteHttpGet<DictionaryItemDto[]>(_configuration.DataServiceUrl + $"/api/Rma/Dictionaries/{dictionaryType}");
        }

        public PriceGroupDto[] GetPriceGroups(BranchType branchType)
        {
            return ExecuteHttpGet<PriceGroupDto[]>(_configuration.DataServiceUrl + $"/api/Rma/Pricing/PriceGroups/" + (int)branchType);
        }

        public PriceGroupDto AddPriceGroup(PriceGroupDto priceGroupDto)
        {
            var addedPriceGroup = ExecuteHttpPost<PriceGroupDto>(_configuration.DataServiceUrl + "/api/Rma/Pricing/AddPriceGroup", priceGroupDto);
            return addedPriceGroup;
        }

        public PriceGroupDto UpdatePriceGroup(PriceGroupDto priceGroupDto)
        {
            var updatedPriceGroup = ExecuteHttpPost<PriceGroupDto>(_configuration.DataServiceUrl + "/api/Rma/Pricing/UpdatePriceGroup", priceGroupDto);
            return updatedPriceGroup;
        }

        public DeleteResult DeletePriceGroup(int priceGroupId)
        {
            return ExecuteHttpDelete<DeleteResult>(_configuration.DataServiceUrl + $"/api/Rma/Pricing/DeletePriceGroup/" + priceGroupId);
        }

        public PricePositionDto[] GetPricePositions(BranchType branchType)
        {
            return ExecuteHttpGet<PricePositionDto[]>(_configuration.DataServiceUrl + $"/api/Rma/Pricing/PricePositions/" + (int)branchType);
        }

        public event EventHandler<CustomerRepositoryChangedEventArgs> CustomerRepositoryChanged;

        public event EventHandler<ResponsiblePersonRepositoryChangedEventArgs> ResponsiblePersonRepositoryChanged;

        public PricePositionDto AddPricePosition(PricePositionDto pricePositionDto)
        {
            return ExecuteHttpPost<PricePositionDto>(_configuration.DataServiceUrl + $"/api/Rma/Pricing/AddPricePosition", pricePositionDto);
        }

        public PricePositionDto UpdatePricePosition(PricePositionDto pricePositionDto)
        {
            return ExecuteHttpPost<PricePositionDto>(_configuration.DataServiceUrl + $"/api/Rma/Pricing/UpdatePricePosition", pricePositionDto);
        }

        public DeleteResult DeletePricePosition(int pricePositionId)
        {
            return ExecuteHttpDelete<DeleteResult>(_configuration.DataServiceUrl + $"/api/Rma/Pricing/DeletePricePosition/" + pricePositionId);
        }

        protected override void HandleError(IRestResponse response)
        {
            throw new ServerSideException(response);
        }
    }
}