using Germadent.Rma.App.Views.DesignMock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Germadent.Rma.App.Test
{
    [TestClass]
    public class DesignMockTest
    {
        [TestMethod]
        public void ShouldCreateAllDesignMockViewModels()
        {
            var designMockMainViewModel = new DesignMockMainViewModel();
            var mockCustomerCatalogVm = new DesignMockCustomerCatalogViewModel();
            var designMockLaboratoryInfoWizardStepViewModel = new DesignMockLaboratoryInfoWizardStepViewModel();
            var designMockResponsiblePersonsCatalogViewModel = new DesignMockResponsiblePersonsCatalogViewModel();
            var designMockMillingCenterInfoWizardStepViewModel = new DesignMockMillingCenterInfoWizardStepViewModel();
            var designMockPriceListViewModel = new DesignMockPriceListViewModel();
            var designMockMillingCenterAdditionalEquipmentViewModel = new DesignMockAdditionalEquipmentWizardStepViewModel();
            var designMockAddPricePositionViewModel = new DesignMockAddPricePositionViewModel();
            var designMockPriceListEditorViewModel = new DesignMockPriceListEditorViewModel();
        }
    }
}