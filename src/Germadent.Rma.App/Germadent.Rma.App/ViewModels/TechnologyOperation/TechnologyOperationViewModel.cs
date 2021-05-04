using Germadent.Rma.Model.Production;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.TechnologyOperation
{
    public class TechnologyOperationViewModel : ViewModelBase
    {
        private TechnologyOperationDto _technologyOperationDto;

        public TechnologyOperationViewModel(TechnologyOperationDto technologyOperationDto)
        {
            _technologyOperationDto = technologyOperationDto;
        }

        public int EmployeePositionId => _technologyOperationDto.EmployeePositionId;

        public int TechnologyOperationId => _technologyOperationDto.TechnologyOperationId;

        public string UserCode => _technologyOperationDto.UserCode;

        public string DisplayName => _technologyOperationDto.Name;

        public decimal Rate => _technologyOperationDto.Rate;

        public void Update(TechnologyOperationDto technologyOperationDto)
        {
            _technologyOperationDto = technologyOperationDto;
            OnPropertyChanged();
        }
    }
}