using System;
using Germadent.Model.Production;
using Germadent.UI.ViewModels;

namespace Germadent.Client.Common.ViewModels
{
    public class WorkViewModel : ViewModelBase
    {
        private readonly WorkDto _work;
        private bool _isChecked;

        public WorkViewModel(WorkDto work)
        {
            _work = work;
        }

        public int WorkId => _work.WorkId;

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

        public string UserFullName => _work.UserFullNameStarted;

        public string TechnologyOperationName => _work.TechnologyOperationName;

        public string TechnologyOperationUserCode => _work.TechnologyOperationUserCode;

        public int Quantity => _work.Quantity;

        public DateTime WorkStarted => _work.WorkStarted;

        public DateTime? WorkCompleted => _work.WorkCompleted;

        public string CustomerName => _work.CustomerName;

        public string PatientFullName => _work.PatientFullName;

        public int WorkOrderId => _work.WorkOrderId;

        public string DocNumber => _work.DocNumber;

        public decimal OperationCost => _work.OperationCost;

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