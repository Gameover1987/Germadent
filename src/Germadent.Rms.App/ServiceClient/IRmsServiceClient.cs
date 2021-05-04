using Germadent.Model;
using Germadent.UserManagementCenter.Model;

namespace Germadent.Rms.App.ServiceClient
{
    public interface IRmsServiceClient
    {
        void Authorize(string login, string password);
        OrderLiteDto[] GetOrders(OrdersFilter filter);
        OrderDto GetOrderById(int workOrderId);
        AuthorizationInfoDto AuthorizationInfo { get; set; }
    }
}