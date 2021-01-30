using System;

namespace Germadent.Rma.App.ViewModels.ToothCard
{
    public class ToothCleanUpEventArgs : EventArgs
    {
        public ToothCleanUpEventArgs(ToothViewModel tooth)
        {
            Tooth = tooth;
        }

        public ToothViewModel Tooth { get; }
    }
}