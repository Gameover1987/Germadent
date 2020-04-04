using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.Views.Wizard;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Infrastructure
{
    public interface IWindowManager
    {
        OrderDto CreateLabOrder(OrderDto order, WizardMode mode);

        OrderDto CreateMillingCenterOrder(OrderDto order, WizardMode mode);

        OrdersFilter CreateOrdersFilter();

        ICustomerViewModel SelectCustomer(string mask);
    }
}