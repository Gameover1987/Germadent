using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public class MillingCenterInfoWizardStepViewModel : ViewModelBase, IWizardStepViewModel
    {
        private string _customer;
        private string _patientFio;
        private string _technicFio;
        private string _technicPhone;
        private DateTime _created;

        public MillingCenterInfoWizardStepViewModel()
        {
            
        }

        public string DisplayName
        {
            get { return "Общие данные"; }
        }

        public string Customer
        {
            get { return _customer; }
            set { SetProperty(() => _customer, value); }
        }

        public string PatientFio
        {
            get { return _patientFio; }
            set { SetProperty(() => _patientFio, value); }
        }

        public string TechnicFio
        {
            get { return _technicFio; }
            set { SetProperty(() => _technicFio, value); }
        }

        public string TechnicPhoneNumber
        {
            get { return _technicPhone; }
            set { SetProperty(() => _technicPhone, value); }
        }

        public DateTime Created
        {
            get { return _created; }
            set { SetProperty(() => _created, value); }
        }

        public void Initialize(Order order)
        {
            
        }
    }
}
