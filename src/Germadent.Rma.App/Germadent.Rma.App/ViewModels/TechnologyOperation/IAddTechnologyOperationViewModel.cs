using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Rma.Model.Production;

namespace Germadent.Rma.App.ViewModels.TechnologyOperation
{
    public enum ViewMode
    {
        Add,
        Edit
    }


    public interface IAddTechnologyOperationViewModel
    {
        void Initialize(ViewMode viewMode, TechnologyOperationDto technologyOperationDto);

        TechnologyOperationDto GetTechnologyOperation();
    }
}
