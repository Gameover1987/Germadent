using System;
using System.Linq;
using Germadent.Common;
using Germadent.Rma.Model.Pricing;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public class PricePositionViewModel : ViewModelBase
    {
        private PricePositionDto _pricePositionDto;
        private readonly IDateTimeProvider _dateTimeProvider;

        public PricePositionViewModel(PricePositionDto pricePositionDto, IDateTimeProvider dateTimeProvider)
        {
            _pricePositionDto = pricePositionDto;
            _dateTimeProvider = dateTimeProvider;
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

        public decimal PriceModel
        {
            get { return _pricePositionDto.Prices.GetCurrentPrice(_dateTimeProvider.GetDateTime()).PriceModel; }
        }

        public decimal PriceStl
        {
            get { return _pricePositionDto.Prices.GetCurrentPrice(_dateTimeProvider.GetDateTime()).PriceStl; }
        }

        public PricePositionDto ToDto() => _pricePositionDto;

        public void Update(PricePositionDto pricePositionDto)
        {
            _pricePositionDto = pricePositionDto;
            OnPropertyChanged();
        }
    }
}