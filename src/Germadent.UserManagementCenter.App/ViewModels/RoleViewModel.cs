﻿using Germadent.UI.ViewModels;
using Germadent.UserManagementCenter.Model;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public class RoleViewModel : ViewModelBase
    {
        private readonly RoleDto _role;
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
            }
        }

        public int RoleId => _role.RoleId;

        public string Name => _role.Name;
    }
}