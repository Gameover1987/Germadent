using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Rma.App.ViewModels;
using Germadent.ServiceClient.Operation;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockOrdersFilterViewModel : OrdersFilterViewModel
    {
        public DesignMockOrdersFilterViewModel() : base(new MockRmaOperations())
        {
            Date = DateTime.Now;
            Customer = "Какой то заказчик";
            Employee = "Какой то сотрудник";
            Patient = "Какой то пациент";
        }
    }
}
