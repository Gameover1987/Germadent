using System;
using System.Collections.ObjectModel;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public class PricePositionCheckedEventArgs : EventArgs
    {
        public PricePositionCheckedEventArgs(PricePositionViewModel pricePosition)
        {
            PricePosition = pricePosition;
        }

        public PricePositionViewModel PricePosition { get; }
    }

    public class ProductFromSetCheckedEventArgs : EventArgs
    {
        public ProductFromSetCheckedEventArgs(ProductSetForPriceGroupViewModel product)
        {
            Product = product;
        }

        public ProductSetForPriceGroupViewModel Product { get; }
    }

    public interface IPriceListViewModel
    {
        void Initialize(BranchType branchType);

        void Setup(ToothDto toothDto);

        ObservableCollection<PricePositionViewModel> Positions { get; }

        event EventHandler PricePositionsChecked;

        event EventHandler ProductChecked;
    }
}