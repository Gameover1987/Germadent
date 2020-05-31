using System;
using System.Net;
using RestSharp;

namespace Germadent.Common.Web
{
    public class ServerSideException : Exception
    {
        private readonly IRestResponse _response;

        public ServerSideException(IRestResponse response)
        {
            _response = response;
        }

        public Uri Api => _response.ResponseUri;

        public HttpStatusCode StatusCode => _response.StatusCode;

        public override string Message => $"При обращении к Web-API по адресу '{Api}' произошла ошибка '{StatusCode}'";
    }
}