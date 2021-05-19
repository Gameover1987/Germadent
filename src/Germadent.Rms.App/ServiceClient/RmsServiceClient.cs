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

        public TechnologyOperationByUserDto[] GetRelevantWorkListByWorkOrder(int workOrderId)
        {
            return ExecuteHttpGet<TechnologyOperationByUserDto[]>(Configuration.DataServiceUrl + $"/api/Rms/OrdersProcessing/GetRelevantOperationsByWorkOrder/{workOrderId}/{AuthorizationInfo.UserId}");
        }

        public void StartWork(WorkDto work, int lastEditorId)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateWork(WorkDto work, int lastEditorId)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteWork(WorkDto work)
        {
            throw new System.NotImplementedException();
        }

        protected override bool CheckRunApplicationRight()
        {
            return AuthorizationInfo.Rights.Any(x => x.RightName == RmsUserRights.RunApplication);
        }
    }
}
