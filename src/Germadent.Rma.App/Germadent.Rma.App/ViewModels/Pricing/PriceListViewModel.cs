using System.Collections.ObjectModel;
using System.Linq;
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

        public PriceListViewModel(IPriceGroupRepository priceGroupRepository, IPricePositionRepository pricePositionRepository)
        {
            _priceGroupRepository = priceGroupRepository;
            _pricePositionRepository = pricePositionRepository;

            Groups = new ObservableCollection<PriceGroupViewModel>();
            Positions = new ObservableCollection<PricePositionViewModel>();
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

                UpdatePositions();
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
        }

        private void UpdatePositions()
        {
            Positions.Clear();

            if (SelectedGroup == null)
                return;

            var positionsFromGroup = _pricePositionRepository.Items.Where(x => x.BranchType == _branchType && x.PriceGroupId == SelectedGroup.PriceGroupId).ToArray();
            foreach (var pricePositionDto in positionsFromGroup)
            {
                Positions.Add(new PricePositionViewModel(pricePositionDto));
            }
        }
    }
}