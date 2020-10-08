using System;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels.Wizard;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockCustomerRepository : ICustomerRepository
    {
        public CustomerDto[] Items
        {
            get
            {
                return new CustomerDto[]
                {
                    new CustomerDto
                    {
                        Name = "Vasya",
                        Description = "Lorem ipsum dolat sit amet",
                        Email = "asd@asd.com", Id = 1,
                        Phone = "222-333",
                        WebSite = "yandex.ru"
                    },
                    new CustomerDto
                    {
                        Name = "Petya",
                        Description = "Lorem ipsum dolat sit amet",
                        Email = "asd@asd.com", Id = 1,
                        Phone = "222-333",
                        WebSite = "yandex.ru"
                    },
                };
            }
        }

        public void Initialize()
        {

        }

        public void ReLoad()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<EventArgs> Changed;
    }

    public class DesignMockDictionaryRepository : IDictionaryRepository
    {
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<EventArgs> Changed;

        public DictionaryItemDto[] Items { get; }

        public DictionaryItemDto[] GetItems(DictionaryType dictionary)
        {
            switch (dictionary)
            {
                case DictionaryType.Transparency:
                    return GetTransparency();
                case DictionaryType.ProstheticCondition:
                    return GetProstheticConditions();
                case DictionaryType.ProstheticType:
                    return GetProstheticTypes();
                case DictionaryType.Material:
                    return GetMaterials();
                default:
                    throw new NotImplementedException("GetItems");
            }
        }

        private DictionaryItemDto[] GetTransparency()
        {
            return new DictionaryItemDto[]
            {
                new DictionaryItemDto {Id = 0, Name = "Мамелоны"},
                new DictionaryItemDto {Id = 1, Name = "Вторичный дентин"},
                new DictionaryItemDto {Id = 2, Name = "Зубы с сильно выраженной прозрачностью "},
                new DictionaryItemDto {Id = 3, Name = "Зубы со слабоо выраженной прозрачностью "},
            };
        }

        private DictionaryItemDto[] GetProstheticConditions()
        {
            var ptostheticsConditions = new[]
            {
                new DictionaryItemDto{Name = "Культя", Id = 1},
                new DictionaryItemDto{Name = "Имплант", Id = 2},
            };

            return ptostheticsConditions;
        }

        private DictionaryItemDto[] GetProstheticTypes()
        {
            return new DictionaryItemDto[]
            {
                new DictionaryItemDto {Name = "Каркас", Id = 1},
                new DictionaryItemDto {Name = "Каркас винт. фикс", Id = 2},
                new DictionaryItemDto {Name = "Абатмент", Id = 3},
                new DictionaryItemDto {Name = "Полная анатомия", Id = 4},
                new DictionaryItemDto {Name = "Временная конструкция", Id = 5},
                new DictionaryItemDto {Name = "Другая конструкция", Id = 6},
            };
        }

        private DictionaryItemDto[] GetMaterials()
        {

            var materials = new[]
            {
                new DictionaryItemDto {Name = "ZrO", Id = 1},
                new DictionaryItemDto {Name = "PMMA mono", Id = 2},
                new DictionaryItemDto {Name = "PMMA multi", Id = 3},
                new DictionaryItemDto {Name = "WAX", Id = 4},
                new DictionaryItemDto {Name = "MIK", Id = 5},
                new DictionaryItemDto {Name = "CAD-Temp mono", Id = 6},
                new DictionaryItemDto {Name = "CAD-Temp multi", Id = 7},
                new DictionaryItemDto {Name = "Enamik mono", Id = 8},
                new DictionaryItemDto {Name = "Enamik multi", Id = 9},
                new DictionaryItemDto {Name = "SUPRINITY", Id = 10},
                new DictionaryItemDto {Name = "Mark II", Id = 11},
                new DictionaryItemDto {Name = "WAX", Id = 12},
                new DictionaryItemDto {Name = "TriLuxe forte", Id = 13},
                new DictionaryItemDto {Name = "Ti", Id = 14},
                new DictionaryItemDto {Name = "E.MAX", Id = 15},
            };

            return materials;
        }
    }

    public class DesignMockMillingCenterInfoWizardStepViewModel : MillingCenterInfoWizardStepViewModel
    {
        public DesignMockMillingCenterInfoWizardStepViewModel()
            : base(new DesignMockCatalogSelectionOperations(), new DesignMockCatalogUIOperations(), new DesignMockCustomerSuggestionProvider(), new DesignMockResponsiblePersonsSuggestionProvider(), new DesignMockCustomerRepository(), new DesignMockResponsiblePersonRepository())
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