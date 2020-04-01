﻿using Germadent.UI.ViewModels;
using Germadent.UserManagementCenter.Model.Rights;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public class RightViewModel : ViewModelBase
    {
        private readonly RightDto _right;

        private bool _isEnabled;

        public RightViewModel(RightDto right)
        {
            _right = right;
        }

        public string Name => _right.RightName;

        public string Application => _right.ApplicationName;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled == value)
                    return;
                _isEnabled = value;
                OnPropertyChanged(() => IsEnabled);
            }
        }
    }
}