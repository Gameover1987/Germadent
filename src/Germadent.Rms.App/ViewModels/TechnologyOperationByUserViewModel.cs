using Germadent.Model.Production;
using Germadent.UI.ViewModels;

namespace Germadent.Rms.App.ViewModels
{
    public class TechnologyOperationByUserViewModel : ViewModelBase
    {
        private readonly TechnologyOperationByUserDto _operationByUser;

        private bool _isChecked;

        public TechnologyOperationByUserViewModel(TechnologyOperationByUserDto operationByUser)
        {
            _operationByUser = operationByUser;
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

        public string DisplayName
        {
            get { return _operationByUser.Operation.Name; }
            set{}
        }

        public int ProductCount => _operationByUser.ProductCount;

        public decimal Rate => _operationByUser.Rate;

        public float UrgencyRatio => _operationByUser.UrgencyRatio;

        public decimal TotalCost => _operationByUser.TotalCost;

        public TechnologyOperationByUserDto ToDto() => _operationByUser;
    }
}