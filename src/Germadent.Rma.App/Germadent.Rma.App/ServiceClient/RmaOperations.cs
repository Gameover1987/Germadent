using System;
using System.Net.Http;
using System.Net.Http.Headers;
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
            var getOrdersUrl = _configuration.DataServiceUrl + "/api/Orders";
            if (ordersFilter != null)
            {
                getOrdersUrl = string.Format("{0}/api/Orders/{1}",_configuration.DataServiceUrl, ordersFilter.SerializeToJson());
            }

            var responseTask = _client.GetAsync(getOrdersUrl);
            responseTask.Wait();

            var response = responseTask.Result;

            var readDataTask = response.Content.ReadAsStringAsync();
            readDataTask.Wait();

            var orders = readDataTask.Result.DeserializeFromJson<Order[]>();
            return orders;
        }

        public Material[] GetMaterials()
        {
            return new Material[0];
        }

        public void AddOrder(Order order)
        {
            throw new System.NotImplementedException();
        }
    }
}