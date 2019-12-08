using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public class OrderLiteViewModel : ViewModelBase
    {
        private OrderLite _model;

        public OrderLiteViewModel(OrderLite model)
        {
            _model = model;
        }

        public OrderLite Model
        {
            get { return _model; }
        }

        public void Update(OrderLite order)
        {
            _model = order;

            OnPropertyChanged();
        }
    }
}
