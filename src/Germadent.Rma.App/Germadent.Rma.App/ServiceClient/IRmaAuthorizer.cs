namespace Germadent.Rma.App.ServiceClient
{
    public interface IRmaAuthorizer
    {
        void Authorize(string user, string password);
    }
}
