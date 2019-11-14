using Germadent.ServiceClient.Model;

namespace Germadent.ServiceClient.Operation
{
    public interface IRmaOperations
    {
        Order[] GetOrders();

        Material[] GetMaterials();
    }

    public class RmaOperations : IRmaOperations
    {
        public Order[] GetOrders()
        {
            throw new System.NotImplementedException();
        }

        public Material[] GetMaterials()
        {
            throw new System.NotImplementedException();
        }
    }
}