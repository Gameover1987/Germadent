using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Model.Production;

namespace Germadent.Rma.App.ViewModels.TechnologyOperation
{
    public interface IAddRateViewModel
    {
        void Initialize(ViewMode viewMode, RateDto rateDto);

        RateDto GetRate();
    }
}
