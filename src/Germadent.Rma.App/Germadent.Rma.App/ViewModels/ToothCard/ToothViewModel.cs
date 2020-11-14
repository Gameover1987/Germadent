using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Germadent.Common.Extensions;
using Germadent.Rma.App.ViewModels.Pricing;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.ToothCard
{
    public class ToothViewModel : ViewModelBase
    {
        private bool _hasBridge;
        private bool _isChanged;
        private PricePositionViewModel[] _pricePositions;

        public ToothViewModel(DictionaryItemDto[] prostheticConditions)
        {
            prostheticConditions.ForEach(x =>
            {
                var prostheticConditionViewModel = new CheckableDictionaryItemViewModel(x);
                prostheticConditionViewModel.Checked += ProstheticConditionViewModelOnChecked;
                ProstheticConditions.Add(prostheticConditionViewModel);
            });
        }

        public int Number { get; set; }

        public bool IsChanged
        {
            get => _isChanged;
            set
            {
                _isChanged = value;
                OnPropertyChanged(() => IsChanged);

                ToothChanged?.Invoke(this, new ToothChangedEventArgs(false));
            }
        }

        public bool HasBridge
        {
            get => _hasBridge;
            set
            {
                if (_hasBridge == value)
                    return;

                _hasBridge = value;
                _isChanged = GetIsChecked();
                OnPropertyChanged(() => HasBridge);
                OnPropertyChanged(() => IsChanged);

                ToothChanged?.Invoke(this, new ToothChangedEventArgs(true));
            }
        }

        public ObservableCollection<CheckableDictionaryItemViewModel> ProstheticConditions { get; } = new ObservableCollection<CheckableDictionaryItemViewModel>();

        public CheckableDictionaryItemViewModel SelectedProstheticCondition
        {
            get { return ProstheticConditions.FirstOrDefault(x => x.IsChecked); }
        }

        public bool HasDescription => SelectedProstheticCondition != null || HasBridge;

        public bool IsValid
        {
            get
            {
                if (!IsChanged)
                    return true;

                if (SelectedProstheticCondition == null)
                    return false;

                if (_pricePositions == null || _pricePositions.Length == 0)
                    return false;

                return true;
            }
        }

        public string Description
        {
            get
            {
                var dto = ToDto();
                return OrderDescriptionBuilder.GetToothDescription(dto);
            }
        }

        public string ErrorDescription
        {
            get
            {
                var descriptionBuilder = new StringBuilder();
                if (SelectedProstheticCondition == null)
                    descriptionBuilder.AppendLine("Выберите условие протезирования");

                if (_pricePositions.Length == 0)
                    descriptionBuilder.AppendLine("Выберите ценовую позицию");

                return descriptionBuilder.ToString();
            }
        }

        public event EventHandler<ToothChangedEventArgs> ToothChanged;

        public event EventHandler<ToothCleanUpEventArgs> ToothCleanup; 

        public void AttachPricePositions(PricePositionViewModel[] pricePositions)
        {
            _pricePositions = pricePositions;

            if (_pricePositions.Any())
                IsChanged = true;
        }

        public void Initialize(ToothDto toothDto)
        {
            IsChanged = true;
            Number = toothDto.ToothNumber;

            var selectedProstheticCondition = ProstheticConditions.FirstOrDefault(x => x.DisplayName == toothDto.ConditionName);
            if (selectedProstheticCondition != null)
                selectedProstheticCondition.IsChecked = true;

            _hasBridge = toothDto.HasBridge;

            _pricePositions = toothDto.PricePositions.Select(x => new PricePositionViewModel(x) { IsChecked = true }).ToArray();

            OnPropertyChanged();
        }

        public ToothDto ToDto()
        {
            return new ToothDto()
            {
                ToothNumber = Number,
                ConditionId = SelectedProstheticCondition?.Item.Id ?? 0,
                ConditionName = SelectedProstheticCondition?.DisplayName,
                PricePositions = _pricePositions?.Select(x => x.ToDto()).ToArray(),
                HasBridge = HasBridge
            };
        }

        public bool CanClear => HasBridge || SelectedProstheticCondition != null && !_pricePositions.IsNullOrEmpty();

        public void Clear()
        {
            HasBridge = false;
            ProstheticConditions.ForEach(x => x.ResetIsChanged());
            _pricePositions = null;

            IsChanged = false;

            ToothCleanup?.Invoke(this, new ToothCleanUpEventArgs(this));
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Number, Description); ;
        }

        private bool GetIsChecked()
        {
            return HasBridge || SelectedProstheticCondition != null || !_pricePositions.IsNullOrEmpty();
        }

        private void ProstheticConditionViewModelOnChecked(object sender, EventArgs e)
        {
            var prostheticConditionViewModels = ProstheticConditions.Where(x => x != sender);
            prostheticConditionViewModels.ForEach(x => x.ResetIsChanged());

            OnPropertyChanged(() => SelectedProstheticCondition);
            IsChanged = GetIsChecked();
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            base.OnPropertyChanged(nameof(IsValid));
            base.OnPropertyChanged(nameof(ErrorDescription));
            base.OnPropertyChanged(nameof(HasDescription));
            base.OnPropertyChanged(nameof(Description));
        }
    }
}
