using System.Globalization;
using Germadent.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Client.Common.ViewModels
{
    public class OrderLiteViewModel : ViewModelBase
    {
        private OrderLiteDto _model;

        public OrderLiteViewModel(OrderLiteDto model)
        {
            _model = model;
        }

        public OrderLiteDto Model => _model;

        public bool IsClosed => _model.Closed != null;

        public bool IsLocked => _model.LockedBy != null;

        public UserDto LockedBy => _model.LockedBy;

        public string LockInfo
        {
            get
            {
                if (_model.LockedBy == null)
                    return null;

                return string.Format("{0} {1}", _model.LockedBy.GetFullName(), _model.LockDate);
            }
        }

        public void Update(OrderLiteDto order)
        {
            _model = order;

            OnPropertyChanged();
        }

        public bool MatchBySearchString(string searchString)
        {
            searchString = searchString.ToLower();
            if (Model.DocNumber != null && Model.DocNumber.ToLower().Contains(searchString))
                return true;

            if (Model.CustomerName != null && Model.CustomerName.ToLower().Contains(searchString))
                return true;

            if (Model.DoctorFullName != null && Model.DoctorFullName.ToLower().Contains(searchString))
                return true;

            if (Model.PatientFnp != null && Model.PatientFnp.ToLower().Contains(searchString))
                return true;

            if (Model.TechnicFullName != null && Model.TechnicFullName.ToLower().Contains(searchString))
                return true;

            if (Model.Created.ToString(new CultureInfo("Ru-ru")).ToLower().Contains(searchString))
                return true;

            return false;
        }
    }
}
