using System;
using Germadent.Rma.Model.Pricing;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public class PricePositionViewModel : ViewModelBase
    {
        private readonly PricePositionDto _pricePositionDto;

        private bool _isChecked;

        public PricePositionViewModel(PricePositionDto pricePositionDto)
        {
            _pricePositionDto = pricePositionDto;
        }

        public int PricePositionId => _pricePositionDto.PricePositionId;

        public int PriceGroupId => _pricePositionDto.PriceGroupId;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked == value)
                    return;
                _isChecked = value;
                OnPropertyChanged(() => IsChecked);

                Checked?.Invoke(this, new PricePositionCheckedEventArgs(this));
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

        public event EventHandler<PricePositionCheckedEventArgs> Checked;

        public PricePositionDto ToDto() => _pricePositionDto;

        public void SetIsChecked(bool isChecked)
        {
            _isChecked = isChecked;
            OnPropertyChanged(() => IsChecked);
        }
    }
}