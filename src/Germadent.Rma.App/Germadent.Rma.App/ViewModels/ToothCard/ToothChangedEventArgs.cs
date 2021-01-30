using System;

namespace Germadent.Rma.App.ViewModels.ToothCard
{
    public class ToothChangedEventArgs : EventArgs
    {
        public ToothChangedEventArgs(bool affectsRenderToothCard)
        {
            AffectsRenderToothCard = affectsRenderToothCard;
        }
        public bool AffectsRenderToothCard { get; }
    }
}