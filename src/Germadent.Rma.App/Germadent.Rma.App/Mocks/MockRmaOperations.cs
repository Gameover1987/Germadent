using System;
using System.Linq;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Mocks
{
    public class MockRmaOperations : IRmaOperations
    {
        public Order[] GetOrders(OrdersFilter ordersFilter = null)
        {
            var orders = new Order[]
            {
                new Order
                {
                    Created = DateTime.Now,
                    Patient = "Иванов Иван Иванович",
                    
                    Customer = "ООО Рога и Копыта",
                },
                new Order
                {
                    Created = DateTime.Now,
                    Patient = "Сергей Сергеевич Серегин",
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

            if (ordersFilter == null)
                return orders;

            if (ordersFilter.Laboratory)
                return orders.Where(x => x.BranchType == BranchType.Laboratory).ToArray();
            if (ordersFilter.MillingCenter)
                return orders.Where(x => x.BranchType == BranchType.MillingCenter).ToArray();

            return orders;
        }

        public Material[] GetMaterials()
        {
            var materials = new[]
            {
                new Material {Name = "ZrO"},
                new Material {Name = "PMMA mono"},
                new Material {Name = "PMMA multi"},
                new Material {Name = "WAX"},
                new Material {Name = "MIK"},
                new Material {Name = "CAD-Temp mono"},
                new Material {Name = "CAD-Temp multi"},
                new Material {Name = "Enamik mono"},
                new Material {Name = "Enamik multi"},
                new Material {Name = "SUPRINITY"},
                new Material {Name = "Mark II"},
                new Material {Name = "WAX"},
                new Material {Name = "TriLuxe forte"},
                new Material {Name = "Ti"},
                new Material {Name = "E.MAX"},
            };

            return materials;
        }

        public void AddOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public Order GetOrderDetails(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}