using System;
using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.ToothCard
{
    public class CheckableDictionaryItemViewModel : ViewModelBase
    {
        private bool _isChecked;
        private DictionaryItemDto _item;

        public CheckableDictionaryItemViewModel(DictionaryItemDto dictionaryItemDto)
        {           
            _item = dictionaryItemDto;
        }

        public string DisplayName => _item.Name;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked == value)
                    return;
                _isChecked = value;
                OnPropertyChanged(() => IsChecked);

                Checked?.Invoke(this, EventArgs.Empty);
            }
        }

        public DictionaryItemDto Item
        {
            get => _item;
            set
            {
                if (_item == value)
                    return;
                _item = value;
                OnPropertyChanged(() => Item);
            }
        }

        public event EventHandler<EventArgs> Checked;

        public void ResetIsChanged()
        {
            _isChecked = false;
            OnPropertyChanged(() => IsChecked);
        }

        public override string ToString()
        {
            if (IsChecked)
                return string.Format("{0}, IsChecked={1}", DisplayName, IsChecked);

            return string.Format("{0}", DisplayName);
        }
    }
}
