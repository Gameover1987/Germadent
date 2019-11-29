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

        public Order[] GetOrders(OrdersFilter ordersFilter = null)
        {
            if (ordersFilter == null)
                ordersFilter = new OrdersFilter();

            var content = new StringContent(ordersFilter.SerializeToJson(), Encoding.UTF8, "application/json");

            var apiUrl = _configuration.DataServiceUrl + "/api/Orders/getOrdersByFilter";
            using (var response = _client.PostAsync(apiUrl, content).Result)
            {
                var orders = response.Content.ReadAsStringAsync().Result.DeserializeFromJson<Order[]>();
                return orders;
            }
        }

        public Order GetOrderDetails(int orderId)
        {
            throw new NotImplementedException();
        }

        public Material[] GetMaterials()
        {
            return new Material[0];
        }

        public void AddOrder(Order order)
        {
            var content = new StringContent(order.SerializeToJson(), Encoding.UTF8, "application/json");

            var apiUrl = _configuration.DataServiceUrl + "/api/Orders/laboratoryOrder";
            using (var response = _client.PostAsync(apiUrl, content).Result)
            {
                var result = response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}