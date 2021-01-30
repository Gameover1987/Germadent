using System.Globalization;
using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
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
