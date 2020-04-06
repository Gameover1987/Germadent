using System;
using Germadent.Rma.App.ViewModels.Wizard;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockMillingCenterInfoWizardStepViewModel : MillingCenterInfoWizardStepViewModel
    {
        public DesignMockMillingCenterInfoWizardStepViewModel() : base(new DesignMockWindowManager(), new DesignMockSuggestionProvider())
        {
            Customer = "Заказчик Заказчиков";
            Patient = "Пациент Пациентов";
            TechnicFullName = "Техник Техникович";
            TechnicPhone = "+7913 453 45 38";
            DateComment = "Какой то комментарий к срокам";
            Created = DateTime.Now;
        }
    }
}