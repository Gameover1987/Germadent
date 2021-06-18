using System.Linq;
using Germadent.Model;

namespace Germadent.Rma.App.ServiceClient
{
    public class RmaUserManager : IUserManager
    {
        private readonly IRmaServiceClient _rmaServiceClient;

        public RmaUserManager(IRmaServiceClient rmaServiceClient)
        {
            _rmaServiceClient = rmaServiceClient;
        }

        public bool HasRight(string rightName)
        {
            return _rmaServiceClient.AuthorizationInfo.Rights.Any(x => x.RightName == rightName);
        }

        public AuthorizationInfoDto AuthorizationInfo
        {
            get { return _rmaServiceClient.AuthorizationInfo; }
        }
    }
}