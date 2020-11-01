using System;
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

    public interface IPriceListViewModel
    {
        void Initialize(BranchType branchType);

        void Setup(ToothDto toothDto);

        event EventHandler<PricePositionCheckedEventArgs> PricePositionChecked;
    }
}