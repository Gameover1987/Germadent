using System.Collections.Generic;
using System.IO;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Rma.Model;
using Newtonsoft.Json;

namespace Germadent.DataAccessService
{
    public class OrdersRepository : IOrdersRepository
    {
        private const string DataFileName = " DataFile.txt";

        private readonly List<Order> _orders = new List<Order>();

        public OrdersRepository()
        {
            if (!File.Exists(DataFileName))
                return;

            _orders = new List<Order>(File.ReadAllText(DataFileName).DeserializeFromJson<Order[]>());
        }

        public Order GetOrderDetails(int id)
        {
            return _orders.FirstOrDefault(x => x.Id == id);
        }

        public Order[] GetOrders(OrdersFilter filter)
        {
            if (filter.IsNullOrEmpty())
                return _orders.ToArray();

            return _orders.Where(x => x.MatchByFilter(filter)).ToArray();
        }

        public void AddOrder(Order order)
        {
            _orders.Add(order);

            SaveOrdersToFile();
        }

        private void SaveOrdersToFile()
        {
            File.WriteAllText(DataFileName, _orders.SerializeToJson(Formatting.Indented));
        }
    }
}