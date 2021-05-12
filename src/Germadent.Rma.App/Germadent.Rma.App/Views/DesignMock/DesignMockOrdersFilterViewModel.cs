using System;
using Germadent.Client.Common.ViewModels;
using Germadent.Rma.App.ViewModels;
using OrdersFilterViewModel = Germadent.Rma.App.ViewModels.OrdersFilterViewModel;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockOrdersFilterViewModel : OrdersFilterViewModel
    {
        public DesignMockOrdersFilterViewModel() : base(new DesignMockDictionaryRepository(), new MockLogger())
        {
            MillingCenter = true;
            Laboratory = true;
            PeriodBegin = DateTime.Now.AddDays(-30);
            PeriodEnd = DateTime.Now;
            Customer = "Какой то заказчик";
            Doctor = "Какой то сотрудник";
            Patient = "Какой то пациент";
        }
    }
}
