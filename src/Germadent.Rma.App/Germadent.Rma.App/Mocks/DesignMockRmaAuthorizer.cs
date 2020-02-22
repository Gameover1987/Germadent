using Germadent.Common;
using Germadent.Rma.App.ServiceClient;

namespace Germadent.Rma.App.Mocks
{
    public class DesignMockRmaAuthorizer : IRmaAuthorizer
    {
        public void Authorize(string user, string password)
        {
            if (user == "Admin" && password == "Admin")
                return;

            throw new UserMessageException("Пользователь не авторизован");
        }
    }
}