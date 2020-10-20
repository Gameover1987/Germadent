using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;
using Germadent.UserManagementCenter.App.ServiceClient;
using Germadent.UserManagementCenter.App.UIOperations;
using Germadent.UserManagementCenter.Model.Rights;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public class RolesManagerViewModel : ViewModelBase, IRolesManagerViewModel
    {
        private readonly IUmcServiceClient _umcServiceClient;
        private readonly IUserManagementUIOperations _userManagementUIOperations;
        private RoleViewModel _selectedRole;

        private ICollectionView _rightsView;

        public RolesManagerViewModel(IUmcServiceClient umcServiceClient, IUserManagementUIOperations userManagementUIOperations)
        {
            _umcServiceClient = umcServiceClient;
            _userManagementUIOperations = userManagementUIOperations;

            AddRoleCommand = new DelegateCommand(AddRoleCommandHandler, CanAddRoleCommandHandler);
            EditRoleCommand = new DelegateCommand(EditRoleCommandHandler, CanEditRoleCommandHandler);
            DeleteRoleCommand = new DelegateCommand(DeleteRoleCommandHandler, CanDeleteRoleCommandHandler);

            InitializeRightsView();
        }

        public ObservableCollection<RoleViewModel> Roles { get; } = new ObservableCollection<RoleViewModel>();

        public RoleViewModel SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                if (_selectedRole == value)
                    return;
                _selectedRole = value;
                OnPropertyChanged(() => SelectedRole);

                LoadRightsByRole();
            }
        }

        public ObservableCollection<RightViewModel> Rights { get; } = new ObservableCollection<RightViewModel>();

        public IDelegateCommand AddRoleCommand { get; }

        public IDelegateCommand EditRoleCommand { get; }

        public IDelegateCommand DeleteRoleCommand { get; }

        public void Initialize()
        {
            var roles = _umcServiceClient.GetRoles();

            Roles.Clear();
            foreach (var role in roles)
            {
                Roles.Add(new RoleViewModel(role));
            }
        }

        private void InitializeRightsView()
        {
            var rightViewModel = new RightViewModel(new RightDto());
            _rightsView = CollectionViewSource.GetDefaultView(Rights);
            _rightsView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(rightViewModel.Application)));
        }

        private void LoadRightsByRole()
        {
            Rights.Clear();

            if (_selectedRole == null)
                return;

            var rightsByRole = SelectedRole.ToModel().Rights;
            foreach (var rightDto in rightsByRole)
            {
                Rights.Add(new RightViewModel(rightDto));
            }
        }

        private bool CanAddRoleCommandHandler()
        {
            return true;
        }

        private void AddRoleCommandHandler()
        {
            var role = _userManagementUIOperations.AddRole();
            if (role == null)
                return;

            role = _umcServiceClient.AddRole(role);
            var roleViewModel = new RoleViewModel(role);
            Roles.Add(roleViewModel);
            SelectedRole = roleViewModel;
        }

        private bool CanEditRoleCommandHandler()
        {
            return SelectedRole != null;
        }

        private void EditRoleCommandHandler()
        {
            var role = _userManagementUIOperations.EditRole(SelectedRole);
            if (role == null)
                return;

            role = _umcServiceClient.EditRole(role);
            SelectedRole.Update(role);
            LoadRightsByRole();
        }

        private bool CanDeleteRoleCommandHandler()
        {
            return SelectedRole != null;
        }

        private void DeleteRoleCommandHandler()
        {
           if (_userManagementUIOperations.DeleteRole(SelectedRole) == false)
               return;

           Roles.Remove(SelectedRole);
        }
    }
}