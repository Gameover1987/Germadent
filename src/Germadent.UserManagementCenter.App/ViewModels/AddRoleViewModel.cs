using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels.Validation;
using Germadent.UserManagementCenter.App.ServiceClient;
using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public class AddRoleViewModel : ValidationSupportableViewModel, IAddRoleViewModel
    {
        private readonly IUmcOperations _userManagementCenterOperations;
        private string _roleName;

        private ICollectionView _rightsView;

        public AddRoleViewModel(IUmcOperations userManagementCenterOperations)
        {
            _userManagementCenterOperations = userManagementCenterOperations;

            InitializeRightsView();

            OkCommand = new DelegateCommand(() => { }, CanOkCommandHandler);

            AddValidationFor(() => RoleName)
                .When(() => string.IsNullOrWhiteSpace(RoleName), () => "Укажите название роли");
        }

        public string Title { get; private set; }

        public string RoleName
        {
            get { return _roleName; }
            set
            {
                if (_roleName == value)
                    return;
                _roleName = value;
                OnPropertyChanged(() => RoleName);
            }
        }

        public ObservableCollection<RightViewModel> Rights { get; } = new ObservableCollection<RightViewModel>();

        public bool AtLeastOneRightChecked => Rights.Any(x => x.IsChecked);

        public IDelegateCommand OkCommand { get; }

        public void Initialize(RoleDto role, string title)
        {
            Title = title;

            foreach (var rightViewModel in Rights)
            {
                rightViewModel.Checked -= RightViewModelOnChecked;
            }

            Rights.Clear();

            var rights = _userManagementCenterOperations.GetRights();
            foreach (var right in rights)
            {
                var rightViewModel = new RightViewModel(right);
                rightViewModel.Checked += RightViewModelOnChecked;
                Rights.Add(rightViewModel);
            }
        }

        public RoleDto GetRole()
        {
            return new RoleDto
            {
                Name = RoleName
            };
        }

        private void RightViewModelOnChecked(object sender, EventArgs e)
        {
            OnPropertyChanged(() => AtLeastOneRightChecked);
        }

        private void InitializeRightsView()
        {
            RightViewModel rightViewModel;
            _rightsView = CollectionViewSource.GetDefaultView(Rights);
            _rightsView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(rightViewModel.Application)));
        }

        private bool CanOkCommandHandler()
        {
            return !HasErrors && !string.IsNullOrWhiteSpace(RoleName) && AtLeastOneRightChecked;
        }
    }
}