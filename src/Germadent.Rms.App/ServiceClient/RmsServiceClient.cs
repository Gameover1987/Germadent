using System;
using Germadent.Model;

namespace Germadent.Rms.App.ServiceClient
{
    public class RmsServiceClient : IRmsServiceClient
    {
        public void Authorize(string login, string password)
        {
            AuthorizationInfo = new AuthorizationInfoDto {Login = login};
        }

        public OrderLiteDto[] GetOrders(OrdersFilter filter)
        {
            return new OrderLiteDto[]
            {
                new OrderLiteDto{BranchType = BranchType.Laboratory},
                new OrderLiteDto{BranchType = BranchType.MillingCenter},
            };
        }

        public OrderDto GetOrderById(int workOrderId)
        {
            throw new NotImplementedException();
        }

        public AuthorizationInfoDto AuthorizationInfo { get; set; }
    }
}
