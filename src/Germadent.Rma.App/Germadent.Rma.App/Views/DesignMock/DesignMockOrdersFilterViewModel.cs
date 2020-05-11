using System;
using Germadent.Rma.App.Mocks;
using Germadent.Rma.App.ViewModels;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockOrdersFilterViewModel : OrdersFilterViewModel
    {
        public DesignMockOrdersFilterViewModel() : base(new DesignMockRmaServiceClient(), new MockLogger())
        {
            MillingCenter = true;
            Laboratory = true;
            PeriodBegin = DateTime.Now.AddDays(-30);
            PeriodEnd = DateTime.Now;
            Customer = "Какой то заказчик";
            Employee = "Какой то сотрудник";
            Patient = "Какой то пациент";
        }
    }
}
