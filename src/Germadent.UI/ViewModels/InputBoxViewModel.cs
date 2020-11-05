using System;
using System.Collections.Generic;
using System.Text;
using Germadent.UI.Commands;

namespace Germadent.UI.ViewModels
{
    public class InputBoxViewModel : ViewModelBase
    {
        private string _inputString;

        public InputBoxViewModel()
        {
            OKCommand = new DelegateCommand(OkCommandHandler, CanOkCommandHandler);
        }

        public string Title { get; set; }

        public string ParameterName { get; set; }

        public string InputString
        {
            get { return _inputString; }
            set
            {
                if (_inputString == value)
                    return;
                _inputString = value;
                OnPropertyChanged(() => InputString);
            }
        }

        public IDelegateCommand OKCommand { get; }

        private void OkCommandHandler()
        {

        }

        private bool CanOkCommandHandler()
        {
            return !string.IsNullOrEmpty(InputString);
        }
    }
}
