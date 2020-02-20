using System.Globalization;
using System.Text;
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

        public OrderLiteDto Model
        {
            get { return _model; }
        }

        public void Update(OrderLiteDto order)
        {
            _model = order;

            OnPropertyChanged();
        }

        public bool MatchBySearchString(string searchString)
        {
            if (Model.DocNumber != null && Model.DocNumber.Contains(searchString))
                return true;

            if (Model.CustomerName != null && Model.CustomerName.Contains(searchString))
                return true;

            if (Model.DoctorFullName != null && Model.DoctorFullName.Contains(searchString))
                return true;

            if (Model.PatientFnp != null && Model.PatientFnp.Contains(searchString))
                return true;

            if (Model.TechnicFullName != null && Model.TechnicFullName.Contains(searchString))
                return true;

            if (Model.Created.ToString(new CultureInfo("Ru-ru")).Contains(searchString))
                return true;

            return false;
        }
    }
}
