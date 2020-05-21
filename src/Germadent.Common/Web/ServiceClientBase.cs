using System.Net;
using Germadent.Common.Extensions;
using RestSharp;

namespace Germadent.Common.Web
{
    public class ServiceClientBase
    {
        private readonly RestClient _client = new RestClient();

        protected T ExecuteHttpGet<T>(string api)
        {
            var restRequest = new RestRequest(api, Method.GET);
            restRequest.RequestFormat = DataFormat.Json;
            var response = _client.Execute(restRequest, Method.GET);
            return response.Content.DeserializeFromJson<T>();
        }

        protected T ExecuteHttpPost<T>(string api, object body)
        {
            var restRequest = new RestRequest(api, Method.POST);
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddJsonBody(body);
            var response = _client.Execute(restRequest, Method.POST);
            return response.Content.DeserializeFromJson<T>();
        }

        protected void ExecuteFileUpload(string api, string filePath)
        {
            var restRequest = new RestRequest(api, Method.POST);
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddHeader("Content-Type", "multipart/form-data");
            restRequest.AddFile("DataFile", filePath, "DataFile");

            var response = _client.Execute(restRequest, Method.POST);
        }

        protected T ExecuteHttpPut<T>(string api, object body, byte[] file = null)
        {
            var restRequest = new RestRequest(api, Method.PUT);

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

        protected T ExecuteHttpDelete<T>(string api)
        {
            var restRequest = new RestRequest(api, Method.DELETE);
            restRequest.RequestFormat = DataFormat.Json;
            var response = _client.Execute(restRequest, Method.DELETE);
            return response.Content.DeserializeFromJson<T>();
        }
    }
}
