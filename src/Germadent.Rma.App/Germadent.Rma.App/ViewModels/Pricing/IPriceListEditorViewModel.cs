using Germadent.Rma.Model;
using Germadent.UI.Commands;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public interface IPriceListEditorViewModel
    {
        BranchType BranchType { get; set; }

        IDelegateCommand EditPriceGroupCommand { get; }

        IDelegateCommand EditPricePositionCommand { get; }

        void Initialize();
    }
}