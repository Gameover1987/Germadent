using System;
using System.Net.Http;
using Germadent.Common.Extensions;
using Germadent.Common.Web;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;

namespace Germadent.Common.Web
{
    public abstract class ServiceClientBase
    {
        private readonly RestClient _client = new RestClient();

        public ServiceClientBase()
        {
            _client.ThrowOnAnyError = true;
            _client.AddHandler("application/json", () => NewtonsoftJsonSerializer.Default);
        }

        protected string AuthenticationToken { get; set; }

        protected T ExecuteHttpGet<T>(string api)
        {
            var request = new RestRequest(api, Method.GET);
            if (!AuthenticationToken.IsNullOrEmpty())
            {
                request.AddHeader("Authorization", string.Format("Bearer {0}", AuthenticationToken));
                _client.Authenticator = new JwtAuthenticator(AuthenticationToken);
                _client.Authenticator.Authenticate(_client, request);
            }
            request.RequestFormat = DataFormat.Json;
            var response = _client.Execute(request, Method.GET);
            ThrowIfError(response);
            return response.Content.DeserializeFromJson<T>();
        }

        protected T ExecuteHttpPost<T>(string api, object body)
        {
            var request = new RestRequest(api, Method.POST);

            if (!AuthenticationToken.IsNullOrEmpty())
            {
                request.AddHeader("Authorization", string.Format("Bearer {0}", AuthenticationToken));
                _client.Authenticator = new JwtAuthenticator(AuthenticationToken);
                _client.Authenticator.Authenticate(_client, request);
            }
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = NewtonsoftJsonSerializer.Default;
            request.AddJsonBody(body);
            var response = _client.Execute(request, Method.POST);
            ThrowIfError(response);
            return response.Content.DeserializeFromJson<T>();
        }

        protected T ExecuteHttpDelete<T>(string api)
        {
            var request = new RestRequest(api, Method.DELETE);
            if (!AuthenticationToken.IsNullOrEmpty())
            {
                request.AddHeader("Authorization", string.Format("Bearer {0}", AuthenticationToken));
                _client.Authenticator = new JwtAuthenticator(AuthenticationToken);
                _client.Authenticator.Authenticate(_client, request);
            }
            request.RequestFormat = DataFormat.Json;
            var response = _client.Execute(request, Method.DELETE);
            ThrowIfError(response);
            return response.Content.DeserializeFromJson<T>();
        }

        protected void ExecuteHttpDelete(string api)
        {
            var restRequest = new RestRequest(api, Method.DELETE);
            restRequest.RequestFormat = DataFormat.Json;
            var response = _client.Execute(restRequest, Method.DELETE);
            ThrowIfError(response);
        }

        protected void ExecuteFileUpload(string api, string filePath)
        {
            var restRequest = new RestRequest(api, Method.POST);
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddHeader("Content-Type", "multipart/form-data");
            restRequest.AddFile("DataFile", filePath, "DataFile");

            var response = _client.Execute(restRequest, Method.POST);
            ThrowIfError(response);
        }

        protected byte[] ExecuteFileDownload(string api)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(api);

            HttpResponseMessage responseParent = client.GetAsync(api).Result;

            var respMessage = responseParent.Content.ReadAsByteArrayAsync().Result;

            return respMessage;
        }

        private void ThrowIfError(IRestResponse response)
        {
            if (response.IsSuccessful)
                return;

            HandleError(response);
        }

        protected abstract void HandleError(IRestResponse response);
    }
}