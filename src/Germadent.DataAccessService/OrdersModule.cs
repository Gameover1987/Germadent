using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Common.Extensions;
using Germadent.Rma.Model;
using Nancy;
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
        }

        private void FillOrders()
        {
            var orders = new Order[]
            {
                new Order
                {
                    Created = DateTime.Now,
                    Patient = "Иванов Иван Иванович",
                    Employee = "Петров Петр Петрович",
                    Customer = "ООО Рога и Копыта",
                    Material = "ZrO"
                },
                new Order
                {
                    Created = DateTime.Now,
                    Patient = "Сергей Сергеевич Серегин",
                    Employee = "Петров Петр Петрович",
                    Customer = "ООО Рога и Копыта",
                    Material = "PMMA multi"
                },
                new Order
                {
                    Created = DateTime.Now,
                    Patient = "Антон Антонович Антонов",
                    Employee = "Петров Петр Петрович",
                    Customer = "ООО Рога и Копыта",
                    Material = "PMMA mono"
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
            return _orders.SerializeToJson();
        }
    }
}