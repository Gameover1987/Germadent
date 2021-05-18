using System.Linq;
using Germadent.Client.Common.Configuration;
using Germadent.Client.Common.ServiceClient.Notifications;
using Germadent.Common;
using Germadent.Common.Web;
using Germadent.Model;
using Germadent.Model.Production;
using Germadent.Model.Rights;
using RestSharp;

namespace Germadent.Rms.App.ServiceClient
{
    public class RmsServiceClient : ServiceClientBase, IRmsServiceClient
    {
        private readonly IClientConfiguration _configuration;
        private readonly ISignalRClient _signalRClient;

        public RmsServiceClient(IClientConfiguration configuration, ISignalRClient signalRClient)
        {
            _configuration = configuration;
            _signalRClient = signalRClient;
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

            _signalRClient.Initialize(info);
        }

        public OrderLiteDto[] GetOrders(OrdersFilter filter)
        {
            var api = _configuration.DataServiceUrl + "/api/Rma/Orders/getByFilter";
            return ExecuteHttpPost<OrderLiteDto[]>(api, filter);
        }

        public OrderScope GetOrderById(int workOrderId)
        {
            var order = ExecuteHttpGet<OrderDto>(_configuration.DataServiceUrl + $"/api/Rma/orders/{workOrderId}/{AuthorizationInfo.UserId}");
            return new OrderScope(this, order);
        }

        public AuthorizationInfoDto AuthorizationInfo { get; set; }

        public DictionaryItemDto[] GetDictionary(DictionaryType dictionaryType)
        {
            return ExecuteHttpGet<DictionaryItemDto[]>(_configuration.DataServiceUrl + $"/api/Rma/Dictionaries/{dictionaryType}");
        }

        public TechnologyOperationByUserDto[] GetRelevantWorkListByWorkOrder(int workOrderId)
        {
            return ExecuteHttpGet<TechnologyOperationByUserDto[]>(_configuration.DataServiceUrl + $"/api/Rms/OrdersProcessing/GetRelevantOperationsByWorkOrder/{workOrderId}/{AuthorizationInfo.UserId}");
        }

        public void StartWorks(WorkDto[] works)
        {
            ExecuteHttpPost<TechnologyOperationByUserDto[]>(_configuration.DataServiceUrl + $"/api/Rms/OrdersProcessing/StartWorks", works);
        }

        public void StartWork(WorkDto work, int lastEditorId)
        {
            
        }

        public void UpdateWork(WorkDto work, int lastEditorId)
        {

        }

        public void DeleteWork(WorkDto work)
        {

        }

        public void UnLockOrder(int workOrderId)
        {
            ExecuteHttpGet<OrderDto>(_configuration.DataServiceUrl + $"/api/Rma/orders/UnlockWorkOrder/{workOrderId}");
        }

        protected override void HandleError(IRestResponse response)
        {
            throw new ServerSideException(response);
        }
    }
}
