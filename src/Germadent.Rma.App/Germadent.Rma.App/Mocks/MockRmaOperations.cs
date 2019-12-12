using System;
using System.Linq;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Mocks
{
    public class MockRmaOperations : IRmaOperations
    {
        public OrderLite[] GetOrders(OrdersFilter ordersFilter = null)
        {
            return new OrderLite[0];
        }

        public Order GetOrderDetails(int id)
        {
            throw new NotImplementedException();
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

        public Order AddOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public Order UpdateOrder(Order order)
        {
            throw new NotImplementedException();
        }
    }
}