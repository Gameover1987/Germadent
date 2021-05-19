using Germadent.Client.Common.Configuration;
using Germadent.Client.Common.ServiceClient.Notifications;
using Germadent.Common;
using Germadent.Common.Web;
using Germadent.Model;
using RestSharp;

namespace Germadent.Client.Common.ServiceClient
{
    /// <summary>
    /// Сервис клиент с поддержкой авторизации
    /// </summary>
    public abstract class AuthSupportableServiceClient : ServiceClientBase, IAuthSupportableServiceClient
    {
        private readonly IClientConfiguration _clientConfiguration;
        private readonly ISignalRClient _signalRClient;

        public AuthSupportableServiceClient(IClientConfiguration clientConfiguration, ISignalRClient signalRClient)
        {
            _clientConfiguration = clientConfiguration;
            _signalRClient = signalRClient;
        }

        public void Authorize(string login, string password)
        {
            var info = ExecuteHttpGet<AuthorizationInfoDto>(
                _clientConfiguration.DataServiceUrl + string.Format("/api/auth/authorize/{0}/{1}", login, password));

            AuthorizationInfo = info;
            AuthenticationToken = info.Token;

            if (AuthorizationInfo.IsLocked)
                throw new UserMessageException("Учетная запись заблокирована.");

            if (!CheckRunApplicationRight())
                throw new UserMessageException("Отсутствует право на запуск приложения");

            _signalRClient.Initialize(info);
        }

        /// <summary>
        /// Данные авторизации
        /// </summary>
        public AuthorizationInfoDto AuthorizationInfo { get; private set; }

        /// <summary>
        /// Конфигурация клиента
        /// </summary>
        public IClientConfiguration Configuration => _clientConfiguration;

        /// <summary>
        /// Проверка права на запуск приложения
        /// </summary>
        /// <returns></returns>
        protected abstract bool CheckRunApplicationRight();

        /// <summary>
        /// Обработчик ошибок при запросе на сервис
        /// </summary>
        /// <param name="response"></param>
        protected override void HandleError(IRestResponse response)
        {
            throw new ServerSideException(response);
        }
    }
}
