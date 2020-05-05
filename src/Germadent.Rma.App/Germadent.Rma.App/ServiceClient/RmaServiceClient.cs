using System;
using Germadent.Common.Extensions;
using Germadent.Common.FileSystem;
using Germadent.Rma.App.Configuration;
using Germadent.Rma.Model;
using RestSharp;

namespace Germadent.Rma.App.ServiceClient
{
    public class RmaServiceClient : IRmaServiceClient
    {
        private readonly IConfiguration _configuration;
        private readonly IFileManager _fileManager;
        private readonly RestClient _client;

        public RmaServiceClient(IConfiguration configuration, IFileManager fileManager)
        {
            _configuration = configuration;
            _fileManager = fileManager;

            _client = new RestClient();
        }

        public void Authorize(string user, string password)
        {
            throw new System.NotImplementedException();
        }

        public OrderLiteDto[] GetOrders(OrdersFilter ordersFilter)
        {
            return ExecuteHttpPost<OrderLiteDto[]>("/api/OrdersList", ordersFilter);
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
            return ExecuteHttpPost<OrderDto>("/api/orders", order, order.DataFileName == null ? null : _fileManager.ReadAllBytes(order.DataFileName));
        }

        public OrderDto UpdateOrder(OrderDto order)
        {
            return ExecuteHttpPut<OrderDto>("/api/orders", order);
        }

        public OrderDto CloseOrder(int id)
        {
            var api = string.Format("/api/orders/{0}", id);
            return ExecuteHttpDelete<OrderDto>(api);
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
            var addedCustomer = ExecuteHttpPost<CustomerDto>("/api/customers", сustomerDto);
            CustomerRepositoryChanged?.Invoke(this, new CustomerRepositoryChangedEventArgs(new []{addedCustomer}, null));
            return addedCustomer;
        }

        public DictionaryItemDto[] GetDictionary(DictionaryType dictionaryType)
        {
            return ExecuteHttpGet<DictionaryItemDto[]>(string.Format("/api/Dictionaries/{0}", dictionaryType));
        }

        public event EventHandler<CustomerRepositoryChangedEventArgs> CustomerRepositoryChanged;

        private T ExecuteHttpGet<T>(string api)
        {
            var restRequest = new RestRequest(_configuration.DataServiceUrl + api, Method.GET);
            restRequest.RequestFormat = DataFormat.Json;
            var response = _client.Execute(restRequest, Method.GET);
            return response.Content.DeserializeFromJson<T>();
        }

        private T ExecuteHttpPost<T>(string api, object body, byte[] file = null)
        {
            var restRequest = new RestRequest(_configuration.DataServiceUrl + api, Method.POST);

            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddHeader("Accept", "application/json");

            restRequest.AddJsonBody(body);

            if (file != null)
            {
                restRequest.AddHeader("Content-Type", "multipart/form-data");
                restRequest.AddFile("DataFile", file, "DataFile");
            }

            var response = _client.Execute(restRequest, Method.POST);
            return response.Content.DeserializeFromJson<T>();
        }

        public T ExecuteHttpPut<T>(string api, object body, byte[] file = null)
        {
            var restRequest = new RestRequest(_configuration.DataServiceUrl + api, Method.PUT);

            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddHeader("Accept", "application/json");

            restRequest.AddJsonBody(body);

            if (file != null)
            {
                restRequest.AddHeader("Content-Type", "multipart/form-data");
                restRequest.AddFile("DataFile", file, "DataFile");
            }

            var response = _client.Execute(restRequest, Method.PUT);
            return response.Content.DeserializeFromJson<T>();
        }

        public T ExecuteHttpDelete<T>(string api)
        {
            var restRequest = new RestRequest(_configuration.DataServiceUrl + api, Method.DELETE);
            restRequest.RequestFormat = DataFormat.Json;
            var response = _client.Execute(restRequest, Method.DELETE);
            return response.Content.DeserializeFromJson<T>();
        }
    }
}