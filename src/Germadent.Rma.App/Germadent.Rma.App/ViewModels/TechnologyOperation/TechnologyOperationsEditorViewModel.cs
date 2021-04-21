using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.TechnologyOperation
{
    public class TechnologyOperationsEditorViewModel : ViewModelBase, ITechnologyOperationsEditorViewModel
    {
        private readonly IEmployeePositionRepository _employeePositionRepository;
        private readonly ITechnologyOperationRepository _technologyOperationRepository;

        private EmployeePositionViewModel _selectedEmployeePosition;
        private TechnologyOperationViewModel _selectedTechnologyOperation;

        private readonly ICollectionView _technologyOperationsView;

        public TechnologyOperationsEditorViewModel(IEmployeePositionRepository employeePositionRepository, ITechnologyOperationRepository technologyOperationRepository)
        {
            _employeePositionRepository = employeePositionRepository;
            _technologyOperationRepository = technologyOperationRepository;

            _technologyOperationsView = CollectionViewSource.GetDefaultView(TechnologyOperations);
            _technologyOperationsView.Filter = TechnologyOperationsFilter;
        }

        public ObservableCollection<EmployeePositionViewModel> EmployeePositions { get; } = new ObservableCollection<EmployeePositionViewModel>();

        public EmployeePositionViewModel SelectedEmployeePosition
        {
            get { return _selectedEmployeePosition; }
            set
            {
                if (_selectedEmployeePosition == value)
                    return;
                _selectedEmployeePosition = value;
                OnPropertyChanged(() => SelectedEmployeePosition);

                _technologyOperationsView.Refresh();
            }
        }

        public ObservableCollection<TechnologyOperationViewModel> TechnologyOperations { get; } = new ObservableCollection<TechnologyOperationViewModel>();

        public TechnologyOperationViewModel SelectedTechnologyOperation
        {
            get { return _selectedTechnologyOperation; }
            set
            {
                if (_selectedTechnologyOperation == value)
                    return;
                _selectedTechnologyOperation = value;
                OnPropertyChanged(() => SelectedTechnologyOperation);
            }
        }

        public void Initialize()
        {
            EmployeePositions.Clear();
            foreach (var employeePositionDto in _employeePositionRepository.Items)
            {
                EmployeePositions.Add(new EmployeePositionViewModel(employeePositionDto));
            }

            SelectedEmployeePosition = EmployeePositions.FirstOrDefault();

            TechnologyOperations.Clear();
            foreach (var technologyOperationDto in _technologyOperationRepository.Items)
            {
                TechnologyOperations.Add(new TechnologyOperationViewModel(technologyOperationDto));
            }
        }

        private bool TechnologyOperationsFilter(object obj)
        {
            if (SelectedEmployeePosition == null)
                return false;

            var technologyOperation = (TechnologyOperationViewModel) obj;
            return technologyOperation.EmployeePositionId == SelectedEmployeePosition.EmployeePositionId;
        }
    }
}