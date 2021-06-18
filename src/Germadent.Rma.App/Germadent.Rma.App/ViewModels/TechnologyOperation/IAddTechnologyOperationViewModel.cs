using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Model.Production;
using Germadent.UI.Commands;

namespace Germadent.Rma.App.ViewModels.TechnologyOperation
{
    public enum ViewMode
    {
        Add,
        Edit
    }


    public interface IAddTechnologyOperationViewModel
    {
        IDelegateCommand EditRateCommand { get; }

        void Initialize(ViewMode viewMode, TechnologyOperationDto technologyOperationDto);

        TechnologyOperationDto GetTechnologyOperation();
    }
}
