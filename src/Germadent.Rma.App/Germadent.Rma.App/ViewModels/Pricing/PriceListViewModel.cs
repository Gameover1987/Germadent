using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Germadent.Common.Extensions;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public class PriceListViewModel : ViewModelBase, IPriceListViewModel
    {
        private BranchType _branchType;

        private readonly IPriceGroupRepository _priceGroupRepository;
        private readonly IProductRepository _productRepository;

        private PriceGroupViewModel _selectedGroup;

        private readonly ICollectionView _pricePositionsView;

        public PriceListViewModel(IPriceGroupRepository priceGroupRepository, IProductRepository productRepository)
        {
            _priceGroupRepository = priceGroupRepository;
            _productRepository = productRepository;

            Groups = new ObservableCollection<PriceGroupViewModel>();
            Products = new ObservableCollection<ProductViewModel>();

            _pricePositionsView = CollectionViewSource.GetDefaultView(Products);
            _pricePositionsView.Filter = PricePositionFilter;
        }

        private bool PricePositionFilter(object obj)
        {
            if (SelectedGroup == null)
                return false;

            var pricePosition = (PricePositionViewModel) obj;
            return SelectedGroup.PriceGroupId == pricePosition.PriceGroupId;
        }

        public ObservableCollection<PriceGroupViewModel> Groups { get; }

        public PriceGroupViewModel SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                if (_selectedGroup == value)
                    return;
                _selectedGroup = value;
                OnPropertyChanged(() => SelectedGroup);

                _pricePositionsView.Refresh();
            }
        }

        public ObservableCollection<ProductViewModel> Products { get; }

        public void Initialize(BranchType branchType)
        {
            _branchType = branchType;
            Groups.Clear();

            var groups = _priceGroupRepository.Items.Where(x => x.BranchType == branchType).ToArray();
            foreach (var priceGroupDto in groups)
            {
                Groups.Add(new PriceGroupViewModel(priceGroupDto));
            }

            SelectedGroup = Groups.FirstOrDefault();

            Products.ForEach(x => x.Checked -= PricePositionOnChecked);
            Products.Clear();
            var positions = _productRepository.Items.Where(x => x.BranchType == _branchType).ToArray();
            foreach (var pricePositionDto in positions)
            {
                var pricePositionViewModel = new ProductViewModel(pricePositionDto);
                pricePositionViewModel.Checked += PricePositionOnChecked;
                Products.Add(pricePositionViewModel);
            }
        }

        public void Setup(ToothDto toothDto)
        {
            Products.ForEach(x => x.SetIsChecked(false));
            Groups.ForEach(x => x.HasChanges = false);

            if (toothDto.Products == null)
                return;

            foreach (var productDto in toothDto.Products)
            {
                var productViewModel = Products.First(x => x.PricePositionId == productDto.PricePositionId);
                productViewModel.SetIsChecked(true);
            }

            var groupsWithChanges = Groups
                .Where(group =>
                    Products
                        .Where(x => x.IsChecked)
                        .Select(x => x.PriceGroupId)
                        .Contains(group.PriceGroupId))
                .ToArray();
            groupsWithChanges.ForEach(x => x.HasChanges = true);
        }

        public event EventHandler PricePositionsChecked;
        public event EventHandler ProductChecked;

        private void PricePositionOnChecked(object sender, ProductCheckedEventArgs e)
        {
            var group = Groups.First(x => x.PriceGroupId == e.Product.PriceGroupId);
            group.HasChanges = Products.Where(x => x.PriceGroupId == group.PriceGroupId).Any(x => x.IsChecked);

            PricePositionsChecked?.Invoke(this, e);
        }
    }
}