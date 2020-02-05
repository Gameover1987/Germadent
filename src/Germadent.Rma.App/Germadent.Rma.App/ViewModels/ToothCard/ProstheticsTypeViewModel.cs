using System;
using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.ToothCard
{
    public class ProstheticsTypeViewModel : ViewModelBase
    {
        private bool _isChecked;
        private ProstheticsTypeDto _item;

        public ProstheticsTypeViewModel(ProstheticsTypeDto prostheticsTypeDto)
        {
            _item = prostheticsTypeDto;
        }

        public string DisplayName => _item.Name;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (SetProperty(() => _isChecked, value))
                {
                    Checked?.Invoke(this, EventArgs.Empty);
                };
            }
        }

        public ProstheticsTypeDto Item
        {
            get { return _item; }
            set { SetProperty(() => _item, value); }
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