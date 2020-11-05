using Germadent.Rma.App.Operations;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.Model;
using Germadent.UserManagementCenter.Model;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public interface IPriceListEditorFactory
    {
        IPriceListEditorViewModel CreateEditor(BranchType branchType);
    }

    public class PriceListEditorFactory : IPriceListEditorFactory
    {
        private readonly IPriceGroupRepository _priceGroupRepository;
        private readonly IPricePositionRepository _pricePositionRepository;
        private readonly IPriceListUIOperations _uiOperations;
        private readonly IUserManager _userManager;

        public PriceListEditorFactory(IPriceGroupRepository priceGroupRepository, IPricePositionRepository pricePositionRepository, IPriceListUIOperations uiOperations, IUserManager userManager)
        {
            _priceGroupRepository = priceGroupRepository;
            _pricePositionRepository = pricePositionRepository;
            _uiOperations = uiOperations;
            _userManager = userManager;
        }

        public IPriceListEditorViewModel CreateEditor(BranchType branchType)
        {
            var editor = new PriceListEditorViewModel(_uiOperations, _userManager, _priceGroupRepository, _pricePositionRepository);
            editor.BranchType = branchType;
            return editor;
        }
    }
}