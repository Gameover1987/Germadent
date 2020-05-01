using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels
{
    public interface IOrdersFilterViewModel
    {
        OrdersFilter GetFilter();

        void Initialize();
    }
}