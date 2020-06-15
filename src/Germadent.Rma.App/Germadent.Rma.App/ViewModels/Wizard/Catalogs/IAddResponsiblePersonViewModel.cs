using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public interface IAddResponsiblePersonViewModel
    {
        void Initialize(CardViewMode viewMode, ResponsiblePersonDto responsiblePerson);

        ResponsiblePersonDto GetResponsiblePerson();
    }
}