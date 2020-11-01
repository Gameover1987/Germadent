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

            PriceList.Initialize(order.BranchType);
            PriceList.PricePositionChecked += PriceListOnPricePositionChecked;
        }

        private void PriceListOnPricePositionChecked(object sender, PricePositionCheckedEventArgs e)
        {
            if (ToothCard.SelectedTeeth == null)
                return;


        }

        public override void AssemblyOrder(OrderDto order)
        {
            
        }
    }
}