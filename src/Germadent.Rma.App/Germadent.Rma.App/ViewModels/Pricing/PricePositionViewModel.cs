using System;
using Germadent.Rma.Model.Pricing;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public class PricePositionViewModel : ViewModelBase
    {
        private PricePositionDto _pricePositionDto;

        public PricePositionViewModel(PricePositionDto pricePositionDto)
        {
            _pricePositionDto = pricePositionDto;
        }

        public int PricePositionId => _pricePositionDto.PricePositionId;

        public int PriceGroupId => _pricePositionDto.PriceGroupId;

        public string UserCode
        {
            get { return _pricePositionDto.UserCode; }
        }

        public string DisplayName
        {
            get { return _pricePositionDto.Name; }
        }

        public PricePositionDto ToDto() => _pricePositionDto;

        public void Update(PricePositionDto pricePositionDto)
        {
            _pricePositionDto = pricePositionDto;
            OnPropertyChanged();
        }
    }
}