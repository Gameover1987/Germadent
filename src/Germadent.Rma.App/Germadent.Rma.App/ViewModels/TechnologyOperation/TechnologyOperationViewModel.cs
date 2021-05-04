using Germadent.Model.Production;
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

        public bool IsObsolete => _technologyOperationDto.IsObsolete;

        public void Update(TechnologyOperationDto technologyOperationDto)
        {
            _technologyOperationDto = technologyOperationDto;
            OnPropertyChanged();
        }

        public TechnologyOperationDto ToDto()
        {
            return _technologyOperationDto;
        }
    }
}