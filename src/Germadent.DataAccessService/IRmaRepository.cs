using Germadent.Rma.Model;

namespace Germadent.DataAccessService
{
    public interface IRmaRepository
    {
        Order GetOrderDetails(int id);

        OrderLite[] GetOrders(OrdersFilter filter);

        void AddLabOrder(LaboratoryOrder laboratoryOrder);

        void UpdateLabOrder(LaboratoryOrder laboratoryOrder);

        void AddMcOrder(MillingCenterOrder millingCenterOrder);

        Material[] GetMaterials();
    }
}