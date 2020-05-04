﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Germadent.Common.Extensions;
using Germadent.Common.Logging;
using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.Model;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public class CustomerCatalogViewModel : ViewModelBase, ICustomerCatalogViewModel
    {
        private readonly IRmaOperations _rmaOperations;
        private readonly ICatalogUIOperations _catalogUIOperations;
        private readonly ILogger _logger;

        private ICustomerViewModel _selectedCustomer;
        private bool _isBusy;
        private string _searchString;

        private ICollectionView _customersView;

        public CustomerCatalogViewModel(IRmaOperations rmaOperations, ICatalogUIOperations catalogUIOperations, ILogger logger)
        {
            _rmaOperations = rmaOperations;
            _catalogUIOperations = catalogUIOperations;
            _logger = logger;

            AddCustomerCommand = new DelegateCommand(AddCustomerCommandHandler);
            EditCustomerCommand = new DelegateCommand(EditCustomerCommandHandler, CanEditCustomerCommandHandler);
            DeleteCustomerCommand = new DelegateCommand(DeleteCustomerCommandHandler, CanDeleteCustomerCommandHandler);
            SelectCustomerCommand = new DelegateCommand(() => { }, () => SelectedCustomer != null);

            _customersView = CollectionViewSource.GetDefaultView(Customers);
            _customersView.Filter = CustomersFilter;
        }

        private bool CustomersFilter(object obj)
        {
            if (SearchString.IsNullOrWhiteSpace())
                return true;

            var customerViewModel = (ICustomerViewModel)obj;
            if (!customerViewModel.DisplayName.ToLower().Contains(SearchString.ToLower()))
                return false;

            return true;
        }

        public ObservableCollection<ICustomerViewModel> Customers { get; } = new ObservableCollection<ICustomerViewModel>();

        public ICustomerViewModel SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                if (_selectedCustomer == value)
                    return;
                _selectedCustomer = value;
                OnPropertyChanged(() => SelectedCustomer);
            }
        }

        public string SearchString
        {
            get { return _searchString; }
            set
            {
                if (_searchString == value)
                    return;
                _searchString = value;
                OnPropertyChanged(() => SearchString);

                _customersView.Refresh();
            }
        }

        public IDelegateCommand AddCustomerCommand { get; }

        public IDelegateCommand EditCustomerCommand { get; }

        public IDelegateCommand DeleteCustomerCommand { get; }

        public IDelegateCommand SelectCustomerCommand { get; }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value)
                    return;
                _isBusy = value;
                OnPropertyChanged(() => IsBusy);
            }
        }

        public async void Initialize()
        {
            try
            {
                IsBusy = true;
                Customers.Clear();

                CustomerDto[] customers = null;
                await ThreadTaskExtensions.Run(() =>
                {
                    customers = _rmaOperations.GetCustomers("");
                });

                foreach (var customer in customers)
                {
                    Customers.Add(new CustomerViewModel(customer));
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void AddCustomerCommandHandler()
        {
            var customer = _catalogUIOperations.AddCustomer(new CustomerDto());
            if (customer == null)
                return;

            customer = _rmaOperations.AddCustomer(customer);
            Customers.Add(new CustomerViewModel(customer));
        }

        private bool CanEditCustomerCommandHandler()
        {
            return SelectedCustomer != null;
        }

        private void EditCustomerCommandHandler()
        {

        }

        private bool CanDeleteCustomerCommandHandler()
        {
            return SelectedCustomer != null;
        }

        private void DeleteCustomerCommandHandler()
        {

        }
    }
}
