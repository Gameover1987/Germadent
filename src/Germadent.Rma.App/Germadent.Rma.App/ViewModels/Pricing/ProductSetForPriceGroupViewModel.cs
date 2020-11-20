using System;
using System.Collections.Generic;
using System.Text;
using Germadent.Rma.Model.Pricing;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public class ProductSetForPriceGroupViewModel : ViewModelBase
    {
        private ProductSetForPriceGroupDto _productSetForPriceGroupDto;

        private bool _isChecked;

        public ProductSetForPriceGroupViewModel(ProductSetForPriceGroupDto productSetForPriceGroupDto)
        {
            _productSetForPriceGroupDto = productSetForPriceGroupDto;
        }

        public int PricePositionId => _productSetForPriceGroupDto.PricePositionId;

        public int PriceGroupId => _productSetForPriceGroupDto.PriceGroupId;

        public int MaterialId => _productSetForPriceGroupDto.MaterialId;

        public int ProductId => _productSetForPriceGroupDto.ProductId;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked == value)
                    return;
                _isChecked = value;
                OnPropertyChanged(() => IsChecked);

                Checked?.Invoke(this, new ProductFromSetCheckedEventArgs(this));
            }
        }

        public string UserCode
        {
            get { return _productSetForPriceGroupDto.PricePositionCode; }
        }

        public string MaterialName
        {
            get { return _productSetForPriceGroupDto.MaterialName; }
        }

        public string ProductName
        {
            get { return _productSetForPriceGroupDto.ProductName; }
        }

        public decimal PriceStl
        {
            get { return _productSetForPriceGroupDto.PriceSTL; }
        }

        public decimal PriceModel
        {
            get { return _productSetForPriceGroupDto.PriceModel; }
        }

        public event EventHandler<ProductFromSetCheckedEventArgs> Checked;

        public void SetIsChecked(bool isChecked)
        {
            _isChecked = isChecked;
            OnPropertyChanged(() => IsChecked);
        }
    }
}
