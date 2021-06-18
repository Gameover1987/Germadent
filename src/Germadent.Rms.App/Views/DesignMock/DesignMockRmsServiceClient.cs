using System;
using Germadent.Client.Common.Configuration;
using Germadent.Client.Common.ServiceClient;
using Germadent.Model;
using Germadent.Model.Production;
using Germadent.Rms.App.ServiceClient;

namespace Germadent.Rms.App.Views.DesignMock
{
    public class DesignMockRmsServiceClient : IRmsServiceClient
    {
        public WorkDto[] GetWorksByWorkOrder(int workOrderId)
        {
            return new WorkDto[0];
        }

        public WorkDto[] GetWorksInProgressByWorkOrder(int workOrderId)
        {
            throw new NotImplementedException();
        }

        public void StartWorks(WorkDto[] works)
        {

        }

        public void FinishWorks(WorkDto[] works)
        {
            throw new NotImplementedException();
        }

        public void PerformQualityControl(int workOrderId)
        {
            throw new NotImplementedException();
        }


        public AuthorizationInfoDto AuthorizationInfo { get; }
        public IClientConfiguration Configuration { get; }
        public void Authorize(string login, string password)
        {
            throw new NotImplementedException();
        }

        public OrderLiteDto[] GetOrders(OrdersFilter filter)
        {
            throw new NotImplementedException();
        }

        public OrderScope GetOrderById(int workOrderId)
        {
            throw new NotImplementedException();
        }

        public void UnLockOrder(int workOrderId)
        {
            throw new NotImplementedException();
        }

        public DictionaryItemDto[] GetDictionary(DictionaryType dictionaryType)
        {
            throw new NotImplementedException();
        }

        public byte[] GetTemplate(DocumentTemplateType documentTemplateType)
        {
            throw new NotImplementedException();
        }
    }
}