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

        public OrderLite[] GetOrders(OrdersFilter ordersFilter = null)
        {
            if (ordersFilter == null)
                ordersFilter = new OrdersFilter();

            var content = new StringContent(ordersFilter.SerializeToJson(), Encoding.UTF8, "application/json");

            var apiUrl = _configuration.DataServiceUrl + "/api/Rma/getOrdersByFilter";
            using (var response = _client.PostAsync(apiUrl, content).Result)
            {
                var orders = response.Content.ReadAsStringAsync().Result.DeserializeFromJson<OrderLite[]>();
                return orders;
            }
        }

        public Order GetOrderDetails(int id)
        {
            var apiUrl = _configuration.DataServiceUrl + string.Format("/api/Rma/orders/{0}", id);
            using (var response = _client.GetAsync(apiUrl).Result)
            {
                var order = response.Content.ReadAsStringAsync().Result.DeserializeFromJson<Order>();
                return order;
            }
        }

        public Material[] GetMaterials()
        {
            var apiUrl = _configuration.DataServiceUrl + "/api/Rma/materials";
            using (var response = _client.GetAsync(apiUrl).Result)
            {
                var materials = response.Content.ReadAsStringAsync().Result.DeserializeFromJson<Material[]>();
                return materials;
            }
        }

        public Order AddOrder(Order order)
        {
            var content = new StringContent(order.SerializeToJson(), Encoding.UTF8, "application/json");

            var apiUrl = _configuration.DataServiceUrl + "/api/Rma/addOrder";
            using (var response = _client.PostAsync(apiUrl, content).Result)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                return result.DeserializeFromJson<Order>();
            }
        }

        public Order UpdateOrder(Order order)
        {
            var content = new StringContent(order.SerializeToJson(), Encoding.UTF8, "application/json");

            var apiUrl = _configuration.DataServiceUrl + "/api/Rma/updateOrder";
            using (var response = _client.PutAsync(apiUrl, content).Result)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                return result.DeserializeFromJson<Order>();
            }
        }
    }
}