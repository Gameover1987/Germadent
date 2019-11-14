namespace Germadent.ServiceClient.Operation
{
    public interface IRmaAuthorizer
    {
        void Authorize(string user, string password);
    }
}
