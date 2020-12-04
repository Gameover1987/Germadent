﻿using System;
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

        private readonly ICollectionView _productsView;

        public PriceListViewModel(IPriceGroupRepository priceGroupRepository, IProductRepository productRepository)
        {
            _priceGroupRepository = priceGroupRepository;
            _productRepository = productRepository;

            Groups = new ObservableCollection<PriceGroupViewModel>();
            Products = new ObservableCollection<ProductViewModel>();

            _productsView = CollectionViewSource.GetDefaultView(Products);
            _productsView.Filter = ProductFilter;
        }

        private bool ProductFilter(object obj)
        {
            if (SelectedGroup == null)
                return false;

            var pricePosition = (ProductViewModel) obj;
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

                _productsView.Refresh();
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

            Products.ForEach(x => x.Checked -= ProductOnChecked);
            Products.Clear();
            var products = _productRepository.Items.ToArray();
            foreach (var productDto in products)
            {
                var productViewModel = new ProductViewModel(productDto);
                productViewModel.Checked += ProductOnChecked;
                Products.Add(productViewModel);
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
                var productViewModel = Products.First(x => x.ProductId == productDto.ProductId);
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
        
        public event EventHandler ProductChecked;

        private void ProductOnChecked(object sender, ProductCheckedEventArgs e)
        {
            var group = Groups.First(x => x.PriceGroupId == e.Product.PriceGroupId);
            group.HasChanges = Products.Where(x => x.PriceGroupId == group.PriceGroupId).Any(x => x.IsChecked);

            ProductChecked?.Invoke(this, e);
        }
    }
}