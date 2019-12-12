using Germadent.Rma.Model;

namespace Germadent.Rma.App.ServiceClient
{
    public interface IRmaOperations
    {
        OrderLite[] GetOrders(OrdersFilter filter = null);

        Order GetOrderDetails(int id);

        Material[] GetMaterials();

        Order AddOrder(Order order);

        Order UpdateOrder(Order order);
    }
}