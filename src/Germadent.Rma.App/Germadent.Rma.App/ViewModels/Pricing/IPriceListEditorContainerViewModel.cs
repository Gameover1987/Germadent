using System;
using Germadent.Model;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public interface IPriceListEditorContainerViewModel
    {
        void Initialize();
    }

    public class PriceListEditorContainerViewModel : ViewModelBase, IPriceListEditorContainerViewModel
    {
        private bool _isBusy;
        private string _busyReason;

        public PriceListEditorContainerViewModel(IPriceListEditorFactory editorFactory)
        {
            MillingCenterPriceListEditor = editorFactory.CreateEditor(BranchType.MillingCenter);
            MillingCenterPriceListEditor.IsBusyChanged += PriceListEditorOnIsBusyChanged;

            LaboratoryPriceListEditor = editorFactory.CreateEditor(BranchType.Laboratory);
            LaboratoryPriceListEditor.IsBusyChanged += PriceListEditorOnIsBusyChanged;
        }

        public IPriceListEditorViewModel MillingCenterPriceListEditor { get; }

        public IPriceListEditorViewModel LaboratoryPriceListEditor { get; }

        public bool IsBusy
        {
            get { return _isBusy; }
            private set
            {
                if (_isBusy == value)
                    return;

                _isBusy = value;
                OnPropertyChanged(() => IsBusy);
            }
        }

        public string BusyReason
        {
            get { return _busyReason; }
            private set
            {
                if (_busyReason == value)
                    return;
                _busyReason = value;
                OnPropertyChanged(() => BusyReason);

                IsBusy = value != null;
            }
        }

        public void Initialize()
        {
            MillingCenterPriceListEditor.Initialize();
            LaboratoryPriceListEditor.Initialize();
        }

        private void PriceListEditorOnIsBusyChanged(object sender, EventArgs e)
        {
            var editor = (IPriceListEditorViewModel) sender;
            BusyReason = editor.BusyReason;

            DelegateCommand.NotifyCanExecuteChangedForAll();
        }
    }
}
