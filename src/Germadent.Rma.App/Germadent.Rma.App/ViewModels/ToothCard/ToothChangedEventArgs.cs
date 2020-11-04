namespace Germadent.Rma.App.ViewModels.ToothCard
{
    public class ToothChangedEventArgs
    {
        public ToothChangedEventArgs(bool affectsRenderToothCard)
        {
            AffectsRenderToothCard = affectsRenderToothCard;
        }
        public bool AffectsRenderToothCard { get; }
    }
}