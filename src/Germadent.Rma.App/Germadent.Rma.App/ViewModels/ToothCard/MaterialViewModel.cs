using System;
using System.Collections.Generic;
using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.ToothCard
{
    public class MaterialViewModel : ViewModelBase
    {
        private bool _isChecked;
        private MaterialDto _item;

        public MaterialViewModel(MaterialDto material)
        {
            _item = material;
        }

        public string DisplayName => _item.MaterialName;

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

        public MaterialDto Item
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