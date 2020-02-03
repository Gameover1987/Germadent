using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.ViewModels.Wizard;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockLaboratoryProjectWizardStepViewModel : LaboratoryProjectWizardStepViewModel
    {
        public DesignMockLaboratoryProjectWizardStepViewModel()
            : base(new ToothCardViewModel(new MockRmaOperations()))
        {
            var order = new OrderDto()
            {
                WorkDescription =
                    "Очень много разной интересной работы, делать ее родиму не переделать. Но мы же крутые спецы, сделаем! Очень много разной интересной работы, делать ее родиму не переделать. Но мы же крутые спецы, сделаем Но мы же крутые спецы, сделаем! Очень много разной интересной работы, делать ее родиму не переделать. Но мы же крутые спецы, сделаем!",
                ColorAndFeatures = "Белоснежный",
            };
            Initialize(order);
        }
    }
}