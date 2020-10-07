using System;
using System.Collections.Generic;
using System.Text;
using Germadent.Rma.App.Views.DesignMock;
using NUnit.Framework;

namespace Germadent.Rma.App.Test
{
    [TestFixture]
    public class DesignMockTest
    {
        [Test]
        public void ShouldCreateAllDesignMockViewModels()
        {
            var designMockMainViewModel = new DesignMockMainViewModel();
            var mockCustomerCatalogVm = new DesignMockCustomerCatalogViewModel();
            var designMockLaboratoryInfoWizardStepViewModel = new DesignMockLaboratoryInfoWizardStepViewModel();
            var designMockResponsiblePersonsCatalogViewModel = new DesignMockResponsiblePersonsCatalogViewModel();
            var designMockMillingCenterInfoWizardStepViewModel = new DesignMockMillingCenterInfoWizardStepViewModel();
            var designMockLaboratoryProjectWizardStepViewModel = new DesignMockLaboratoryProjectWizardStepViewModel();
        }
    }
}