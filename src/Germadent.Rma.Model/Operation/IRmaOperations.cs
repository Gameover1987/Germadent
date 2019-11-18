namespace Germadent.Rma.Model.Operation
{
    public interface IRmaOperations
    {
        Order[] GetOrders(OrdersFilter filter = null);

        Material[] GetMaterials();
    }

    public class RmaOperations : IRmaOperations
    {
        public Order[] GetOrders(OrdersFilter ordersFilter = null)
        {
            throw new System.NotImplementedException();
        }

        public Material[] GetMaterials()
        {
            throw new System.NotImplementedException();
        }
    }
}