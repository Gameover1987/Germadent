using System;
using Germadent.Model.Pricing;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public enum PriceKind
    {
        Past = 0,
        Current = 1,
        Future = 2
    }

    public class PriceViewModel : ViewModelBase
    {
        private PriceDto _priceDto;
        private PriceKind _priceKind;

        public PriceViewModel(PriceDto priceDto)
        {
            _priceDto = priceDto;
        }

        public DateTime Begin
        {
            get { return _priceDto.DateBeginning; }
        }

        public decimal PriceStl
        {
            get { return _priceDto.PriceStl; }
        }

        public decimal PriceModel
        {
            get { return _priceDto.PriceModel; }
        }

        public PriceKind PriceKind
        {
            get { return _priceKind; }
            set
            {
                if (_priceKind == value)
                    return;
                _priceKind = value;
                OnPropertyChanged(() => PriceKind);
            }
        }

        public PriceDto ToDto() => _priceDto;

        public void Update(PriceDto price)
        {
            _priceDto = price;
            OnPropertyChanged();
        }
    }
}