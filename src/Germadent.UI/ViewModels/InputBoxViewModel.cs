using System;
using System.Collections.Generic;
using System.Text;

namespace Germadent.UI.ViewModels
{
    public class InputBoxViewModel : ViewModelBase
    {
        private string _inputString;

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
    }
}
