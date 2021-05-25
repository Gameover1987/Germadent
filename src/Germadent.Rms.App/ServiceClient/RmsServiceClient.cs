using System.Linq;
using Germadent.Client.Common.Configuration;
using Germadent.Client.Common.ServiceClient;
using Germadent.Client.Common.ServiceClient.Notifications;
using Germadent.Common;
using Germadent.Common.Web;
using Germadent.Model;
using Germadent.Model.Production;
using Germadent.Model.Rights;
using RestSharp;

namespace Germadent.Rms.App.ServiceClient
{
    public class RmsServiceClient : BaseClientOperationsServiceClient, IRmsServiceClient
    {
        public RmsServiceClient(IClientConfiguration configuration, ISignalRClient signalRClient)
            : base(configuration, signalRClient)
        {
        }

        public WorkDto[] GetWorksByWorkOrder(int workOrderId)
        {
            return ExecuteHttpGet<WorkDto[]>(Configuration.DataServiceUrl + $"/api/Rms/OrdersProcessing/GetWorksByWorkOrder/{workOrderId}/{AuthorizationInfo.UserId}");
        }

        public WorkDto[] GetWorksInProgressByWorkOrder(int workOrderId)
        {
            return ExecuteHttpGet<WorkDto[]>(Configuration.DataServiceUrl + $"/api/Rms/OrdersProcessing/GetWorksInProgressByWorkOrder/{workOrderId}/{AuthorizationInfo.UserId}");
        }

        public void StartWorks(WorkDto[] works)
        {
            ExecuteHttpPost<WorkDto[]>(Configuration.DataServiceUrl + $"/api/Rms/OrdersProcessing/StartWorks", works);
        }

        public void FinishWorks(WorkDto[] works)
        {
            ExecuteHttpPost<WorkDto[]>(Configuration.DataServiceUrl + $"/api/Rms/OrdersProcessing/FinishWorks", works);
        }

        protected override bool CheckRunApplicationRight()
        {
            return AuthorizationInfo.Rights.Any(x => x.RightName == RmsUserRights.RunApplication);
        }
    }
}
