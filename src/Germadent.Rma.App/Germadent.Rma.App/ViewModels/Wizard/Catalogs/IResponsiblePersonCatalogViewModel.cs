using Germadent.Rma.Model;
using Germadent.UI.Commands;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public interface IResponsiblePersonCatalogViewModel
    {
        string SearchString { get; set; }

        IResponsiblePersonViewModel SelectedResponsiblePerson { get; }

        IDelegateCommand AddResponsiblePersonCommand { get; }

        void Initialize();
    }

    public interface IResponsiblePersonViewModel
    {
        string FullName { get; }

        string Position { get; }

        string Phone { get; }

        string Email { get; }

        string Description { get; }

        ResponsiblePersonDto ToDto();
    }
}