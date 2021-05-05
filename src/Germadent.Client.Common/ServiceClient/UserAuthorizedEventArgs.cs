using System;
using Germadent.Model;

namespace Germadent.Client.Common.ServiceClient
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