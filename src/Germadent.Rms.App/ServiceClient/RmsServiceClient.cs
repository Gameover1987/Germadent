using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Model;
using Germadent.UserManagementCenter.Model;

namespace Germadent.Rms.App.ServiceClient
{
    public class RmsServiceClient : IRmsServiceClient
    {
        public void Authorize(string login, string password)
        {
            throw new NotImplementedException();
        }

        public OrderLiteDto[] GetOrders(OrdersFilter filter)
        {
            throw new NotImplementedException();
        }

        public OrderDto GetOrderById(int workOrderId)
        {
            throw new NotImplementedException();
        }

        public AuthorizationInfoDto AuthorizationInfo { get; set; }
    }
}
