using System;
using Germadent.Rma.App.ViewModels.Wizard;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockLaboratoryInfoWizardStepViewModel : LaboratoryInfoWizardStepViewModel
    {
        public DesignMockLaboratoryInfoWizardStepViewModel() 
            : base(new DesignMockCatalogSelectionOperations(), new DesignMockCatalogUIOperations(), new DesignMockCustomerSuggestionProvider(), new DesignMockResponsiblePersonsSuggestionProvider(), new DesignMockCustomerRepository(), new DesignMockResponsiblePersonRepository())
        {
            Customer = "Какой то заказчик";
            ResponsiblePerson = "Какой то доктор";
            PatientFio = "Какой то пациент";

            Gender = Gender.Female;
            Age = 22;

            Created = DateTime.Now;
            FittingDate = DateTime.Now;
            DateOfCompletion = DateTime.Now;

            DateComment = "Какой то комментарий к срокам выполнения";
        }
    }
}