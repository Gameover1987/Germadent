using System;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Repository;
using Germadent.Client.Common.ViewModels;
using Germadent.Common.Logging;
using Germadent.Model;
using Germadent.Rms.App.ViewModels;

namespace Germadent.Rms.App.Views.DesignMock
{
    public class DesignMockDictionaryRepository : IDictionaryRepository
    {
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public DictionaryItemDto[] Items { get; }
        public event EventHandler<RepositoryChangedEventArgs<DictionaryItemDto>> Changed;
        public DictionaryItemDto[] GetItems(DictionaryType dictionary)
        {
            throw new NotImplementedException();
        }
    }

    public class DesignMockLogger : ILogger
    {
        public void Info(string message)
        {
            throw new NotImplementedException();
        }

        public void Error(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Fatal(Exception exception)
        {
            throw new NotImplementedException();
        }
    }

    public class DesignMockOrdersFilterViewModel : OrdersFilterViewModel
    {
        public DesignMockOrdersFilterViewModel() : base(new DesignMockDictionaryRepository(), new DesignMockLogger(), new DesignMockRmsServiceClient())
        {
            MillingCenter = true;
            Laboratory = true;
            PeriodBegin = DateTime.Now.AddDays(-30);
            PeriodEnd = DateTime.Now;
            Customer = "Какой то заказчик";
            Doctor = "Какой то сотрудник";
            Patient = "Какой то пациент";
            ShowOnlyMyOrders = true;
        }
    }
}
