using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Germadent.Common.Extensions;
using Germadent.Model;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels.Validation;
using Germadent.UserManagementCenter.App.ServiceClient;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public class AddRoleViewModel : ValidationSupportableViewModel, IAddRoleViewModel
    {
        private readonly IUmcServiceClient _umcServiceClient;
        private string _roleName;
        private int _roleId;

        private ICollectionView _rightsView;

        public AddRoleViewModel(IUmcServiceClient umcServiceClient)
        {
            _umcServiceClient = umcServiceClient;

            _rightsView = CollectionViewSource.GetDefaultView(Rights);
            _rightsView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(RightViewModel.ApplicationModule)));

            OkCommand = new DelegateCommand(() => { }, CanOkCommandHandler);

            AddValidationFor(() => RoleName)
                .When(() => string.IsNullOrWhiteSpace(RoleName), () => "Укажите название роли");
        }

        public string Title { get; private set; }

        public ViewMode ViewMode { get; private set; }

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

        public void Initialize(RoleDto role, ViewMode viewMode)
        {
            ViewMode = viewMode;
            if (ViewMode == ViewMode.Add)
            {
                Title = "Добавление роли";
            }
            else
            {
                Title = "Редактирование данных роли";
            }

            _roleName = role.RoleName;
            _roleId = role.RoleId;

            foreach (var rightViewModel in Rights)
            {
                rightViewModel.Checked -= RightViewModelOnChecked;
            }

            Rights.Clear();

            var rights = _umcServiceClient.GetRights()
                .OrderBy(x => x.ApplicationModule.GetDescription())
                .ThenBy(x => x.RightDescription)
                .ToArray();
            foreach (var right in rights)
            {
                var rightViewModel = new RightViewModel(right);
                rightViewModel.Checked += RightViewModelOnChecked;
                Rights.Add(rightViewModel);
            }

            if (role.Rights == null)
                return;

            foreach (var roleRight in role.Rights)
            {
                var rightViewModel = Rights.First(x => x.Name == roleRight.RightName && x.ApplicationModule == roleRight.ApplicationModule);
                rightViewModel.IsChecked = true;
            }
        }

        public RoleDto GetRole()
        {
            return new RoleDto
            {
                RoleId = _roleId,
                RoleName = RoleName,
                Rights = Rights.Where(x => x.IsChecked).Select(x => x.ToDto()).ToArray()
            };
        }

        private void RightViewModelOnChecked(object sender, EventArgs e)
        {
            OnPropertyChanged(() => AtLeastOneRightChecked);
        }

        private bool CanOkCommandHandler()
        {
            return !HasErrors && !string.IsNullOrWhiteSpace(RoleName) && AtLeastOneRightChecked;
        }
    }
}