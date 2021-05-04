using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ViewModels;
using Germadent.Common.Extensions;
using Germadent.Model;
using Germadent.Model.Pricing;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.App.ViewModels.Pricing;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.ToothCard
{
    public class ToothViewModel : ViewModelBase
    {
        private readonly IProductRepository _productRepository;

        private bool _hasBridge;
        private bool _isChanged;
        private ProductViewModel[] _products;

        public ToothViewModel(IDictionaryRepository dictionaryRepository, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            dictionaryRepository
                .Items
                .Where(x => x.Dictionary == DictionaryType.ProstheticCondition)
                .ForEach(x =>
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

                if (_products.IsNullOrEmpty())
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

                if (_products.IsNullOrEmpty())
                    descriptionBuilder.AppendLine("Выберите ценовую позицию");

                return descriptionBuilder.ToString();
            }
        }

        public event EventHandler<ToothChangedEventArgs> ToothChanged;

        public event EventHandler<ToothCleanUpEventArgs> ToothCleanup; 

        public void AttachPricePositions(ProductViewModel[] products)
        {
            _products = products;

            if (_products.Any())
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

            var products = new List<ProductDto>();
            foreach (var productDto in toothDto.Products)
            {
                var productFromRepository = _productRepository.Items.First(x =>
                    x.ProductId == productDto.ProductId && x.PricePositionId == productDto.PricePositionId);
                products.Add(productFromRepository);
            }

            _products = products.Select(x => new ProductViewModel(x)).ToArray();

            OnPropertyChanged();
        }

        public ToothDto ToDto()
        {
            return new ToothDto()
            {
                ToothNumber = Number,
                Products = _products?.Select(x => x.ToDto()).ToArray(),
                ConditionId = SelectedProstheticCondition?.Item.Id ?? 0,
                ConditionName = SelectedProstheticCondition?.DisplayName,
                HasBridge = HasBridge
            };
        }

        public bool CanClear => HasBridge || SelectedProstheticCondition != null || !_products.IsNullOrEmpty();

        public void Clear()
        {
            HasBridge = false;
            ProstheticConditions.ForEach(x => x.ResetIsChanged());
            _products = null;

            IsChanged = false;

            ToothCleanup?.Invoke(this, new ToothCleanUpEventArgs(this));
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Number, Description); ;
        }

        private bool GetIsChecked()
        {
            return HasBridge || SelectedProstheticCondition != null || !_products.IsNullOrEmpty();
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
