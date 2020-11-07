using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Germadent.Common.Extensions;
using Germadent.Common.Logging;
using Germadent.Rma.App.Operations;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.Model;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public class ResponsiblePersonCatalogViewModel : ViewModelBase, IResponsiblePersonCatalogViewModel
    {
        private readonly IResponsiblePersonRepository _responsiblePersonRepository;
        private readonly ICatalogUIOperations _catalogUIOperations;
        private readonly ILogger _logger;

        private IResponsiblePersonViewModel _selectedResponsiblePerson;
        private bool _isBusy;
        private string _searchString;

        private readonly ICollectionView _customersView;

        public ResponsiblePersonCatalogViewModel(IResponsiblePersonRepository responsiblePersonRepository, ICatalogUIOperations catalogUIOperations, ILogger logger)
        {
            _responsiblePersonRepository = responsiblePersonRepository;
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
            get => _selectedResponsiblePerson;
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
            get => _searchString;
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
            get => _isBusy;
            set
            {
                if (_isBusy == value)
                    return;
                _isBusy = value;
                OnPropertyChanged(() => IsBusy);
            }
        }

        public void Initialize()
        {
            try
            {
                IsBusy = true;
                ResponsiblePersons.Clear();

                foreach (var responsiblePersonDto in _responsiblePersonRepository.Items)
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
            var responsiblePerson = _catalogUIOperations.AddResponsiblePerson(new ResponsiblePersonDto());
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
            var updatedResponsiblePerson = _catalogUIOperations.UpdateResponsiblePerson(SelectedResponsiblePerson.ToDto());
            if (updatedResponsiblePerson == null)
                return;

            SelectedResponsiblePerson.Update(updatedResponsiblePerson);
        }

        private bool CanDeleteResponsiblePersonCommandHandler()
        {
            return SelectedResponsiblePerson != null;
        }

        private void DeleteResponsiblePersonCommandHandler()
        {
            var result = _catalogUIOperations.DeleteResponsiblePerson(SelectedResponsiblePerson.ResponsiblePersonId);
            if (result == null)
                return;

            ResponsiblePersons.Remove(SelectedResponsiblePerson);
        }
    }
}
