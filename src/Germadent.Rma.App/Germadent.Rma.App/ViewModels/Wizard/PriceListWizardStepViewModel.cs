using System;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Rma.App.ViewModels.Pricing;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class PriceListWizardStepViewModel : WizardStepViewModelBase, IToothCardContainer
    {
        public PriceListWizardStepViewModel(IToothCardViewModel toothCard, IPriceListViewModel priceList)
        {
            ToothCard = toothCard;

            PriceList = priceList;
        }

        public override string DisplayName
        {
            get { return "Детализация работ"; }
        }

        public override bool IsValid
        {
            get { return ToothCard.IsValid; }
        }

        public IToothCardViewModel ToothCard { get; }

        public IPriceListViewModel PriceList { get; }

        public override void Initialize(OrderDto order)
        {
            ToothCard.Initialize(order.ToothCard);
            ToothCard.ToothSelected += ToothCard_ToothSelected;
            ToothCard.ToothChanged += ToothCardOnToothChanged;

            PriceList.Initialize(order.BranchType);
            PriceList.PricePositionsChecked += PriceList_PricePositionChecked;
        }

        private void ToothCardOnToothChanged(object sender, ToothChangedEventArgs e)
        {
            if (ToothCard.SelectedTeeth == null || ToothCard.SelectedTeeth.Length == 0)
                return;

            PriceList.Setup(ToothCard.SelectedTeeth.First().ToDto());
        }

        private void ToothCard_ToothSelected(object sender, ToothSelectedEventArgs e)
        {
            PriceList.Setup(e.SelectedTooth?.ToDto());
        }

        private void PriceList_PricePositionChecked(object sender, EventArgs e)
        {
            ToothCard.AttachPricePositions(PriceList.Positions.Where(x => x.IsChecked).ToArray());
        }

        public override void AssemblyOrder(OrderDto order)
        {
            order.ToothCard = ToothCard.Teeth.Where(x => x.IsChanged).Select(x => x.ToDto()).ToArray();
            order.ToothCard.ForEach(x => x.WorkOrderId = order.WorkOrderId);
        }
    }
}