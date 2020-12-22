using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class AdditionalEquipmentViewModel : ViewModelBase
    {     
        private int _quantityIn;

        private int _quantityOut;

        public AdditionalEquipmentViewModel(DictionaryItemDto dictionaryItemDto)
        {
            EquipmentId = dictionaryItemDto.Id;
            DisplayName = dictionaryItemDto.Name;
        }

        public int EquipmentId { get; }

        public string DisplayName { get; }

        public int QuantityIn
        {
            get => _quantityIn;
            set
            {
                if (_quantityIn == value)
                    return;

                _quantityIn = value;
                OnPropertyChanged(() => QuantityIn);
            }
        }

        public int QuantityOut
        {
            get => _quantityOut;
            set
            {
                if (_quantityOut == value)
                    return;

                _quantityOut = value;
                OnPropertyChanged(() => QuantityOut);
            }
        }

        public AdditionalEquipmentDto ToDto()
        {
            return new AdditionalEquipmentDto
            {
                EquipmentId = EquipmentId,
                QuantityIn = QuantityIn,
                QuantityOut = QuantityOut
            };
        }

        public void Initialize(AdditionalEquipmentDto additionalEquipmentDto)
        {
            QuantityIn = additionalEquipmentDto.QuantityIn;
            QuantityOut = additionalEquipmentDto.QuantityOut;
        }
    }
}
