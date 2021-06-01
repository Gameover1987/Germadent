using System;
using System.Linq;
using System.Threading;
using Germadent.Client.Common.Configuration;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Notifications;
using Germadent.Model;
using Germadent.Model.Pricing;
using Germadent.Model.Production;
using Germadent.Model.Rights;

namespace Germadent.Rma.App.ServiceClient
{
    public class RmaServiceClient : BaseClientOperationsServiceClient, IRmaServiceClient
    {
        public RmaServiceClient(IClientConfiguration configuration, ISignalRClient signalRClient) 
            : base(configuration, signalRClient)
        {
        }

        protected override bool CheckRunApplicationRight()
        {
            return AuthorizationInfo.Rights.Any(x => x.RightName == RmaUserRights.RunApplication);
        }

        public OrderDto AddOrder(OrderDto order)
        {
            order.CreatorId = AuthorizationInfo.UserId;
            
            var addedOrder = ExecuteHttpPost<OrderDto>(Configuration.DataServiceUrl + "/api/Rma/Orders/add", order);

            return addedOrder;
        }

        public OrderDto UpdateOrder(OrderDto order)
        {
            var updatedOrder = ExecuteHttpPost<OrderDto>(Configuration.DataServiceUrl + "/api/Rma/orders/update", order);

            return updatedOrder;
        }

        public OrderDto CloseOrder(int id)
        {
            return ExecuteHttpDelete<OrderDto>(Configuration.DataServiceUrl + $"/api/Rma/orders/close/{id}");
        }

        public ReportListDto[] GetWorkReport(int id)
        {
            return ExecuteHttpGet<ReportListDto[]>(Configuration.DataServiceUrl + $"/api/Rma/reports/{id}");
        }

        public CustomerDto[] GetCustomers(string mask)
        {
            return ExecuteHttpGet<CustomerDto[]>(Configuration.DataServiceUrl + $"/api/Rma/Customers?mask={mask}");
        }

        public CustomerDto AddCustomer(CustomerDto сustomerDto)
        {
            var addedCustomer = ExecuteHttpPost<CustomerDto>(Configuration.DataServiceUrl + "/api/Rma/customers/add", сustomerDto);
            return addedCustomer;
        }

        public CustomerDto UpdateCustomer(CustomerDto customerDto)
        {
            var updatedCustomer = ExecuteHttpPost<CustomerDto>(Configuration.DataServiceUrl + "/api/Rma/customers/update", customerDto);
            return updatedCustomer;
        }

        public DeleteResult DeleteCustomer(int customerId)
        {
            return ExecuteHttpDelete<DeleteResult>(Configuration.DataServiceUrl + $"/api/Rma/Customers/{customerId}");
        }

        public ResponsiblePersonDto[] GetResponsiblePersons()
        {
            return ExecuteHttpGet<ResponsiblePersonDto[]>(Configuration.DataServiceUrl + "/api/Rma/responsiblePersons");
        }

        public ResponsiblePersonDto AddResponsiblePerson(ResponsiblePersonDto responsiblePersonDto)
        {
            var addedResponsiblePerson = ExecuteHttpPost<ResponsiblePersonDto>(Configuration.DataServiceUrl + "/api/Rma/responsiblePersons/add", responsiblePersonDto);
            return addedResponsiblePerson;
        }

        public ResponsiblePersonDto UpdateResponsiblePerson(ResponsiblePersonDto responsiblePersonDto)
        {
            var updatedResponsiblePerson = ExecuteHttpPost<ResponsiblePersonDto>(Configuration.DataServiceUrl + "/api/Rma/responsiblePersons/update", responsiblePersonDto);
            return updatedResponsiblePerson;
        }

        public DeleteResult DeleteResponsiblePerson(int responsiblePersonId)
        {
            return ExecuteHttpDelete<DeleteResult>(Configuration.DataServiceUrl + $"/api/Rma/responsiblePersons/{responsiblePersonId}");
        }

        public PriceGroupDto[] GetPriceGroups(BranchType branchType)
        {
            return ExecuteHttpGet<PriceGroupDto[]>(Configuration.DataServiceUrl + $"/api/Rma/Pricing/PriceGroups/" + (int)branchType);
        }

        public PriceGroupDto AddPriceGroup(PriceGroupDto priceGroupDto)
        {
            var addedPriceGroup = ExecuteHttpPost<PriceGroupDto>(Configuration.DataServiceUrl + "/api/Rma/Pricing/AddPriceGroup", priceGroupDto);
            return addedPriceGroup;
        }

        public PriceGroupDto UpdatePriceGroup(PriceGroupDto priceGroupDto)
        {
            var updatedPriceGroup = ExecuteHttpPost<PriceGroupDto>(Configuration.DataServiceUrl + "/api/Rma/Pricing/UpdatePriceGroup", priceGroupDto);
            return updatedPriceGroup;
        }

        public DeleteResult DeletePriceGroup(int priceGroupId)
        {
            return ExecuteHttpDelete<DeleteResult>(Configuration.DataServiceUrl + $"/api/Rma/Pricing/DeletePriceGroup/" + priceGroupId);
        }

        public PricePositionDto[] GetPricePositions(BranchType branchType)
        {
            return ExecuteHttpGet<PricePositionDto[]>(Configuration.DataServiceUrl + $"/api/Rma/Pricing/PricePositions/" + (int)branchType);
        }

        public PricePositionDto AddPricePosition(PricePositionDto pricePositionDto)
        {
            return ExecuteHttpPost<PricePositionDto>(Configuration.DataServiceUrl + $"/api/Rma/Pricing/AddPricePosition", pricePositionDto);
        }

        public PricePositionDto UpdatePricePosition(PricePositionDto pricePositionDto)
        {
            return ExecuteHttpPost<PricePositionDto>(Configuration.DataServiceUrl + $"/api/Rma/Pricing/UpdatePricePosition", pricePositionDto);
        }

        public DeleteResult DeletePricePosition(int pricePositionId)
        {
            return ExecuteHttpDelete<DeleteResult>(Configuration.DataServiceUrl + $"/api/Rma/Pricing/DeletePricePosition/" + pricePositionId);
        }

        public ProductDto[] GetProducts()
        {
            return ExecuteHttpGet<ProductDto[]>(Configuration.DataServiceUrl + $"/api/Rma/Pricing/GetProducts");
        }

        public AttributeDto[] GetAttributes()
        {
            return ExecuteHttpGet<AttributeDto[]>(Configuration.DataServiceUrl + $"/api/Rma/Attributes/GetAttributesAndValues");
        }

        public EmployeePositionDto[] GetEmployeePositions()
        {
            return ExecuteHttpGet<EmployeePositionDto[]>(Configuration.DataServiceUrl + $"/api/Rma/Technology/EmployeePositions");
        }

        public TechnologyOperationDto[] GetTechnologyOperations()
        {
            return ExecuteHttpGet<TechnologyOperationDto[]>(Configuration.DataServiceUrl + $"/api/Rma/Technology/Operations");
        }

        public TechnologyOperationDto AddTechnologyOperation(TechnologyOperationDto technologyOperationDto)
        {
            return ExecuteHttpPost<TechnologyOperationDto>(Configuration.DataServiceUrl + $"/api/Rma/Technology/AddOperation", technologyOperationDto);
        }

        public TechnologyOperationDto UpdateTechnologyOperation(TechnologyOperationDto technologyOperationDto)
        {
            return ExecuteHttpPost<TechnologyOperationDto>(Configuration.DataServiceUrl + $"/api/Rma/Technology/UpdateOperation", technologyOperationDto);
        }

        public DeleteResult DeleteTechnologyOperation(int technologyOperationId)
        {
            return ExecuteHttpDelete<DeleteResult>(Configuration.DataServiceUrl + $"/api/Rma/Technology/DeleteOperation/" + technologyOperationId);
        }

        public UserDto[] GetAllUsers()
        {
            return ExecuteHttpGet<UserDto[]>(Configuration.DataServiceUrl + "/api/userManagement/users/GetUsers");
        }

        public WorkDto[] GetSalaryReport(int? userId, DateTime dateFrom, DateTime dateTo)
        {
            var api = Configuration.DataServiceUrl + "/api/Rma/Reports/GetSalaryReport";
            return ExecuteHttpPost<WorkDto[]>(api, new SalaryFilter{ UserId = userId, DateFrom = dateFrom, DateTo = dateTo} );
        }
    }
}