﻿using Germadent.Common.Logging;
using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.ServiceClient;
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