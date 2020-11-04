using Germadent.UserManagementCenter.Model;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockUserManager : IUserManager
    {
        public bool HasRight(string rightName)
        {
            return true;
        }

        public AuthorizationInfoDto AuthorizationInfo { get; }
    }
}