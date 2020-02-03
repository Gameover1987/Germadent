using System.Collections.ObjectModel;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class LaboratoryProjectWizardStepViewModel : WizardStepViewModelBase
    {
        private string _workDescription;
        private string _colorAndFeatures;
        private int _transparency;

        public LaboratoryProjectWizardStepViewModel(IToothCardViewModel toothCard)
        {
            ToothCard = toothCard;
        }

        public override string DisplayName
        {
            get { return "Проект"; }
        }

        public override bool IsValid => !HasErrors;

        public string WorkDescription
        {
            get { return _workDescription; }
            set { SetProperty(() => _workDescription, value); }
        }

        public string ColorAndFeatures
        {
            get { return _colorAndFeatures; }
            set { SetProperty(() => _colorAndFeatures, value); }
        }

        public int Transparency
        {
            get { return _transparency; }
            set { SetProperty(() => _transparency, value); }
        }

        public IToothCardViewModel ToothCard { get; } 

        public override void Initialize(OrderDto order)
        {
            _workDescription = order.WorkDescription;
            _colorAndFeatures = order.ColorAndFeatures;
            _transparency = order.Transparency;

            ToothCard.Initialize(order.ToothCard);

            OnPropertyChanged();
        }

        public override void AssemblyOrder(OrderDto order)
        {
            order.WorkDescription = WorkDescription;
            order.ColorAndFeatures = ColorAndFeatures;
            order.Transparency = Transparency;

            order.ToothCard = ToothCard.ToModel();
            order.ToothCard.ForEach(x => x.WorkOrderId = order.WorkOrderId);
        }
    }
}