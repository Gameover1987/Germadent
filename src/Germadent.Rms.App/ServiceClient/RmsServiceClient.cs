using System;
using System.Linq;
using Germadent.Common;
using Germadent.Common.Web;
using Germadent.Model;
using Germadent.Model.Rights;
using Germadent.Rms.App.Infrastructure;
using RestSharp;

namespace Germadent.Rms.App.ServiceClient
{
    public class RmsServiceClient : ServiceClientBase, IRmsServiceClient
    {
        private readonly IConfiguration _configuration;

        public RmsServiceClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Authorize(string login, string password)
        {
            var info = ExecuteHttpGet<AuthorizationInfoDto>(
                _configuration.DataServiceUrl + string.Format("/api/auth/authorize/{0}/{1}", login, password));

            AuthorizationInfo = info;
            AuthenticationToken = info.Token;

            if (AuthorizationInfo.IsLocked)
                throw new UserMessageException("Учетная запись заблокирована.");

            if (AuthorizationInfo.Rights.Count(x => x.RightName == RmsUserRights.RunApplication) == 0)
                throw new UserMessageException("Отсутствует право на запуск приложения");
        }

        public OrderLiteDto[] GetOrders(OrdersFilter filter)
        {
            var api = _configuration.DataServiceUrl + "/api/Rma/Orders/getByFilter";
            return ExecuteHttpPost<OrderLiteDto[]>(api, filter);
        }

        public OrderDto GetOrderById(int workOrderId)
        {
            throw new NotImplementedException();
        }

        public AuthorizationInfoDto AuthorizationInfo { get; set; }

        public DictionaryItemDto[] GetDictionary(DictionaryType dictionaryType)
        {
            return ExecuteHttpGet<DictionaryItemDto[]>(_configuration.DataServiceUrl + $"/api/Rma/Dictionaries/{dictionaryType}");
        }

        protected override void HandleError(IRestResponse response)
        {
            throw new ServerSideException(response);
        }
    }
}
