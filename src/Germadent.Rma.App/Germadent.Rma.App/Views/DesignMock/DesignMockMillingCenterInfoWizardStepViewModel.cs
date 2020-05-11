using System;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels.Wizard;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockCustomerRepository : ICustomerRepository
    {
        public CustomerDto[] Items { get; }

        public void Initialize()
        {
            
        }

        public void ReLoad()
        {
            throw new NotImplementedException();
        }
        
        public event EventHandler<EventArgs> Changed;
    }

    public class DesignMockMillingCenterInfoWizardStepViewModel : MillingCenterInfoWizardStepViewModel
    {
        public DesignMockMillingCenterInfoWizardStepViewModel() : base(new DesignMockCatalogUIOperations(), new DesignMockSuggestionProvider(), new DesignMockCustomerRepository())
        { 
            Customer = "Заказчик Заказчиков";
            Patient = "Пациент Пациентов";
            ResponsiblePerson = "Техник Техникович";
            ResponsiblePersonPhone = "+7913 453 45 38";
            DateComment = "Какой то комментарий к срокам";
            Created = DateTime.Now;
        }
    }
}