using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockAddCustomerViewModel : AddCustomerViewModel
    {
        public DesignMockAddCustomerViewModel()
        {
            var customer = new CustomerDto
            {
                Name = "ООО Пошла родимая",
                Phone = "222-333-444",
                WebSite = "http://megasite.ru",
                Description = "Lorem ipsum dolor sit amet"
            };
            Initialize("Добавление", customer);
        }
    }
}

