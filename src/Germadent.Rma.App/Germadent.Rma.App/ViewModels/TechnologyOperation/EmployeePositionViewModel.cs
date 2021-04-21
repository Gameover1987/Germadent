using Germadent.Rma.Model.Production;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.TechnologyOperation
{
    public class EmployeePositionViewModel : ViewModelBase
    {
        private readonly EmployeePositionDto _employeePositionDto;

        public EmployeePositionViewModel(EmployeePositionDto employeePositionDto)
        {
            _employeePositionDto = employeePositionDto;
        }

        public int EmployeePositionId => _employeePositionDto.EmployeePositionId;

        public string DisplayName => _employeePositionDto.Name;
    }
}