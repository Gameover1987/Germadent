using System;
using System.Collections.ObjectModel;
using Germadent.Model;
using Germadent.Rma.App.ViewModels.ToothCard;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public class ProductCheckedEventArgs : EventArgs
    {
        public ProductCheckedEventArgs(ProductViewModel product)
        {
            Product = product;
        }

        public ProductViewModel Product{ get; }
    }   

    public interface IPriceListViewModel
    {
        void Initialize(BranchType branchType);

        void Setup(ToothDto toothDto);

        ObservableCollection<ProductViewModel> Products { get; }

        event EventHandler ProductChecked;
    }
}