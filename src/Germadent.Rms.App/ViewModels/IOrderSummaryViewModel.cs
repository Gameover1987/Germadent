using Germadent.Client.Common.Converters;
using Germadent.Model;
using Germadent.Model.Production;

namespace Germadent.Rms.App.ViewModels
{
    public interface IOrderSummaryViewModel
    {
        void Initialize(OrderDto orderDto);

        WorkDto[] GetWorks();
    }
}
