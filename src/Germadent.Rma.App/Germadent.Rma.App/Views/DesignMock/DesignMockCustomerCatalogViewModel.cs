﻿using System.Linq;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockCustomerCatalogViewModel : CustomerCatalogViewModel
    {
        public DesignMockCustomerCatalogViewModel()
            : base(new DesignMockCustomerRepository(), new DesignMockCatalogUIOperations(), new MockLogger())
        {
            Initialize();

            SelectedCustomer = Customers.LastOrDefault();
        }
    }
}
