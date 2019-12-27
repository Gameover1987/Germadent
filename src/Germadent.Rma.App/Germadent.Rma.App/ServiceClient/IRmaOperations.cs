using Germadent.Rma.Model;

namespace Germadent.Rma.App.ServiceClient
{
    public interface IRmaOperations
    {
        OrderLiteDto[] GetOrders(OrdersFilter filter = null);

        OrderDto GetOrderDetails(int id);

        MaterialDto[] GetMaterials();

        OrderDto AddOrder(OrderDto order);

        OrderDto UpdateOrder(OrderDto order);
    }
}