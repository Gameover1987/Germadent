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

        public int PriceGroupId => _priceGroupDto.Id;

        public string DisplayName => _priceGroupDto.Name;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked == value)
                    return;
                _isChecked = value;
                OnPropertyChanged(() => IsChecked);
            }
        }
    }

    public class PricePositionViewModel : ViewModelBase
    {
        private readonly PricePositionDto _pricePositionDto;

        private bool _isChecked;

        public PricePositionViewModel(PricePositionDto pricePositionDto)
        {
            _pricePositionDto = pricePositionDto;
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked == value)
                    return;
                _isChecked = value;
                OnPropertyChanged(() => IsChecked);
            }
        }

        public string UserCode
        {
            get { return _pricePositionDto.UserCode; }
        }

        public string DisplayName
        {
            get { return _pricePositionDto.Name; }
        }
    }
}