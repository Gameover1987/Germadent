using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public class ProstheticsTypeViewModel : ViewModelBase
    {
        private bool _isChecked;
        private ProstheticsTypeDto _item;

        public ProstheticsTypeViewModel(ProstheticsTypeDto prostheticsTypeDto)
        {
            _item = prostheticsTypeDto;
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set { SetProperty(() => _isChecked, value); }
        }

        public ProstheticsTypeDto Item
        {
            get { return _item; }
            set { SetProperty(() => _item, value); }
        }
    }
}