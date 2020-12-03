using System;
using Germadent.Rma.Model.Pricing;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public class PriceGroupViewModel : ViewModelBase
    {
        private readonly PriceGroupDto _priceGroupDto;

        private bool _hasChanges;

        public PriceGroupViewModel(PriceGroupDto priceGroupDto)
        {
            _priceGroupDto = priceGroupDto ?? throw new ArgumentNullException(nameof(priceGroupDto));
        }

        public int PriceGroupId => _priceGroupDto.PriceGroupId;

        public string DisplayName
        {
            get { return _priceGroupDto.Name; }
            set
            {
                if (_priceGroupDto.Name == value)
                    return;
                _priceGroupDto.Name = value;
                OnPropertyChanged(() => DisplayName);
            }
        } 

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges == value)
                    return;
                _hasChanges = value;
                OnPropertyChanged(() => HasChanges);
            }
        }

        public PriceGroupDto ToDto()
        {
            return _priceGroupDto;
        }
    }
}