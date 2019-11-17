using Germadent.Common;
using Germadent.Common.Extensions;
using Germadent.ServiceClient.Operation;

namespace Germadent.Rma.App.Mocks
{
    public class MockRmaAuthorizer : IRmaAuthorizer
    {
        public void Authorize(string user, string password)
        {
            if (!user.IsNullOrWhiteSpace())
                return;

            throw new UserMessageException("Пользователь не авторизован");
        }
    }
}