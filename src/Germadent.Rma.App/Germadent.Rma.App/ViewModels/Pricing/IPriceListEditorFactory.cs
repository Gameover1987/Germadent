using Germadent.Rma.App.Operations;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.Model;
using Germadent.UI.Infrastructure;
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
        private readonly IUserManager _userManager;
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IRmaServiceClient _serviceClient;

        public PriceListEditorFactory(IPriceGroupRepository priceGroupRepository, IPricePositionRepository pricePositionRepository, IUserManager userManager, IShowDialogAgent dialogAgent, IRmaServiceClient serviceClient)
        {
            _priceGroupRepository = priceGroupRepository;
            _pricePositionRepository = pricePositionRepository;
            _userManager = userManager;
            _dialogAgent = dialogAgent;
            _serviceClient = serviceClient;
        }

        public IPriceListEditorViewModel CreateEditor(BranchType branchType)
        {
            var editor = new PriceListEditorViewModel(_userManager, _priceGroupRepository, _pricePositionRepository, _dialogAgent, _serviceClient);
            editor.BranchType = branchType;
            return editor;
        }
    }
}