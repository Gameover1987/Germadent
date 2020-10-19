using System;
using Germadent.UI.ViewModels;
using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public class RoleViewModel : ViewModelBase
    {
        private RoleDto _role;
        private bool _isChecked;

        public RoleViewModel(RoleDto role)
        {
            _role = role;
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked == value)
                    return;
                _isChecked = value;
                OnPropertyChanged(() => IsChecked);

                Checked?.Invoke(this, EventArgs.Empty);
            }
        }

        public int RoleId => _role.RoleId;

        public string Name => _role.RoleName;


        public event EventHandler<EventArgs> Checked;

        public RoleDto ToModel()
        {
            return _role;
        }

        public void Update(RoleDto role)
        {
            _role = role;
            OnPropertyChanged();
        }
    }
}