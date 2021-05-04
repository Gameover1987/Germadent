using Germadent.Client.Common.Infrastructure;
using Germadent.Model;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.Reporting;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.App.ViewModels.Wizard;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesingMockMillingCenterProjectWizardStepViewModel : MillingCenterProjectWizardStepViewModel
    {
        public DesingMockMillingCenterProjectWizardStepViewModel() 
            : base(new ToothCardViewModel(new DesignMockDictionaryRepository(), new DesignMockProductRepository(),  new ClipboardHelper()))
        {
            Initialize(new OrderDto{WorkAccepted = true});
        }
    }
}