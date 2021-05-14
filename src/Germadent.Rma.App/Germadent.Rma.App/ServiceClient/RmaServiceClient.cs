using System.Linq;
using Germadent.Common;
using Germadent.Common.Web;
using Germadent.Model;
using Germadent.Model.Pricing;
using Germadent.Model.Production;
using Germadent.Model.Rights;
using Germadent.Rma.App.Infrastructure.Configuration;
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

            _signalRClient.Initialize(info);
        }
        public AuthorizationInfoDto AuthorizationInfo { get; protected set; }

        public OrderLiteDto[] GetOrders(OrdersFilter ordersFilter)
        {
            var api = _configuration.DataServiceUrl + "/api/Rma/Orders/getByFilter";
            return ExecuteHttpPost<OrderLiteDto[]>(api, ordersFilter);
        }

        public OrderDto GetOrderById(int workOrderId)
        {
            return ExecuteHttpGet<OrderDto>(_configuration.DataServiceUrl + $"/api/Rma/orders/{workOrderId}/{AuthorizationInfo.UserId}");
        }

        public byte[] GetDataFileByWorkOrderId(int id)
        {
            var apiUrl = _configuration.DataServiceUrl + string.Format("/api/Rma/orders/fileDownload/{0}", id);
            return ExecuteFileDownload(apiUrl);
        }

        public OrderDto AddOrder(OrderDto order)
        {
            order.CreatorId = AuthorizationInfo.UserId;
            
            var addedOrder = ExecuteHttpPost<OrderDto>(_configuration.DataServiceUrl + "/api/Rma/Orders/add", order);

            return addedOrder;
        }

        public OrderDto UpdateOrder(OrderDto order)
        {
            var updatedOrder = ExecuteHttpPost<OrderDto>(_configuration.DataServiceUrl + "/api/Rma/orders/update", order);

            return updatedOrder;
        }

        public OrderDto CloseOrder(int id)
        {
            return ExecuteHttpDelete<OrderDto>(_configuration.DataServiceUrl + $"/api/Rma/orders/close/{id}");
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

        public ProductDto[] GetProducts()
        {
            return ExecuteHttpGet<ProductDto[]>(_configuration.DataServiceUrl + $"/api/Rma/Pricing/GetProducts");
        }

        public AttributeDto[] GetAttributes()
        {
            return ExecuteHttpGet<AttributeDto[]>(_configuration.DataServiceUrl + $"/api/Rma/Attributes/GetAttributesAndValues");
        }

        public EmployeePositionDto[] GetEmployeePositions()
        {
            return ExecuteHttpGet<EmployeePositionDto[]>(_configuration.DataServiceUrl + $"/api/Rma/Technology/EmployeePositions");
        }

        public TechnologyOperationDto[] GetTechnologyOperations()
        {
            return ExecuteHttpGet<TechnologyOperationDto[]>(_configuration.DataServiceUrl + $"/api/Rma/Technology/Operations");
        }

        public TechnologyOperationDto AddTechnologyOperation(TechnologyOperationDto technologyOperationDto)
        {
            return ExecuteHttpPost<TechnologyOperationDto>(_configuration.DataServiceUrl + $"/api/Rma/Technology/AddOperation", technologyOperationDto);
        }

        public TechnologyOperationDto UpdateTechnologyOperation(TechnologyOperationDto technologyOperationDto)
        {
            return ExecuteHttpPost<TechnologyOperationDto>(_configuration.DataServiceUrl + $"/api/Rma/Technology/UpdateOperation", technologyOperationDto);
        }

        public DeleteResult DeleteTechnologyOperation(int technologyOperationId)
        {
            return ExecuteHttpDelete<DeleteResult>(_configuration.DataServiceUrl + $"/api/Rma/Technology/DeleteOperation/" + technologyOperationId);
        }

        public void UnLockOrder(int workOrderId)
        {
            ExecuteHttpGet<OrderDto>(_configuration.DataServiceUrl + $"/api/Rma/orders/UnlockWorkOrder/{workOrderId}");
        }


        protected override void HandleError(IRestResponse response)
        {
            throw new ServerSideException(response);
        }
    }
}