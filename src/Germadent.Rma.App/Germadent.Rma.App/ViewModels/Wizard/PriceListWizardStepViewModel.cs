using System;
using System.Collections.Generic;
using System.Text;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class PriceListWizardStepViewModel : WizardStepViewModelBase, IToothCardContainer
    {
        public PriceListWizardStepViewModel(IToothCardViewModel toothCard)
        {
            ToothCard = toothCard;
        }

        public override string DisplayName
        {
            get { return "Детализация работ"; }
        }

        public override bool IsValid { get; }

        public IToothCardViewModel ToothCard { get; }

        public override void Initialize(OrderDto order)
        {
            ToothCard.Initialize(order.ToothCard);
        }

        public override void AssemblyOrder(OrderDto order)
        {
            
        }
    }
}
