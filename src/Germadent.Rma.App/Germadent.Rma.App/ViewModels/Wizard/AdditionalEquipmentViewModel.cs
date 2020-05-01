using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class AdditionalEquipmentViewModel : ViewModelBase
    {     
        private int _quantity;

        public AdditionalEquipmentViewModel(DictionaryItemDto dictionaryItemDto)
        {
            EquipmentId = dictionaryItemDto.Id;
            DisplayName = dictionaryItemDto.Name;
        }

        public int EquipmentId { get; }

        public string DisplayName { get; }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity == value)
                    return;

                _quantity = value;
                OnPropertyChanged(() => Quantity);
            }
        }

        public AdditionalEquipmentDto ToDto()
        {
            return new AdditionalEquipmentDto
            {
                EquipmentId = EquipmentId,
                Quantity = Quantity
            };
        }

        public void Initialize(AdditionalEquipmentDto additionalEquipmentDto)
        {
            Quantity = additionalEquipmentDto.Quantity;
        }
    }
}
