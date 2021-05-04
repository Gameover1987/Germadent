using Germadent.Rma.Model.Pricing;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public interface IAddPriceViewModel
    {
        void Initialize(PriceDto price);

        PriceDto GetPrice();
    }
}