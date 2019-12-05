using Germadent.Rma.Model;

namespace Germadent.Rma.App.ServiceClient
{
    public interface IRmaOperations
    {
        Order[] GetOrders(OrdersFilter filter = null);

        T GetOrderDetails<T>(int id);

        Material[] GetMaterials();

        LaboratoryOrder AddLaboratoryOrder(LaboratoryOrder order);

        LaboratoryOrder UpdateLaboratoryOrder(LaboratoryOrder order);
    }
}