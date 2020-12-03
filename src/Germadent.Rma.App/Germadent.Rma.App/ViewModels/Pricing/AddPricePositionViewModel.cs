using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Germadent.Common;
using Germadent.Common.Extensions;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels.ToothCard;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.App.Views.Pricing;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels.Validation;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public class AddPricePositionViewModel : ValidationSupportableViewModel, IAddPricePositionViewModel
    {
        private readonly IPriceGroupRepository _priceGroupRepository;
        private readonly IDictionaryRepository _dictionaryRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUiTimer _timer;
        private readonly IShowDialogAgent _dialogAgent;
        private readonly IAddPriceViewModel _addPriceViewModel;
        private BranchType _branchType;
        private string _userCode;
        private string[] _allUserCodes;
        private int _pricePositionId;
        private string _name;

        private PriceGroupDto _selectedPriceGroup;
        private DictionaryItemDto _selectedMaterial;     
        private PriceViewModel _selectedPrice;

        public AddPricePositionViewModel(IPriceGroupRepository priceGroupRepository,
            IDictionaryRepository dictionaryRepository,
            IDateTimeProvider dateTimeProvider,
            IUiTimer timer,
            IShowDialogAgent dialogAgent,
            IAddPriceViewModel addPriceViewModel)
        {
            _priceGroupRepository = priceGroupRepository;
            _dictionaryRepository = dictionaryRepository;
            _dateTimeProvider = dateTimeProvider;
            _timer = timer;
            _dialogAgent = dialogAgent;
            _addPriceViewModel = addPriceViewModel;
            _timer.Tick += TimerOnTick;

            AddValidationFor(() => Name)
                .When(() => string.IsNullOrWhiteSpace(Name), () => "Укажите наименование ценовой позиции");
            AddValidationFor(() => UserCode)
                .When(() => string.IsNullOrWhiteSpace(UserCode), () => "Укажите код");
            AddValidationFor(() => UserCode)
                .When(() => _allUserCodes.ContainsIgnoreCase(UserCode), () => "Укажите уникальный код");

            OkCommand = new DelegateCommand(CanOkCommandHandler);
            AddPriceCommand = new DelegateCommand(AddPriceCommandHandler);
            EditPriceCommand = new DelegateCommand(EditPriceCommandHandler, CanEditPriceCommandHandler);
            DeletePriceCommand = new DelegateCommand(DeletePriceCommandHandler, CanDeletePriceCommandHandler);

            Prices.CollectionChanged += PricesOnCollectionChanged;
        }

        public string Title => GetTitle(ViewMode);

        public CardViewMode ViewMode { get; set; }

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

        public string UserCode
        {
            get { return _userCode; }
            set
            {
                if (_userCode == value)
                    return;
                _userCode = value;
                OnPropertyChanged(() => UserCode);
            }
        }

        public ObservableCollection<PriceGroupDto> Groups { get; } = new ObservableCollection<PriceGroupDto>();

        public PriceGroupDto SelectedPriceGroup
        {
            get { return _selectedPriceGroup; }
            set
            {
                if (_selectedPriceGroup == value)
                    return;
                _selectedPriceGroup = value;
                OnPropertyChanged(() => SelectedPriceGroup);
            }
        }

        public ObservableCollection<DictionaryItemDto> Materials { get; } = new ObservableCollection<DictionaryItemDto>();

        public DictionaryItemDto SelectedMaterial
        {
            get { return _selectedMaterial; }
            set
            {
                if (_selectedMaterial == value)
                    return;
                _selectedMaterial = value;
                OnPropertyChanged(() => SelectedMaterial);
            }
        }

        public ObservableCollection<CheckableDictionaryItemViewModel> ProsthteticTypes { get; } = new ObservableCollection<CheckableDictionaryItemViewModel>();

        public ObservableCollection<PriceViewModel> Prices { get; } = new ObservableCollection<PriceViewModel>();

        public PriceViewModel SelectedPrice
        {
            get { return _selectedPrice; }
            set
            {
                if (_selectedPrice == value)
                    return;
                _selectedPrice = value;
                OnPropertyChanged(() => SelectedPrice);
            }
        }

        public IDelegateCommand OkCommand { get; }

        public IDelegateCommand AddPriceCommand { get; }

        public IDelegateCommand EditPriceCommand { get; }

        public IDelegateCommand DeletePriceCommand { get; }

        private bool CanOkCommandHandler()
        {
            return !HasErrors && IsValid();
        }

        private bool IsValid()
        {
            if (Name.IsNullOrWhiteSpace())
                return false;

            if (UserCode.IsNullOrWhiteSpace())
                return false;

            if (SelectedPriceGroup == null)
                return false;

            if (SelectedMaterial == null)
                return false;

            if (ProsthteticTypes.All(x => x.IsChecked == false))
                return false;

            if (Prices.Count == 0)
                return false;

            if (Prices.Count(x => x.PriceKind == PriceKind.Current) == 0)
                return false;

            return true;
        }

        public void Initialize(CardViewMode viewMode, PricePositionDto pricePositionDto, string[] allUserCodes, BranchType branchType)
        {
            ResetValidation();          
            _timer.Stop();
            _timer.Initialize(TimeSpan.FromSeconds(2));

            ViewMode = viewMode;
            _branchType = branchType;
            _allUserCodes = allUserCodes;

            Groups.Clear();
            foreach (var priceGroupDto in _priceGroupRepository.Items.Where(x => x.BranchType == branchType))
            {
                Groups.Add(priceGroupDto);
            }

            Materials.Clear();
            foreach (var dictionaryItemDto in _dictionaryRepository.Items.Where(x => x.Dictionary == DictionaryType.Material))
            {
                Materials.Add(dictionaryItemDto);
            }

            ProsthteticTypes.Clear();
            var prostheticIDs = pricePositionDto.Products.Select(x => x.ProductId).ToArray();
            foreach (var dictionaryItemDto in _dictionaryRepository.Items.Where(x => x.Dictionary == DictionaryType.ProstheticType))
            {
                var item = new CheckableDictionaryItemViewModel(dictionaryItemDto);
                item.IsChecked = prostheticIDs.Contains(dictionaryItemDto.Id);
                ProsthteticTypes.Add(item);
            }

            Prices.Clear();
            foreach (var priceDto in pricePositionDto.Prices)
            {
                Prices.Add(new PriceViewModel(priceDto));
            }

            ActualizePrices();

            _pricePositionId = pricePositionDto.PricePositionId;
            _name = pricePositionDto.Name;
            _userCode = pricePositionDto.UserCode;
            _selectedPriceGroup = Groups.FirstOrDefault(x => x.PriceGroupId == pricePositionDto.PriceGroupId);
            _selectedMaterial = Materials.FirstOrDefault(x => x.Id == pricePositionDto.MaterialId);          

            _timer.Start();
        }

        public PricePositionDto GetPricePosition()
        {
            return new PricePositionDto
            {
                PricePositionId = _pricePositionId,
                Name = Name,
                UserCode = UserCode,
                BranchType = _branchType,
                PriceGroupId = SelectedPriceGroup.PriceGroupId,
                MaterialId = SelectedMaterial.Id,
                Products = ProsthteticTypes.Where(x => x.IsChecked).Select(x => new ProductDto
                {
                    ProductName = x.Item.Name,
                    ProductId = x.Item.Id,
                    MaterialId = SelectedMaterial.Id,
                }).ToArray(),
                Prices = Prices.Select(x => x.ToDto()).ToArray()
            };
        }

        private void AddPriceCommandHandler()
        {
            _addPriceViewModel.Initialize(new PriceDto { DateBeginning = _dateTimeProvider.GetDateTime() });
            if (_dialogAgent.ShowDialog<AddPriceWindow>(_addPriceViewModel) == false)
                return;

            var price = _addPriceViewModel.GetPrice();
            Prices.Add(new PriceViewModel(price));
        }

        private bool CanEditPriceCommandHandler()
        {
            return SelectedPrice != null;
        }

        private void EditPriceCommandHandler()
        {
            _addPriceViewModel.Initialize(SelectedPrice.ToDto());
            if (_dialogAgent.ShowDialog<AddPriceWindow>(_addPriceViewModel) == false)
                return;

            var price = _addPriceViewModel.GetPrice();
            SelectedPrice.Update(price);
        }

        private bool CanDeletePriceCommandHandler()
        {
            return SelectedPrice != null;
        }

        private void DeletePriceCommandHandler()
        {
            Prices.Remove(SelectedPrice);
        }

        private string GetTitle(CardViewMode cardViewMode)
        {
            switch (cardViewMode)
            {
                case CardViewMode.Add:
                    return "Добавление ценовой позиции";

                case CardViewMode.Edit:
                    return "Редактирование ценовой позиции";

                case CardViewMode.View:
                    return "Просмотр данных ценовой позиции";

                default:
                    throw new NotImplementedException("Неизвестный режим представления");
            }
        }

        private void ActualizePrices()
        {
            if (Prices.IsNullOrEmpty())
                return;

            Prices.ForEach(x => x.PriceKind = PriceKind.Past);

            if (Prices.Count == 1)
            {
                Prices.First().PriceKind = PriceKind.Current;
            }

            var pastPrices = Prices.OrderBy(x => x.Begin).Where(x => x.Begin < _dateTimeProvider.GetDateTime()).ToArray();
            pastPrices.ForEach(x => x.PriceKind = PriceKind.Past);
            var futurePrices = Prices.OrderBy(x => x.Begin).Where(x => x.Begin > _dateTimeProvider.GetDateTime()).ToArray();
            futurePrices.ForEach(x => x.PriceKind = PriceKind.Future);

            pastPrices.Last().PriceKind = PriceKind.Current;
        }

        private void TimerOnTick(object sender, EventArgs e)
        {
            ActualizePrices();

            DelegateCommand.NotifyCanExecuteChangedForAll();
        }

        private void PricesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ActualizePrices();

            DelegateCommand.NotifyCanExecuteChangedForAll();
        }
    }
}