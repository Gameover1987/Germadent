using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public class MaterialViewModel : ViewModelBase
    {
        private bool _isChecked;
        private MaterialDto _item;

        public MaterialViewModel(MaterialDto material)
        {
            _item = material;
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set { SetProperty(() => _isChecked, value); }
        }

        public MaterialDto Item
        {
            get { return _item; }
            set { SetProperty(() => _item, value); }
        }
    }
}