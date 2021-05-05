namespace Germadent.Model
{
    public interface IUserManager
    {
        bool HasRight(string rightName);

        AuthorizationInfoDto AuthorizationInfo { get; }
    }
}