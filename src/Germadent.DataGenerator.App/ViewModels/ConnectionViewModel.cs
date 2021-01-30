using System;
using System.Windows.Input;
using Germadent.Common.Extensions;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;
using Germadent.UI.ViewModels.Validation;

namespace Germadent.DataGenerator.App.ViewModels
{
    public interface IConnectionViewModel { }

    public class ConnectionViewModel : ValidationSupportableViewModel, IConnectionViewModel
    {
        private string _address;
        private string _user;
        private string _password;
        private string _dbName;
        private bool _createNewDb;

        public ConnectionViewModel()
        {
            OKCommand = new DelegateCommand(CanOkCommandHandler);
        }

        private bool CanOkCommandHandler()
        {
            return !IsEmpty;
        }

        public bool IsEmpty
        {
            get
            {
                return Address.IsNullOrWhiteSpace() ||
                       User.IsNullOrWhiteSpace() ||
                       Password.IsNullOrWhiteSpace() ||
                       DbName.IsNullOrWhiteSpace();
            }
        }


        public string Address
        {
            get { return _address; }
            set
            {
                if (_address == value)
                    return;
                _address = value;
                OnPropertyChanged(() => Address);
                OnPropertyChanged(() => IsEmpty);
            }
        }

        public string User
        {
            get { return _user; }
            set
            {
                if (_user == value)
                    return;
                _user = value;
                OnPropertyChanged(() => User);
                OnPropertyChanged(() => IsEmpty);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value)
                    return;
                _password = value;
                OnPropertyChanged(() => Password);
                OnPropertyChanged(() => IsEmpty);
            }
        }

        public string DbName
        {
            get { return _dbName; }
            set
            {
                if (_dbName == value)
                    return;
                _dbName = value;
                OnPropertyChanged(() => DbName);
                OnPropertyChanged(() => IsEmpty);
            }
        }

        public bool CreateNewDb
        {
            get { return _createNewDb; }
            set
            {
                if (_createNewDb == value)
                    return;
                _createNewDb = value;
                OnPropertyChanged(() => CreateNewDb);
                OnPropertyChanged(() => IsEmpty);
            }
        }

        public ICommand OKCommand { get; }
    }
}
