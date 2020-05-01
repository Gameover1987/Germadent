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
        private readonly RestClient _client;

        public RmaOperations(IConfiguration configuration, IFileManager fileManager)
        {
            _configuration = configuration;
            _fileManager = fileManager;

            _client = new RestClient();
        }

        public OrderLiteDto[] GetOrders(OrdersFilter ordersFilter)
        {
            var client = new RestClient();
            var restRequest = new RestRequest(_configuration.DataServiceUrl + "/api/OrdersList", Method.POST);

            restRequest.RequestFormat = DataFormat.Json;

            restRequest.AddJsonBody(ordersFilter);

            var response = client.Execute(restRequest, Method.POST);
            return response.Content.DeserializeFromJson<OrderLiteDto[]>();
        }

        public OrderDto GetOrderById(int id)
        {
            var api = string.Format("/api/orders/{0}", id);
            return ExecuteHttpGet<OrderDto>(api);
        }

        public IFileResponse GetDataFileByWorkOrderId(int id)
        {
            return null;
            //var apiUrl = _configuration.DataServiceUrl + string.Format("/api/Rma/files/{0}", id);
            //var response = _client.GetAsync(apiUrl).Result;

            //return new FileResponse(response);
        }

        public OrderDto AddOrder(OrderDto order)
        {
            return ExecuteHttpPost<OrderDto>("/api/orders", order);
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
            return null;
            //var apiUrl = _configuration.DataServiceUrl + string.Format("/api/Rma/closeOrder/{0}", id);
            //using (var response = _client.GetAsync(apiUrl).Result)
            //{
            //    var order = response.Content.ReadAsStringAsync().Result.DeserializeFromJson<OrderDto>();
            //    return order;
            //}
        }

        public ReportListDto[] GetWorkReport(int id)
        {
            return null;
            //var apiUrl = _configuration.DataServiceUrl + string.Format("/api/Rma/excel/{0}", id);

            //using (var response = _client.GetAsync(apiUrl).Result)
            //{
            //    var excelDto = response.Content.ReadAsStringAsync().Result.DeserializeFromJson<ReportListDto[]>();
            //    return excelDto;
            //}
        }

        public CustomerDto[] GetCustomers(string mask)
        {
            return ExecuteHttpGet<CustomerDto[]>(string.Format("/api/Customers?mask={0}", mask));
        }

        public ResponsiblePersonDto[] GetResponsiblePersons(int customerId)
        {
            return null;
            //var apiUrl = _configuration.DataServiceUrl + string.Format("/api/Rma/responsiblePersons/{0}", customerId);
            //using (var response = _client.GetAsync(apiUrl).Result)
            //{
            //    var rpDto = response.Content.ReadAsStringAsync().Result.DeserializeFromJson<ResponsiblePersonDto[]>();
            //    return rpDto;
            //}
        }

        public CustomerDto AddCustomer(CustomerDto сustomerDto)
        {
            return ExecuteHttpPost<CustomerDto>("/api/customers", сustomerDto);
        }

        public DictionaryItemDto[] GetDictionary(DictionaryType dictionaryType)
        {
            return ExecuteHttpGet<DictionaryItemDto[]>(string.Format("/api/Dictionaries/{0}", dictionaryType));
        }

        private T ExecuteHttpGet<T>(string api)
        {
            var restRequest = new RestRequest(_configuration.DataServiceUrl + api, Method.GET);
            restRequest.RequestFormat = DataFormat.Json;
            var response = _client.Execute(restRequest, Method.GET);
            return response.Content.DeserializeFromJson<T>();
        }

        private T ExecuteHttpPost<T>(string api, object body)
        {
            var restRequest = new RestRequest(_configuration.DataServiceUrl + api, Method.POST);

            restRequest.RequestFormat = DataFormat.Json;

            restRequest.AddJsonBody(body);

            var response = _client.Execute(restRequest, Method.POST);
            return response.Content.DeserializeFromJson<T>();
        }
    }
}