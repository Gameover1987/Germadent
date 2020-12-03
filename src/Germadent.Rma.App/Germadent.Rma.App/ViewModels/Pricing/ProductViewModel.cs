using System;
using System.Collections.Generic;
using System.Text;
using Germadent.Rma.Model.Pricing;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public class ProductViewModel : ViewModelBase
    {
        private readonly ProductDto _productDto;
        private bool _isChecked;

        public ProductViewModel(ProductDto productDto)
        {
            _productDto = productDto;
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked == value)
                    return;
                _isChecked = value;
                OnPropertyChanged(() => IsChecked);

                Checked?.Invoke(this, new ProductCheckedEventArgs(this));
            }
        }

        public int PricePositionId
        {
            get { return _productDto.PricePositionId; }
        }

        public int PriceGroupId
        {
            get { return _productDto.PriceGroupId; }
        }

        public string DisplayName
        {
            get { return _productDto.ProductName; }
        }

        public event EventHandler<ProductCheckedEventArgs> Checked;

        public void SetIsChecked(bool isChecked)
        {
            _isChecked = isChecked;
            OnPropertyChanged(() => IsChecked);
        }

        public ProductDto ToDto()
        {
            return _productDto;
        }
    }
}
