using Germadent.Model;

namespace Germadent.Rms.App.ViewModels
{
    public interface IOrderSummaryViewModel
    {
        void Initialize(OrderDto orderDto);
    }

    public class OrderSummaryViewModel : IOrderSummaryViewModel
    {
        public void Initialize(OrderDto orderDto)
        {
            throw new System.NotImplementedException();
        }
    }
}
