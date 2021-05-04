using Germadent.Model;

namespace Germadent.Client.Common.ViewModels
{
    public interface IOrdersFilterViewModel
    {
        OrdersFilter GetFilter();

        void Initialize(OrdersFilter filter);
    }
}