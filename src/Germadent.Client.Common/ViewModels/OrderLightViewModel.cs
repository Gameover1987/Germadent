using System;
using System.Globalization;
using Germadent.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Client.Common.ViewModels
{
    public class OrderLiteViewModel : ViewModelBase
    {
        private OrderLiteDto _dto;

        public OrderLiteViewModel(OrderLiteDto dto)
        {
            _dto = dto;
        }

        public int WorkOrderId => _dto.WorkOrderId;

        public BranchType BranchType => _dto.BranchType;

        public string DocNumber => _dto.DocNumber;

        public string CustomerName => _dto.CustomerName;

        public string DoctorFullName => _dto.DoctorFullName;

        public string PatientFullName => _dto.PatientFnp;

        public string TechnicFullName => _dto.TechnicFullName;

        public DateTime Created => _dto.Created;

        public string CreatorFullName => _dto.CreatorFullName;

        public OrderStatus Status
        {
            get { return _dto.Status; }
            set
            {
                if (_dto.Status == value)
                    return;
                _dto.Status = value;
                OnPropertyChanged(() => Status);
            }
        }

        public DateTime StatusChanged
        {
            get { return _dto.StatusChanged; }
            set
            {
                if (_dto.StatusChanged == value)
                    return;
                _dto.StatusChanged = value;
                OnPropertyChanged(() => StatusChanged);
            }
        }

        public bool IsClosed => _dto.Status == OrderStatus.Closed;

        public bool IsLocked => _dto.LockedBy != null;

        public DateTime? LockDate
        {
            get { return _dto.LockDate; }
            set
            {
                if (_dto.LockDate == value)
                    return;
                _dto.LockDate = value;
                OnPropertyChanged(() => LockDate);
            }
        }

        public UserDto LockedBy
        {
            get { return _dto.LockedBy; }
            set
            {
                if (_dto.LockedBy == value)
                    return;
                _dto.LockedBy = value;
                OnPropertyChanged(() => LockedBy);
                OnPropertyChanged(() => LockInfo);
            }
        } 

        public string LockInfo
        {
            get
            {
                if (_dto.LockedBy == null)
                    return null;

                return string.Format("{0} {1}", _dto.LockedBy.GetFullName(), _dto.LockDate);
            }
        }

        public void Update(OrderLiteDto order)
        {
            _dto = order;

            OnPropertyChanged();
        }

        public bool MatchBySearchString(string searchString)
        {
            searchString = searchString.ToLower();
            if (DocNumber != null && DocNumber.ToLower().Contains(searchString))
                return true;

            if (CustomerName != null && CustomerName.ToLower().Contains(searchString))
                return true;

            if (DoctorFullName != null && DoctorFullName.ToLower().Contains(searchString))
                return true;

            if (PatientFullName != null && PatientFullName.ToLower().Contains(searchString))
                return true;

            if (TechnicFullName != null && TechnicFullName.ToLower().Contains(searchString))
                return true;

            if (Created.ToString(new CultureInfo("Ru-ru")).ToLower().Contains(searchString))
                return true;

            return false;
        }
    }
}
