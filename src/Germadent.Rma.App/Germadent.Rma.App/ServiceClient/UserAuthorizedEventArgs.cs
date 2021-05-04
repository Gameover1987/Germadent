using System;
using Germadent.UserManagementCenter.Model;

namespace Germadent.Rma.App.ServiceClient
{
    public class UserAuthorizedEventArgs : EventArgs
    {
        public UserAuthorizedEventArgs(AuthorizationInfoDto info)
        {
            AuthorizationInfo = info;
        }

        public AuthorizationInfoDto AuthorizationInfo { get; }
    }
}