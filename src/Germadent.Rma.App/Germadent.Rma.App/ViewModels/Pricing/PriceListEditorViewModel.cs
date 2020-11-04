using System.Collections.ObjectModel;
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

        void Initialize();
    }

    public class PriceListEditorViewModel : ViewModelBase, IPriceListEditorViewModel
    {
        private readonly IPriceListUIOperations _uiOperations;
        private readonly IUserManager _userManager;

        public PriceListEditorViewModel(IPriceListUIOperations uiOperations, IUserManager userManager)
        {
            _uiOperations = uiOperations;
            _userManager = userManager;

            Groups = new ObservableCollection<PriceGroupViewModel>();
            Positions = new ObservableCollection<PricePositionViewModel>();

            AddPriceGroupCommand = new DelegateCommand(AddPriceGroupCommandHandler, CanAddPriceGroupCommandHandler);
            EditPriceGroupCommand = new DelegateCommand(EditPriceGroupCommandHandler, CanEditPriceGroupCommandHandler);
            DeletePriceGroupCommand = new DelegateCommand(DeletePriceGroupCommandHandler, CanDeletePriceGroupCommandHandler);

            CanEditPriceList = _userManager.HasRight(RmaUserRights.EditPriceList);
        }

        public bool CanEditPriceList { get; }

        public BranchType BranchType { get; set; }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public ObservableCollection<PriceGroupViewModel> Groups { get; }

        public ObservableCollection<PricePositionViewModel> Positions { get; }

        public IDelegateCommand AddPriceGroupCommand { get; }

        public IDelegateCommand EditPriceGroupCommand { get; }

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

        private bool CanEditPriceGroupCommandHandler()
        {
            return CanEditPriceList;
        }

        private void EditPriceGroupCommandHandler()
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