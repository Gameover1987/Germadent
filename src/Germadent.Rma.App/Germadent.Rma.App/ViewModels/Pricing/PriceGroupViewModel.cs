using Germadent.Rma.Model.Pricing;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public class PriceGroupViewModel : ViewModelBase
    {
        private readonly PriceGroupDto _priceGroupDto;

        private bool _isChecked;

        public PriceGroupViewModel(PriceGroupDto priceGroupDto)
        {
            _priceGroupDto = priceGroupDto;
        }

        public string DisplayName => _priceGroupDto.Name;
    }
}