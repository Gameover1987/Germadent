using System.Collections.ObjectModel;
using System.Linq;
using Germadent.Model;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class AttributeViewModel : ViewModelBase
    {
        private AttributeDto _selectedValue;

        public AttributeViewModel(string attributeName, int selectedValueId, AttributeDto[] attributes)
        {
            AttributeName = attributeName;
            SelectedValue = attributes.FirstOrDefault(x => x.AttributeValueId == selectedValueId);
            Values = new ObservableCollection<AttributeDto>(attributes);

            CleanupCommand = new DelegateCommand(CleanupCommandHandler, CanCleanupCommandHandler);
        }

        public string AttributeName { get; }

        public ObservableCollection<AttributeDto> Values { get; } 

        public AttributeDto SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                if (_selectedValue == value)
                    return;

                _selectedValue = value;
                OnPropertyChanged(() => SelectedValue);
            }
        }

        public IDelegateCommand CleanupCommand { get; }

        private bool CanCleanupCommandHandler()
        {
            return SelectedValue != null;
        }

        private void CleanupCommandHandler()
        {
            SelectedValue = null;
        }
    }
}