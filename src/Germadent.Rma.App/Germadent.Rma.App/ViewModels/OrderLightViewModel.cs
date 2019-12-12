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
    }
}
