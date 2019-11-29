using Germadent.Rma.Model;

namespace Germadent.DataAccessService
{
    public interface IOrdersRepository
    {
        Order GetOrderDetails(int id);

        Order[] GetOrders(OrdersFilter filter);

        void AddOrder(Order order);
    }
}