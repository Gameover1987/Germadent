using Germadent.Client.Common.Converters;
using Germadent.Model;

namespace Germadent.Rms.App.ViewModels
{
    public interface IOrderSummaryViewModel
    {
        void Initialize(OrderDto orderDto);
    }
}
