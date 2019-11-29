using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Common.Extensions;
using Germadent.Rma.Model;
using Nancy;
using Nancy.ModelBinding;
using Newtonsoft.Json;

namespace Germadent.DataAccessService
{
    public class OrdersModule : NancyModule
    {
        private List<Order> _orders = new List<Order>();

        public OrdersModule()
        {
            FillOrders();

            ModulePath = "api/Orders";

            Get("/", GetOrders);
            Post("/", GetOrdersByFilter);
        }

        private void FillOrders()
        {
            var orders = new Order[]
            {
                new Order
                {
                    Created = DateTime.Now,
                    Patient = "Иванов Иван Иванович",
                    
                    Customer = "ООО Рога и Копыта",
                    
                }
            };

            for (int i = 0; i < orders.Length; i++)
            {
                var order = orders[i];
                order.Number = i;
                if (i % 2 == 0)
                {
                    order.BranchType = BranchType.Laboratory;
                    order.Closed = DateTime.Now;
                }
            }

            _orders.Clear();
            _orders.AddRange(orders);
        }

        private object GetOrders(object arg)
        {
            return Response.AsJson(_orders);
        }

        private object GetOrdersByFilter(object arg)
        {
            var filter = this.Bind<OrdersFilter>();

            return Response.AsJson(_orders);
        }
    }
}