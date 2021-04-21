using Germadent.UI.Commands;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public class ContextMenuItemViewModel : ViewModelBase
    {
        private bool _isChecked;

        public string Header { get; set; }

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

        public object Parameter { get; set; }

        public IDelegateCommand Command { get; set; }
    }
}