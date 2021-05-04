using Germadent.Model;
using Germadent.Rma.App.Views.Wizard;

namespace Germadent.Rma.App.Operations
{
    public interface IOrderUIOperations
    {
        OrderDto CreateLabOrder(OrderDto order, WizardMode mode);

        OrderDto CreateMillingCenterOrder(OrderDto order, WizardMode mode);

        OrdersFilter CreateOrdersFilter(OrdersFilter filter);
    }
}