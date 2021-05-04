using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Germadent.Common;
using Germadent.Common.Extensions;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels.Pricing;
using Germadent.Rma.App.Views.TechnologyOperation;
using Germadent.Rma.Model.Pricing;
using Germadent.Rma.Model.Production;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels.Validation;

namespace Germadent.Rma.App.ViewModels.TechnologyOperation
{
    public enum UserCodeValidationState
    {
        NonValidated,
        Valid,
        Invalid
    }

    public class AddTechnologyOperationViewModel : ValidationSupportableViewModel, IAddTechnologyOperationViewModel
    {
        private readonly IEmployeePositionRepository _employeePositionRepository;
        private readonly ITechnologyOperationRepository _technologyOperationRepository;
        private readonly IPricePositionRepository _pricePositionRepository;
        private readonly IAddRateViewModel _addRateViewModel;
        private readonly IShowDialogAgent _dialogAgent;

        private ViewMode _viewMode;

        private int _technologyOperationId;
        private string _name;
        private EmployeePositionViewModel _selectedEmployeePosition;
        private string _userCode;

        private string[] _operationNames;
        private string _userCodeValidationMessage;
        private UserCodeValidationState _userCodeValidationState;
        private string _pricePositionSearchString;

        private readonly ICollectionView _pricePositionsView;
        private RateViewModel _selectedRate;
        private bool _isObsolete;

        public AddTechnologyOperationViewModel(IEmployeePositionRepository employeePositionRepository,
            ITechnologyOperationRepository technologyOperationRepository,
            IPricePositionRepository pricePositionRepository,
            IAddRateViewModel addRateViewModel,
            IShowDialogAgent dialogAgent)
        {
            _employeePositionRepository = employeePositionRepository;
            _technologyOperationRepository = technologyOperationRepository;
            _technologyOperationRepository.Changed += TechnologyOperationRepositoryOnChanged;

            _pricePositionRepository = pricePositionRepository;
            _addRateViewModel = addRateViewModel;
            _dialogAgent = dialogAgent;
            _pricePositionRepository.Changed += PricePositionRepositoryOnChanged;

            AddValidationFor(() => Name)
                .When(() => Name.IsNullOrWhiteSpace(), () => "Укажите название технологической операции");
            AddValidationFor(() => Name)
                .When(() =>
                {
                    if (Name.IsNullOrWhiteSpace())
                        return false;
                    return _operationNames.Contains(Name.ToLower());
                }, () => "Укажите уникальное название технологической операции");

            _pricePositionsView = CollectionViewSource.GetDefaultView(PricePositions);
            _pricePositionsView.Filter = PricePositionsFilter;

            OkCommand = new DelegateCommand(CanOkCommandHandler);
            AttachPricePositionCommand = new DelegateCommand(AttachPricePositionCommandHandler, CanAttachPricePositionCommandHandler);
            AddRateCommand = new DelegateCommand(AddRateCommandHandler);
            EditRateCommand = new DelegateCommand(EditRateCommandHandler, CanEditRateCommandHandler);
            DeleteRateCommand = new DelegateCommand(DeleteRateCommandHandler, CanDeleteRateCommandHandler);
        }

        private bool PricePositionsFilter(object obj)
        {
            if (PricePositionSearchString.IsNullOrWhiteSpace())
                return true;

            var pricePositionViewModel = (PricePositionViewModel) obj;
            return pricePositionViewModel.UserCode.ToLower().Contains(PricePositionSearchString);
        }

        public string Title
        {
            get
            {
                if (_viewMode == ViewMode.Add)
                    return "Добавление технологической операции";

                return "Редактирование данных технологической операции";
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;
                _name = value;
                OnPropertyChanged(() => Name);
            }
        }

        public bool IsObsolete
        {
            get { return _isObsolete; }
            set
            {
                if (_isObsolete == value)
                    return;
                _isObsolete = value;
                OnPropertyChanged(() => IsObsolete);
            }
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
            }
        }

        public string UserCode
        {
            get { return _userCode; }
            set
            {
                if (_userCode == value)
                    return;
                _userCode = value;
                OnPropertyChanged(() => UserCode);

                PricePositionSearchString = value;

                ValidateUserCode();
            }
        }

        public UserCodeValidationState UserCodeValidationState
        {
            get { return _userCodeValidationState; }
            set
            {
                if (_userCodeValidationState == value)
                    return;
                _userCodeValidationState = value;
                OnPropertyChanged(() => UserCodeValidationState);
            }
        }

        public string UserCodeValidationMessage
        {
            get { return _userCodeValidationMessage; }
            set
            {
                if (_userCodeValidationMessage == value)
                    return;
                _userCodeValidationMessage = value;
                OnPropertyChanged(() => UserCodeValidationMessage);
            }
        }

        public ObservableCollection<PricePositionViewModel> PricePositions { get; } = new ObservableCollection<PricePositionViewModel>();

        public string PricePositionSearchString
        {
            get { return _pricePositionSearchString; }
            set
            {
                if (_pricePositionSearchString == value)
                    return;
                _pricePositionSearchString = value;
                OnPropertyChanged(() => PricePositionSearchString);

                _pricePositionsView.Refresh();
            }
        }


        public ObservableCollection<RateViewModel> Rates { get; } = new ObservableCollection<RateViewModel>();

        public RateViewModel SelectedRate
        {
            get { return _selectedRate; }
            set
            {
                if (_selectedRate == value)
                    return;
                _selectedRate = value;
                OnPropertyChanged(() => SelectedRate);
            }
        }

        public IDelegateCommand OkCommand { get; }

        public IDelegateCommand AttachPricePositionCommand { get; }

        public IDelegateCommand AddRateCommand { get; }

        public IDelegateCommand EditRateCommand { get; }

        public IDelegateCommand DeleteRateCommand { get; }

        public void Initialize(ViewMode viewMode, TechnologyOperationDto technologyOperationDto)
        {
            ResetValidation();

            _viewMode = viewMode;
            _userCodeValidationState = UserCodeValidationState.NonValidated;

            _technologyOperationId = technologyOperationDto.TechnologyOperationId;

            _name = technologyOperationDto.Name;
            _userCode = technologyOperationDto.UserCode;
            _isObsolete = technologyOperationDto.IsObsolete;

            _operationNames = GetOperationNames();

            EmployeePositions.Clear();
            foreach (var employeePositionDto in _employeePositionRepository.Items)
            {
                EmployeePositions.Add(new EmployeePositionViewModel(employeePositionDto));
            }
            SelectedEmployeePosition = EmployeePositions.FirstOrDefault(x => x.EmployeePositionId == technologyOperationDto.EmployeePositionId);

            PricePositions.Clear();
            foreach (var pricePositionDto in _pricePositionRepository.Items)
            {
                PricePositions.Add(new PricePositionViewModel(pricePositionDto, new DateTimeProvider()));
            }

            Rates.Clear();
            foreach (var rateDto in technologyOperationDto.Rates)
            {
                Rates.Add(new RateViewModel(rateDto));
            }
        }

        public TechnologyOperationDto GetTechnologyOperation()
        {
            return new TechnologyOperationDto
            {
                TechnologyOperationId = _technologyOperationId,
                EmployeePositionId = SelectedEmployeePosition.EmployeePositionId,
                Name = Name,
                Rates = Rates.Select(x => x.ToDto()).ToArray(),
                UserCode = UserCode,
                IsObsolete = IsObsolete
            };
        }

        private string[] GetOperationNames()
        {
            return _technologyOperationRepository.Items.Select(x => x.Name.ToLower()).ToArray();
        }

        private void ValidateUserCode()
        {
            UserCodeValidationMessage = null;
            UserCodeValidationState = UserCodeValidationState.NonValidated;

            if (UserCode.IsNullOrWhiteSpace())
            {
                UserCodeValidationState = UserCodeValidationState.Valid;
                return;
            }

            var pricePositionByUserCode = _pricePositionRepository.Items.FirstOrDefault(x => x.UserCode.ToLower() == UserCode.ToLower());
            if (pricePositionByUserCode == null)
            {
                UserCodeValidationMessage = "Не найдена ценовая позиция";
                UserCodeValidationState = UserCodeValidationState.Invalid;
            }
            else
            {
                UserCodeValidationMessage = pricePositionByUserCode.Name;
                UserCodeValidationState = UserCodeValidationState.Valid;
            }
        }

        private bool CanOkCommandHandler()
        {
            if (HasErrors)
                return false;

            if (Name.IsNullOrWhiteSpace())
                return false;

            if (UserCodeValidationState == UserCodeValidationState.Invalid)
                return false;

            if (SelectedEmployeePosition == null)
                return false;

            if (Rates.Count == 0)
                return false;

            return true;
        }

        private bool CanAttachPricePositionCommandHandler(object obj)
        {
            var pricePosition = (PricePositionViewModel)obj;

            if (UserCode.IsNullOrWhiteSpace())
                return true;

            return UserCode.ToLower() != pricePosition.UserCode.ToLower();
        }

        private void AttachPricePositionCommandHandler(object obj)
        {
            var pricePosition = (PricePositionViewModel) obj;
            _userCode = pricePosition.UserCode;
            ValidateUserCode();

            OnPropertyChanged(() => UserCode);
        }

        private void AddRateCommandHandler()
        {
            var rateDto = new RateDto
            {
                TechnologyOperationId = _technologyOperationId,
                DateBeginning = DateTime.Now,
                QualifyingRank = 1,
            };
            _addRateViewModel.Initialize(ViewMode.Add, rateDto);
            if (_dialogAgent.ShowDialog<AddRateWindow>(_addRateViewModel) == false)
                return;

            var rate = _addRateViewModel.GetRate();
            Rates.Add(new RateViewModel(rate));
        }

        private bool CanEditRateCommandHandler()
        {
            return SelectedRate != null;
        }

        private void EditRateCommandHandler()
        {
            _addRateViewModel.Initialize(ViewMode.Edit, SelectedRate.ToDto());
            if (_dialogAgent.ShowDialog<AddRateWindow>(_addRateViewModel) == false)
                return;

            var rate = _addRateViewModel.GetRate();
            SelectedRate.Update(rate);
        }

        private bool CanDeleteRateCommandHandler()
        {
            return SelectedRate != null;
        }

        private void DeleteRateCommandHandler()
        {
            Rates.Remove(SelectedRate);
        }

        private void TechnologyOperationRepositoryOnChanged(object sender, RepositoryChangedEventArgs<TechnologyOperationDto> e)
        {
            _operationNames = _technologyOperationRepository.Items.Select(x => x.Name.ToLower()).ToArray();
        }

        private void PricePositionRepositoryOnChanged(object sender, RepositoryChangedEventArgs<PricePositionDto> e)
        {
            ValidateUserCode();
        }
    }
}