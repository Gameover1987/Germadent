using Germadent.ServiceClient.Model;

namespace Germadent.ServiceClient.Operation
{
    public interface IRmaOperations
    {
        Order[] GetOrders();
    }

    public class RmaOperations : IRmaOperations
    {
        public Order[] GetOrders()
        {
            throw new System.NotImplementedException();
        }
    }
}