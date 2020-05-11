using System;
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
    public class ResponsiblePersonsCatalogViewModel : ViewModelBase, IResponsiblePersonCatalogViewModel
    {
        private readonly IRmaServiceClient _rmaOperations;
        private readonly ICatalogUIOperations _catalogUIOperations;
        private readonly ILogger _logger;

        private IResponsiblePersonViewModel _selectedResponsiblePerson;
        private bool _isBusy;
        private string _searchString;

        private ICollectionView _customersView;

        public ResponsiblePersonsCatalogViewModel(IRmaServiceClient rmaOperations, ICatalogUIOperations catalogUIOperations, ILogger logger)
        {
            _rmaOperations = rmaOperations;
            _catalogUIOperations = catalogUIOperations;
            _logger = logger;

            AddResponsiblePersonCommand = new DelegateCommand(AddResponsiblePersonCommandHandler);
            EditResponsiblePersonCommand = new DelegateCommand(EditResponsiblePersonCommandHandler, CanEditResponsiblePersonCommandHandler);
            DeleteResponsiblePersonCommand = new DelegateCommand(DeleteResponsiblePersonCommandHandler, CanDeleteResponsiblePersonCommandHandler);
            SelectResponsiblePersonCommand = new DelegateCommand(() => { }, () => SelectedResponsiblePerson != null);

            _customersView = CollectionViewSource.GetDefaultView(ResponsiblePersons);
            _customersView.Filter = ResponsiblePersonsFilter;
        }

        private bool ResponsiblePersonsFilter(object obj)
        {
            if (SearchString.IsNullOrWhiteSpace())
                return true;

            var responsiblePersonViewModel = (IResponsiblePersonViewModel)obj;
            if (!responsiblePersonViewModel.FullName.ToLower().Contains(SearchString.ToLower()))
                return false;

            return true;
        }

        public ObservableCollection<IResponsiblePersonViewModel> ResponsiblePersons { get; } = new ObservableCollection<IResponsiblePersonViewModel>();

        public IResponsiblePersonViewModel SelectedResponsiblePerson
        {
            get { return _selectedResponsiblePerson; }
            set
            {
                if (_selectedResponsiblePerson == value)
                    return;
                _selectedResponsiblePerson = value;
                OnPropertyChanged(() => SelectedResponsiblePerson);
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

        public IDelegateCommand AddResponsiblePersonCommand { get; }

        public IDelegateCommand EditResponsiblePersonCommand { get; }

        public IDelegateCommand DeleteResponsiblePersonCommand { get; }

        public IDelegateCommand SelectResponsiblePersonCommand { get; }

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
                ResponsiblePersons.Clear();

                ResponsiblePersonDto[] responsiblePersons = null;
                await ThreadTaskExtensions.Run(() =>
                {
                    responsiblePersons = _rmaOperations.GetResponsiblePersons();
                });

                foreach (var responsiblePersonDto in responsiblePersons)
                {
                    ResponsiblePersons.Add(new ResponsiblePersonViewModel(responsiblePersonDto));
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

        private void AddResponsiblePersonCommandHandler()
        {
            var responsiblePerson = _catalogUIOperations.AddResponsiblePersons(new ResponsiblePersonDto());
            if (responsiblePerson == null)
                return;

            ResponsiblePersons.Add(new ResponsiblePersonViewModel(responsiblePerson));
        }

        private bool CanEditResponsiblePersonCommandHandler()
        {
            return SelectedResponsiblePerson != null;
        }

        private void EditResponsiblePersonCommandHandler()
        {

        }

        private bool CanDeleteResponsiblePersonCommandHandler()
        {
            return SelectedResponsiblePerson != null;
        }

        private void DeleteResponsiblePersonCommandHandler()
        {

        }
    }
}
