using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public class PriceListViewModel : ViewModelBase, IPriceListViewModel
    {
        private readonly IRmaServiceClient _serviceClient;
        private PriceGroupViewModel _selectedPriceGroup;

        public PriceListViewModel(IRmaServiceClient serviceClient)
        {
            _serviceClient = serviceClient;

            PriceGroups = new ObservableCollection<PriceGroupViewModel>();
        }

        public ObservableCollection<PriceGroupViewModel> PriceGroups { get; }

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

        public void Initialize(BranchType branchType)
        {
            PriceGroups.Clear();

            var groups = _serviceClient.GetPriceGroups(branchType);
            foreach (var priceGroupDto in groups)
            {
                PriceGroups.Add(new PriceGroupViewModel(priceGroupDto));
            }
        }
    }
}