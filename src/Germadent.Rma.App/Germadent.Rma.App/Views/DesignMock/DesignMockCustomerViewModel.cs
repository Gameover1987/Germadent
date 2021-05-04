using Germadent.Model;
using Germadent.Rma.App.ViewModels;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockCustomerViewModel : CustomerViewModel
    {
        public DesignMockCustomerViewModel() : base(new CustomerDto
        {
            Name = "ООО Пошла родимая",
            Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
            Phone = "+7913-999-44-66",
            WebSite = "http://xn----8sbbcdtrifnipjk4bzlpa.xn--p1ai/nashi-ob-ekty/26-ooo-poshla-rodimaya"
        })
        {
        }
    }

    public class DesignMockResponsiblePersonViewModel : ResponsiblePersonViewModel
    {
        public DesignMockResponsiblePersonViewModel() : base(new ResponsiblePersonDto
        {
            FullName = "Профессор Преображенский",
            Description = "Очень крутой чувак!",
            Email = "prof.preob@gmail.com",
            Phone = "222-33-4444",
            Position = "Доктор"
        })
        {
        }
    }
}
