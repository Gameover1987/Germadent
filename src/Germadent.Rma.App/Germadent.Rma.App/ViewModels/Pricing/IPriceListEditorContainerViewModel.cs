using System;
using System.Collections.Generic;
using System.Text;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public interface IPriceListEditorContainerViewModel
    {
        void Initialize();
    }

    public class PriceListEditorContainerViewModel : IPriceListEditorContainerViewModel
    {
        public PriceListEditorContainerViewModel(IPriceListEditorFactory editorFactory)
        {
            MillingCenterPriceListEditor = editorFactory.CreateEditor(BranchType.MillingCenter);
            LaboratoryPriceListEditor = editorFactory.CreateEditor(BranchType.Laboratory);
        }

        public IPriceListEditorViewModel MillingCenterPriceListEditor { get; }

        public IPriceListEditorViewModel LaboratoryPriceListEditor { get; }

        public void Initialize()
        {
            MillingCenterPriceListEditor.Initialize();
            LaboratoryPriceListEditor.Initialize();
        }
    }
}
