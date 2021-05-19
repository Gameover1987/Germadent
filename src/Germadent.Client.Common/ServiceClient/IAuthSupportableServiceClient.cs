using Germadent.Client.Common.Configuration;
using Germadent.Model;

namespace Germadent.Client.Common.ServiceClient
{
    /// <summary>
    /// Сервис клиент с поддержкой авторизации
    /// </summary>
    public interface IAuthSupportableServiceClient
    {
        AuthorizationInfoDto AuthorizationInfo { get; }

        IClientConfiguration Configuration { get; }

        void Authorize(string login, string password);
    }
}