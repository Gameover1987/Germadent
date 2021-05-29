using System.Linq;
using Germadent.Model;

namespace Germadent.Rms.App.ServiceClient
{
    public class RmsUserManager : IUserManager
    {
        private readonly IRmsServiceClient _rmsServiceClient;

        public RmsUserManager(IRmsServiceClient rmsServiceClient)
        {
            _rmsServiceClient = rmsServiceClient;
        }

        public bool HasRight(string rightName)
        {
            return _rmsServiceClient.AuthorizationInfo.Rights.Any(x => x.RightName == rightName);
        }

        public AuthorizationInfoDto AuthorizationInfo
        {
            get { return _rmsServiceClient.AuthorizationInfo; }
        }
    }
}