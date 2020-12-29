using System.Linq;
using System.Threading;
using Germadent.Common;
using Germadent.Common.Web;
using Germadent.Rma.App.Configuration;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;
using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;
using RestSharp;

namespace Germadent.Rma.App.ServiceClient
{
    public class RmaServiceClient : ServiceClientBase, IRmaServiceClient
    {
        private readonly IConfiguration _configuration;
        private readonly ISignalRClient _signalRClient;

        public RmaServiceClient(IConfiguration configuration, ISignalRClient signalRClient)
        {
            _configuration = configuration;
            _signalRClient = signalRClient;
        }

        public void Authorize(string login, string password)
        {
            var info = ExecuteHttpGet<AuthorizationInfoDto>(
                _configuration.DataServiceUrl + string.Format("/api/auth/authorize/{0}/{1}", login, password));

            AuthorizationInfo = info;
            AuthenticationToken = info.Token;

            if (AuthorizationInfo.IsLocked)
                throw new UserMessageException("Учетная запись заблокирована.");

            if (AuthorizationInfo.Rights.Count(x => x.RightName == RmaUserRights.RunApplication) == 0)
                throw new UserMessageException("Отсутствует право на запуск приложения");

            _signalRClient.Initialize();
        }
        public AuthorizationInfoDto AuthorizationInfo { get; protected set; }

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

            return addedOrder;
        }

        public OrderDto UpdateOrder(OrderDto order)
        {
            var updatedOrder = ExecuteHttpPost<OrderDto>(_configuration.DataServiceUrl + "/api/Rma/orders/update", order);          

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
            return addedCustomer;
        }

        public CustomerDto UpdateCustomer(CustomerDto customerDto)
        {
            var updatedCustomer = ExecuteHttpPost<CustomerDto>(_configuration.DataServiceUrl + "/api/Rma/customers/update", customerDto);
            return updatedCustomer;
        }

        public DeleteResult DeleteCustomer(int customerId)
        {
            return ExecuteHttpDelete<DeleteResult>(_configuration.DataServiceUrl + $"/api/Rma/Customers/{customerId}");
        }

        public ResponsiblePersonDto[] GetResponsiblePersons()
        {
            return ExecuteHttpGet<ResponsiblePersonDto[]>(_configuration.DataServiceUrl + "/api/Rma/responsiblePersons");
        }

        public ResponsiblePersonDto AddResponsiblePerson(ResponsiblePersonDto responsiblePersonDto)
        {
            var addedResponsiblePerson = ExecuteHttpPost<ResponsiblePersonDto>(_configuration.DataServiceUrl + "/api/Rma/responsiblePersons/add", responsiblePersonDto);
            return addedResponsiblePerson;
        }

        public ResponsiblePersonDto UpdateResponsiblePerson(ResponsiblePersonDto responsiblePersonDto)
        {
            var updatedResponsiblePerson = ExecuteHttpPost<ResponsiblePersonDto>(_configuration.DataServiceUrl + "/api/Rma/responsiblePersons/update", responsiblePersonDto);
            return updatedResponsiblePerson;
        }

        public DeleteResult DeleteResponsiblePerson(int responsiblePersonId)
        {
            return ExecuteHttpDelete<DeleteResult>(_configuration.DataServiceUrl + $"/api/Rma/responsiblePersons/{responsiblePersonId}");
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
            Thread.Sleep(3000);
            var addedPriceGroup = ExecuteHttpPost<PriceGroupDto>(_configuration.DataServiceUrl + "/api/Rma/Pricing/AddPriceGroup", priceGroupDto);
            return addedPriceGroup;
        }

        public PriceGroupDto UpdatePriceGroup(PriceGroupDto priceGroupDto)
        {
           Thread.Sleep(3000);
            var updatedPriceGroup = ExecuteHttpPost<PriceGroupDto>(_configuration.DataServiceUrl + "/api/Rma/Pricing/UpdatePriceGroup", priceGroupDto);
            return updatedPriceGroup;
        }

        public DeleteResult DeletePriceGroup(int priceGroupId)
        {
           Thread.Sleep(3000);
            return ExecuteHttpDelete<DeleteResult>(_configuration.DataServiceUrl + $"/api/Rma/Pricing/DeletePriceGroup/" + priceGroupId);
        }

        public PricePositionDto[] GetPricePositions(BranchType branchType)
        {
            return ExecuteHttpGet<PricePositionDto[]>(_configuration.DataServiceUrl + $"/api/Rma/Pricing/PricePositions/" + (int)branchType);
        }

        public PricePositionDto AddPricePosition(PricePositionDto pricePositionDto)
        {
            Thread.Sleep(3000);
            return ExecuteHttpPost<PricePositionDto>(_configuration.DataServiceUrl + $"/api/Rma/Pricing/AddPricePosition", pricePositionDto);
        }

        public PricePositionDto UpdatePricePosition(PricePositionDto pricePositionDto)
        {
            Thread.Sleep(3000);
            return ExecuteHttpPost<PricePositionDto>(_configuration.DataServiceUrl + $"/api/Rma/Pricing/UpdatePricePosition", pricePositionDto);
        }

        public DeleteResult DeletePricePosition(int pricePositionId)
        {
            Thread.Sleep(3000);
            return ExecuteHttpDelete<DeleteResult>(_configuration.DataServiceUrl + $"/api/Rma/Pricing/DeletePricePosition/" + pricePositionId);
        }

        public ProductDto[] GetProducts()
        {
            return ExecuteHttpGet<ProductDto[]>(_configuration.DataServiceUrl + $"/api/Rma/Pricing/GetProducts");
        }

        public AttributeDto[] GetAttributes()
        {
            return ExecuteHttpGet<AttributeDto[]>(_configuration.DataServiceUrl + $"/api/Rma/Attributes/GetAttributesAndValues");
        }

        protected override void HandleError(IRestResponse response)
        {
            throw new ServerSideException(response);
        }
    }
}