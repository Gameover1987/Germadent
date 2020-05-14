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

    public class DesignMockResponsiblePersonRepository : IResponsiblePersonRepository
    {
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<EventArgs> Changed;

        public ResponsiblePersonDto[] Items { get; }
    }

    public class DesignMockDictionaryRepository :IDictionaryRepository
    {
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<EventArgs> Changed;
        public DictionaryItemDto[] Items { get; }
        public DictionaryItemDto[] GetItems(DictionaryType dictionary)
        {
            throw new NotImplementedException();
        }
    }

    public class DesignMockMillingCenterInfoWizardStepViewModel : MillingCenterInfoWizardStepViewModel
    {
        public DesignMockMillingCenterInfoWizardStepViewModel() 
            : base(new DesignMockCatalogSelectionOperations(), new DesignMockCatalogUIOperations(),  new DesignMockCustomerSuggestionProvider(), new DesignMockResponsiblePersonsSuggestionProvider(),  new DesignMockCustomerRepository(), new DesignMockResponsiblePersonRepository())
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