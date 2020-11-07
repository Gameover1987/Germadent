using Germadent.Rma.App.ViewModels.Wizard.Catalogs;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockResponsiblePersonsCatalogViewModel : ResponsiblePersonCatalogViewModel
    {
        public DesignMockResponsiblePersonsCatalogViewModel() 
            : base(new DesignMockResponsiblePersonRepository(), new DesignMockCatalogUIOperations(), new MockLogger())
        {
        }
    }
}