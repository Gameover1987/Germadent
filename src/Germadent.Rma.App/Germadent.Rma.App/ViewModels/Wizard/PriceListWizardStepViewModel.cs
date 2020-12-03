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
            ToothCard.ToothCleanup += ToothCardOnToothCleanup;

            PriceList.Initialize(order.BranchType);
            PriceList.ProductChecked += PriceList_ProductChecked;
        }

        private void ToothCardOnToothCleanup(object sender, ToothCleanUpEventArgs e)
        {
            PriceList.Setup(e.Tooth.ToDto());
        }

        private void ToothCard_ToothSelected(object sender, ToothSelectedEventArgs e)
        {
            PriceList.Setup(e.SelectedTooth?.ToDto());
        }

        private void PriceList_ProductChecked(object sender, EventArgs e)
        {
            ToothCard.AttachPricePositions(PriceList.Products.Where(x => x.IsChecked).ToArray());
        }

        public override void AssemblyOrder(OrderDto order)
        {
            order.ToothCard = ToothCard.Teeth.Where(x => x.IsChanged).Select(x => x.ToDto()).ToArray();
            order.ToothCard.ForEach(x => x.WorkOrderId = order.WorkOrderId);
        }
    }
}