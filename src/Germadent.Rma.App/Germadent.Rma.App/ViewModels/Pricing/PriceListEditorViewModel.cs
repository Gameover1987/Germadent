using Germadent.Rma.App.Operations;
using Germadent.Rma.Model;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;
using Germadent.UserManagementCenter.Model;
using Germadent.UserManagementCenter.Model.Rights;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public interface IPriceListEditorViewModel
    {
        BranchType BranchType { get; set; }
    }

    public class PriceListEditorViewModel : ViewModelBase, IPriceListEditorViewModel
    {
        private readonly IPriceListUIOperations _uiOperations;
        private readonly IUserManager _userManager;

        public PriceListEditorViewModel(IPriceListUIOperations uiOperations, IUserManager userManager)
        {
            _uiOperations = uiOperations;
            _userManager = userManager;

            AddPriceGroupCommand = new DelegateCommand(AddPriceGroupCommandHandler, CanAddPriceGroupCommandHandler);
            UpdatePriceGroupCommand = new DelegateCommand(UpdatePriceGroupCommandHandler, CanUpdatePriceGroupCommandHandler);
            DeletePriceGroupCommand = new DelegateCommand(DeletePriceGroupCommandHandler, CanDeletePriceGroupCommandHandler);

            CanEditPriceList = _userManager.HasRight(RmaUserRights.EditPriceList);
        }

        public bool CanEditPriceList { get; }

        public BranchType BranchType { get; set; }

        public IDelegateCommand AddPriceGroupCommand { get; }

        public IDelegateCommand UpdatePriceGroupCommand { get; }

        public IDelegateCommand DeletePriceGroupCommand { get; }

        private bool CanAddPriceGroupCommandHandler()
        {
            return CanEditPriceList;
        }

        private void AddPriceGroupCommandHandler()
        {
            var priceGroup = _uiOperations.AddPriceGroup(BranchType);
            if (priceGroup == null)
                return;
        }

        private bool CanUpdatePriceGroupCommandHandler()
        {
            return CanEditPriceList;
        }

        private void UpdatePriceGroupCommandHandler()
        {

        }

        private bool CanDeletePriceGroupCommandHandler()
        {
            return CanEditPriceList;
        }

        private void DeletePriceGroupCommandHandler()
        {

        }
    }
}