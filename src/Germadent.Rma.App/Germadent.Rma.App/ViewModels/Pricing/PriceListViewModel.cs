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
        private readonly IPricePositionRepository _pricePositionRepository;

        private PriceGroupViewModel _selectedGroup;
        private PricePositionViewModel _selectedPosition;

        private readonly ICollectionView _pricePositionsView;

        public PriceListViewModel(IPriceGroupRepository priceGroupRepository, IPricePositionRepository pricePositionRepository)
        {
            _priceGroupRepository = priceGroupRepository;
            _pricePositionRepository = pricePositionRepository;

            Groups = new ObservableCollection<PriceGroupViewModel>();
            Positions = new ObservableCollection<PricePositionViewModel>();

            _pricePositionsView = CollectionViewSource.GetDefaultView(Positions);
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

        public ObservableCollection<PricePositionViewModel> Positions { get; }

        public PricePositionViewModel SelectedPosition
        {
            get { return _selectedPosition; }
            set
            {
                if (_selectedPosition == value)
                    return;
                _selectedPosition = value;
                OnPropertyChanged(() => SelectedPosition);
            }
        }

        public void Initialize(BranchType branchType)
        {
            _branchType = branchType;
            Groups.Clear();

            var groups = _priceGroupRepository.Items.Where(x => x.BranchType == branchType).ToArray();
            foreach (var priceGroupDto in groups)
            {
                Groups.Add(new PriceGroupViewModel(priceGroupDto));
            }

            Positions.ForEach(x => x.Checked -= PricePositionOnChecked);
            Positions.Clear();
            var positions = _pricePositionRepository.Items.Where(x => x.BranchType == _branchType).ToArray();
            foreach (var pricePositionDto in positions)
            {
                var pricePositionViewModel = new PricePositionViewModel(pricePositionDto);
                pricePositionViewModel.Checked += PricePositionOnChecked;
                Positions.Add(pricePositionViewModel);
            }
        }

        public void Setup(ToothDto toothDto)
        {
            
        }

        public event EventHandler<PricePositionCheckedEventArgs> PricePositionChecked;

        private void PricePositionOnChecked(object sender, PricePositionCheckedEventArgs e)
        {
            PricePositionChecked?.Invoke(this, e);
        }
    }
}