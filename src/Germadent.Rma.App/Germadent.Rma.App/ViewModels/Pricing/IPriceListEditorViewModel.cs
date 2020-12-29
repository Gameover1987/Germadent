using System;
using Germadent.Rma.Model;
using Germadent.UI.Commands;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public interface IPriceListEditorViewModel
    {
        BranchType BranchType { get; set; }

        IDelegateCommand EditPriceGroupCommand { get; }

        IDelegateCommand EditPricePositionCommand { get; }

        bool IsBusy { get; }

        string BusyReason { get; }

        event EventHandler<EventArgs> IsBusyChanged;

        void Initialize();
    }
}