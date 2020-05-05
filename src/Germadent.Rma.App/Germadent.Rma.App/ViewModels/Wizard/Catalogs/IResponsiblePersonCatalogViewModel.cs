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
        string DisplayName { get; set; }
    }
}