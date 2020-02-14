using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Germadent.Common.Extensions;
using Germadent.Rma.App.Configuration;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ServiceClient
{
    public class RmaOperations : IRmaOperations
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        public RmaOperations(IConfiguration configuration)
        {
            _configuration = configuration;

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
            var content = new StringContent(order.SerializeToJson(), Encoding.UTF8, "application/json");

            var apiUrl = _configuration.DataServiceUrl + "/api/Rma/addOrder";
            using (var response = _client.PostAsync(apiUrl, content).Result)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                return result.DeserializeFromJson<OrderDto>();
            }
        }

        public OrderDto UpdateOrder(OrderDto order)
        {
            var content = new StringContent(order.SerializeToJson(), Encoding.UTF8, "application/json");

            var apiUrl = _configuration.DataServiceUrl + "/api/Rma/updateOrder";
            using (var response = _client.PutAsync(apiUrl, content).Result)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                return result.DeserializeFromJson<OrderDto>();
            }
        }

        public void UploadStlFile(int id, byte[] stlFile)
        {
            throw new NotImplementedException();
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
            return new EquipmentDto[0];
        }

        public EquipmentDto[] GetEquipment()
        {
            var apiUrl = _configuration.DataServiceUrl + "/api/Rma/equipment";
            using (var response = _client.GetAsync(apiUrl).Result)
            {
                var equipment = response.Content.ReadAsStringAsync().Result.DeserializeFromJson<EquipmentDto[]>();
                return equipment;
            }
        }
    }
}