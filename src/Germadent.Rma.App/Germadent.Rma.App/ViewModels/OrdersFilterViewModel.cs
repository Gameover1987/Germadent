using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.ServiceClient.Operation;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public interface IOrdersFilterViewModel { }

    public class OrdersFilterViewModel : ViewModelBase, IOrdersFilterViewModel
    {
        private readonly IRmaOperations _rmaOperations;

        public OrdersFilterViewModel(IRmaOperations rmaOperations)
        {
            _rmaOperations = rmaOperations;
        }

        public DateTime Date { get; set; }

        public string Customer { get; set; }

        public string Employee { get; set; }

        public string Patient { get; set; }

        public ObservableCollection<MaterialViewModel> Materials { get; } = new ObservableCollection<MaterialViewModel>();

    }
}
