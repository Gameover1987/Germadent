using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Accessibility;
using Germadent.Common.Extensions;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;
using Germadent.UI.ViewModels.Validation;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public class AddPricePositionViewModel : ValidationSupportableViewModel, IAddPricePositionViewModel
    {
        private readonly IPriceGroupRepository _priceGroupRepository;
        private BranchType _branchType;
        private string _userCode;
        private string[] _allUserCodes;
        private string _name;
        private decimal _priceStl;
        private decimal _priceModel;
        private PriceGroupViewModel _selectedPriceGroup;

        public AddPricePositionViewModel(IPriceGroupRepository priceGroupRepository)
        {
            _priceGroupRepository = priceGroupRepository;

            AddValidationFor(() => Name)
                .When(() => string.IsNullOrWhiteSpace(Name), () => "Укажите наименование ценовой позиции");

            AddValidationFor(() => UserCode)
                .When(() => string.IsNullOrWhiteSpace(UserCode), () => "Укажите код");
            AddValidationFor(() => UserCode)
                .When(() => _allUserCodes.ContainsIgnoreCase(UserCode), () => "Укажите уникальный код");

            AddValidationFor(() => PriceStl)
                .When(() => PriceStl <= 0, () => "Укажите цену с STL отличную от нуля");
            AddValidationFor(() => PriceModel)
                .When(() => PriceStl <= 0, () => "Укажите цену с модели отличную от нуля");


            OkCommand = new DelegateCommand(CanOkCommandHandler);
        }

        public string Title
        {
            get { return GetTitle(ViewMode); }
        }

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

        public ObservableCollection<PriceGroupViewModel> Groups { get; } = new ObservableCollection<PriceGroupViewModel>();

        public PriceGroupViewModel SelectedPriceGroup
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

        public decimal PriceStl
        {
            get { return _priceStl; }
            set
            {
                if (_priceStl == value)
                    return;
                _priceStl = value;
                OnPropertyChanged(() => PriceStl);
            }
        }

        public decimal PriceModel
        {
            get { return _priceModel; }
            set
            {
                if (_priceModel == value)
                    return;
                _priceModel = value;
                OnPropertyChanged(() => PriceModel);
            }
        }
        
        public IDelegateCommand OkCommand { get; }

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

            if (PriceModel <= 0 || PriceStl <= 0)
                return false;

            return true;
        }

        public void Initialize(CardViewMode viewMode, PricePositionDto pricePositionDto, string[] allUserCodes, BranchType branchType)
        {
            ViewMode = viewMode;
            _branchType = branchType;
            _allUserCodes = allUserCodes;

            Groups.Clear();
            foreach (var priceGroupDto in _priceGroupRepository.Items.Where(x => x.BranchType == branchType))
            {
                Groups.Add(new PriceGroupViewModel(priceGroupDto));
            }

            Name = pricePositionDto.Name;
            UserCode = pricePositionDto.UserCode;
            SelectedPriceGroup = Groups.FirstOrDefault(x => x.PriceGroupId == pricePositionDto.PriceGroupId);

            PriceStl = pricePositionDto.PriceStl;
            PriceModel = pricePositionDto.PriceModel;
        }

        public PricePositionDto GetPricePosition()
        {
            return new PricePositionDto
            {
                BranchType = _branchType,
                Name = Name,
                PriceGroupId = SelectedPriceGroup.PriceGroupId,
                PriceStl =  PriceStl,
                PriceModel = PriceModel,
                UserCode = UserCode
            };
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
    }
}