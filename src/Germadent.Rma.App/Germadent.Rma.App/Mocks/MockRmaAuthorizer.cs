using Germadent.Common;
using Germadent.Common.Extensions;

namespace Germadent.ServiceClient.Operation
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