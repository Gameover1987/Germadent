using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public class OrderLightViewModel : ViewModelBase
    {
        private Order _model;

        public OrderLightViewModel(Order model)
        {
            _model = model;
        }

        public Order Model
        {
            get { return _model; }
        }

        public void Update(Order order)
        {
            _model = order;

            OnPropertyChanged();
        }
    }
}
