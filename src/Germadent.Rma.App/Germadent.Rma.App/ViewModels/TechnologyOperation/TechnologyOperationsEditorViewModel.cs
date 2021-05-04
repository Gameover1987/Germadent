using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.Views.TechnologyOperation;
using Germadent.Rma.Model.Production;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.TechnologyOperation
{
    public class TechnologyOperationsEditorViewModel : ViewModelBase, ITechnologyOperationsEditorViewModel
    {
        private readonly IEmployeePositionRepository _employeePositionRepository;
        private readonly ITechnologyOperationRepository _technologyOperationRepository;
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IAddTechnologyOperationViewModel _addTechnologyOperationViewModel;

        private EmployeePositionViewModel _selectedEmployeePosition;
        private TechnologyOperationViewModel _selectedTechnologyOperation;

        private readonly ICollectionView _technologyOperationsView;

        public TechnologyOperationsEditorViewModel(
            IEmployeePositionRepository employeePositionRepository, 
            ITechnologyOperationRepository technologyOperationRepository,
            IShowDialogAgent dialogAgent,
            IAddTechnologyOperationViewModel addTechnologyOperationViewModel)
        {
            _employeePositionRepository = employeePositionRepository;
            _technologyOperationRepository = technologyOperationRepository;
            _technologyOperationRepository.Changed += TechnologyOperationRepositoryOnChanged;
            _dialogAgent = dialogAgent;
            _addTechnologyOperationViewModel = addTechnologyOperationViewModel;

            _technologyOperationsView = CollectionViewSource.GetDefaultView(TechnologyOperations);
            _technologyOperationsView.Filter = TechnologyOperationsFilter;

            AddTechnologyOperationCommand = new DelegateCommand(AddTechnologyOperationCommandHandler, CanAddTechnologyOperationCommandHandler);
            EditTechnologyOperationCommand = new DelegateCommand(EditTechnologyOperationCommandHandler, CanEditTechnologyOperationCommandHandler);
            DeleteTechnologyOperationCommand = new DelegateCommand(DeleteTechnologyOperationCommandHandler, CanDeleteTechnologyOperationCommand);
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

        public IDelegateCommand AddTechnologyOperationCommand { get; }

        public IDelegateCommand EditTechnologyOperationCommand { get; }

        public IDelegateCommand DeleteTechnologyOperationCommand { get; }

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

        private bool CanAddTechnologyOperationCommandHandler()
        {
            return SelectedEmployeePosition != null;
        }

        private void AddTechnologyOperationCommandHandler()
        {
            var newTechnologyOperationDto = new TechnologyOperationDto
            {
                EmployeePositionId = SelectedEmployeePosition.EmployeePositionId
            };
            _addTechnologyOperationViewModel.Initialize(ViewMode.Add, newTechnologyOperationDto);
            if (_dialogAgent.ShowDialog<AddTechnologyOperationWindow>(_addTechnologyOperationViewModel) == false)
                return;

            var technologyOperation = _addTechnologyOperationViewModel.GetTechnologyOperation();
            _technologyOperationRepository.AddTechnologyOperation(technologyOperation);
        }

        private bool CanEditTechnologyOperationCommandHandler()
        {
            return SelectedTechnologyOperation != null;
        }

        private void EditTechnologyOperationCommandHandler()
        {
            _addTechnologyOperationViewModel.Initialize(ViewMode.Edit, SelectedTechnologyOperation.ToDto());
            if (_dialogAgent.ShowDialog<AddTechnologyOperationWindow>(_addTechnologyOperationViewModel) == false)
                return;

            var technologyOperation = _addTechnologyOperationViewModel.GetTechnologyOperation();
            SelectedTechnologyOperation.Update(technologyOperation);

            _technologyOperationRepository.EditTechnologyOperation(technologyOperation);
        }

        private bool CanDeleteTechnologyOperationCommand()
        {
            return SelectedTechnologyOperation != null;
        }

        private void DeleteTechnologyOperationCommandHandler()
        {
            var msg = "Вы действительно хотите удалить технологическую операцию?";
            if (_dialogAgent.ShowMessageDialog(msg, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            _technologyOperationRepository.DeleteTechnologyOperation(SelectedTechnologyOperation.TechnologyOperationId);
            TechnologyOperations.Remove(SelectedTechnologyOperation);
        }

        private bool TechnologyOperationsFilter(object obj)
        {
            if (SelectedEmployeePosition == null)
                return false;

            var technologyOperation = (TechnologyOperationViewModel) obj;
            return technologyOperation.EmployeePositionId == SelectedEmployeePosition.EmployeePositionId;
        }

        private void TechnologyOperationRepositoryOnChanged(object sender, RepositoryChangedEventArgs<TechnologyOperationDto> e)
        {
            var itemsToDelete = TechnologyOperations
                .Where(x => e.DeletedItems.Contains(x.TechnologyOperationId))
                .ToArray();
            foreach (var item in itemsToDelete)
            {
                TechnologyOperations.Remove(item);
            }

            foreach (var technologyOperationDto in e.ChangedItems)
            {
                var technologyOperationViewModel = TechnologyOperations
                    .FirstOrDefault(x => x.TechnologyOperationId == technologyOperationDto.TechnologyOperationId);

                technologyOperationViewModel?.Update(technologyOperationDto);
            }

            foreach (var addedItem in e.AddedItems)
            {
                TechnologyOperations.Add(new TechnologyOperationViewModel(addedItem));
            }
        }
    }
}