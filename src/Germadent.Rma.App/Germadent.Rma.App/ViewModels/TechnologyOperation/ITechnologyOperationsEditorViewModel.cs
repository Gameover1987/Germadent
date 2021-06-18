using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.UI.Commands;

namespace Germadent.Rma.App.ViewModels.TechnologyOperation
{
    public interface ITechnologyOperationsEditorViewModel
    {
        IDelegateCommand EditTechnologyOperationCommand { get; }

        void Initialize();
    }
}
