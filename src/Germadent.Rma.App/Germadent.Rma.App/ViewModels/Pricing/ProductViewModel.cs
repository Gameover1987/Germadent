using System;
using System.Collections.Generic;
using System.Text;
using Germadent.Common.Extensions;
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

        public int ProductId
        {
            get { return _productDto.ProductId; }
        }

        public int PriceGroupId
        {
            get { return _productDto.PriceGroupId; }
        }

        public string Caption
        {
            get
            {
                if (MaterialName.IsNullOrWhiteSpace())
                    return _productDto.ProductName;

                return string.Format("{0} / {1}", _productDto.ProductName, _productDto.MaterialName);
            }
        }

        public string UserCode
        {
            get { return _productDto.PricePositionCode; }
        }

        public string MaterialName
        {
            get { return _productDto.MaterialName; }
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
