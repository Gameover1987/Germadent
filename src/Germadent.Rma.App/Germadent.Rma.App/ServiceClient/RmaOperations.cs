using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Germadent.Common.Extensions;
using Germadent.Common.FileSystem;
using Germadent.Rma.App.Configuration;
using Germadent.Rma.Model;
using RestSharp;

namespace Germadent.Rma.App.ServiceClient
{
    public class RmaOperations : IRmaOperations
    {
        private readonly IConfiguration _configuration;
        private readonly IFileManager _fileManager;
        private readonly HttpClient _client;

        public RmaOperations(IConfiguration configuration, IFileManager fileManager)
        {
            _configuration = configuration;
            _fileManager = fileManager;

            _client = new HttpClient();
            _client.BaseAddress = new Uri(_configuration.DataServiceUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public OrderLiteDto[] GetOrders(OrdersFilter ordersFilter = null)
        {
            if (ordersFilter == null)
                ordersFilter = new OrdersFilter();

            var content = new StringContent(ordersFilter.SerializeToJson(), Encoding.UTF8, "application/json");

            var apiUrl = _configuration.DataServiceUrl + "/api/Rma/getOrdersByFilter";
            using (var response = _client.PostAsync(apiUrl, content).Result)
            {
                var orders = response.Content.ReadAsStringAsync().Result.DeserializeFromJson<OrderLiteDto[]>();
                return orders;
            }
        }

        public OrderDto GetOrderDetails(int id)
        {
            var apiUrl = _configuration.DataServiceUrl + string.Format("/api/Rma/orders/{0}", id);
            using (var response = _client.GetAsync(apiUrl).Result)
            {
                var order = response.Content.ReadAsStringAsync().Result.DeserializeFromJson<OrderDto>();
                return order;
            }
        }

        public IFileResponse GetDataFileByWorkOrderId(int id)
        {
            var apiUrl = _configuration.DataServiceUrl + string.Format("/api/Rma/files/{0}", id);
            var response = _client.GetAsync(apiUrl).Result;

            return new FileResponse(response);
        }

        public ProstheticConditionDto[] GetProstheticConditions()
        {
            var apiUrl = _configuration.DataServiceUrl + "/api/Rma/prostheticConditions";
            using (var response = _client.GetAsync(apiUrl).Result)
            {
                var prostheticConditions = response.Content.ReadAsStringAsync().Result.DeserializeFromJson<ProstheticConditionDto[]>();
                return prostheticConditions;
            }
        }

        public MaterialDto[] GetMaterials()
        {
            var apiUrl = _configuration.DataServiceUrl + "/api/Rma/materials";
            using (var response = _client.GetAsync(apiUrl).Result)
            {
                var materials = response.Content.ReadAsStringAsync().Result.DeserializeFromJson<MaterialDto[]>();
                return materials;
            }
        }

        public ProstheticsTypeDto[] GetProstheticTypes()
        {
            var apiUrl = _configuration.DataServiceUrl + "/api/Rma/prostheticTypes";
            using (var response = _client.GetAsync(apiUrl).Result)
            {
                var prostheticTypes = response.Content.ReadAsStringAsync().Result.DeserializeFromJson<ProstheticsTypeDto[]>();
                return prostheticTypes;
            }
        }

        public OrderDto AddOrder(OrderDto order)
        {
            var client = new RestClient();
            var restRequest = new RestRequest(_configuration.DataServiceUrl + "/api/Rma/addOrder", Method.POST);

            restRequest.RequestFormat = DataFormat.Json;

            restRequest.AddBody(order.SerializeToJson());

            if (order.DataFileName != null)
                restRequest.AddFile("OrderData", _fileManager.ReadAllBytes(order.DataFileName), "OrderData");

            var response = client.Execute(restRequest, Method.POST);
            return response.Content.DeserializeFromJson<OrderDto>();
        }

        public OrderDto UpdateOrder(OrderDto order)
        {
            var client = new RestClient();
            IRestRequest restRequest = new RestRequest(_configuration.DataServiceUrl + "/api/Rma/updateOrder", Method.POST);

            restRequest.RequestFormat = DataFormat.Json;

            restRequest.AddBody(order.SerializeToJson());

            if (order.DataFileName != null)
                restRequest.AddFile("OrderData", _fileManager.ReadAllBytes(order.DataFileName), "OrderData");

            var response = client.Execute(restRequest, Method.POST);
            return response.Content.DeserializeFromJson<OrderDto>();
        }

        public OrderDto CloseOrder(int id)
        {
            var apiUrl = _configuration.DataServiceUrl + string.Format("/api/Rma/closeOrder/{0}", id);
            using (var response = _client.GetAsync(apiUrl).Result)
            {
                var order = response.Content.ReadAsStringAsync().Result.DeserializeFromJson<OrderDto>();
                return order;
            }
        }

        public TransparencesDto[] GetTransparences()
        {
            var apiUrl = _configuration.DataServiceUrl + "/api/Rma/transparences";
            using (var response = _client.GetAsync(apiUrl).Result)
            {
                var transparences = response.Content.ReadAsStringAsync().Result.DeserializeFromJson<TransparencesDto[]>();
                return transparences;
            }
        }

        public EquipmentDto[] GetEquipments()
        {
            var apiUrl = _configuration.DataServiceUrl + "/api/Rma/equipments";
            using (var response = _client.GetAsync(apiUrl).Result)
            {
                var equipments = response.Content.ReadAsStringAsync().Result.DeserializeFromJson<EquipmentDto[]>();
                return equipments;
            }
        }

        public ReportListDto[] GetWorkReport(int id)
        {
            var apiUrl = _configuration.DataServiceUrl + string.Format("/api/Rma/excel/{0}", id);

            using (var response = _client.GetAsync(apiUrl).Result)
            {
                var excelDto = response.Content.ReadAsStringAsync().Result.DeserializeFromJson<ReportListDto[]>();
                return excelDto;
            }
        }

        public CustomerDto[] GetCustomers(string mask)
        {
            var apiUrl = _configuration.DataServiceUrl + string.Format("/api/Rma/customers/{0}", mask);
            using (var response = _client.GetAsync(apiUrl).Result)
            {
                var customers = response.Content.ReadAsStringAsync().Result.DeserializeFromJson<CustomerDto[]>();
                return customers;
            }
        }

        public ResponsiblePersonDto[] GetResponsiblePersons(int customerId)
        {
            var apiUrl = _configuration.DataServiceUrl + string.Format("/api/Rma/responsiblePersons/{0}", customerId);
            using (var response = _client.GetAsync(apiUrl).Result)
            {
                var rpDto = response.Content.ReadAsStringAsync().Result.DeserializeFromJson<ResponsiblePersonDto[]>();
                return rpDto;
            }
        }

        public CustomerDto AddCustomer(CustomerDto сustomerDto)
        {
            var apiUrl = _configuration.DataServiceUrl + string.Format("/api/Rma/addCustomer");
            var content = new StringContent(сustomerDto.SerializeToJson(), Encoding.UTF8, "application/json");
            using (var response = _client.PostAsync(apiUrl, content).Result)
            {
                var newCustomerDto = response.Content.ReadAsStringAsync().Result.DeserializeFromJson<CustomerDto>();
                return newCustomerDto;
            }
        }
    }
}