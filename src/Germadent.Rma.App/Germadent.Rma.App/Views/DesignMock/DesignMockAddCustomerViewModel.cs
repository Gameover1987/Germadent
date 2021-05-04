using Germadent.Model;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;

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
                Email = "asdasd@gmail.com",
                WebSite = "http://megasite.ru",
                Description = "Lorem ipsum dolor sit amet"
            };
            Initialize(CardViewMode.Add, customer);
        }
    }

    public class DesignMockAddResponsiblePersonViewModel : AddResponsiblePersonViewModel
    {
        public DesignMockAddResponsiblePersonViewModel()
        {
            var responsiblePersonDto = new ResponsiblePersonDto()
            {
                FullName = "ООО Пошла родимая",
                Phone = "222-333-444",
                Email = "asdasd@gmail.com",
                Position = "Мега доктор",
                Description = "Lorem ipsum dolor sit amet"
            };
            Initialize(CardViewMode.View, responsiblePersonDto);
        }
    }
}