using Germadent.Rma.Model;

namespace Germadent.Rma.App.ServiceClient
{
    public interface IRmaOperations
    {
        Order[] GetOrders(OrdersFilter filter = null);

        Material[] GetMaterials();

        void AddOrder(Order order);
    }
}