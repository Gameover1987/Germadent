using Germadent.ServiceClient.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public class MaterialViewModel : ViewModelBase
    {
        private bool _isChecked;
        private Material _item;

        public MaterialViewModel(Material material)
        {
            _item = material;
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set { SetProperty(() => _isChecked, value); }
        }

        public Material Item
        {
            get { return _item; }
            set { SetProperty(() => _item, value); }
        }
    }
}