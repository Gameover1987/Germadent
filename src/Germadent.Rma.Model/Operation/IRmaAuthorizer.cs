namespace Germadent.Rma.Model.Operation
{
    public interface IRmaAuthorizer
    {
        void Authorize(string user, string password);
    }
}
