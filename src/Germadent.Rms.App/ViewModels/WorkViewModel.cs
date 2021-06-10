using Germadent.Model.Production;
using Germadent.UI.ViewModels;

namespace Germadent.Rms.App.ViewModels
{
    public class WorkViewModel : ViewModelBase
    {
        private readonly WorkDto _work;

        private bool _isChecked;

        public WorkViewModel(WorkDto work)
        {
            _work = work;
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

        public string UserCode => _work.TechnologyOperationUserCode;

        public string DisplayName => _work.TechnologyOperationName;

        public int ProductCount => _work.Quantity;

        public decimal Rate => _work.Rate;

        public float UrgencyRatio => _work.UrgencyRatio;

        public decimal TotalCost => _work.OperationCost;
        public string ProductName => _work.ProductName;

        public string Comment
        {
            get => _work.Comment;
            set
            {
                if (_work.Comment == value)
                    return;
                _work.Comment = value;
                OnPropertyChanged(() => Comment);
            }
        }

        public WorkDto ToDto() => _work;
    }
}