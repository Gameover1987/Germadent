using Germadent.DataAccessService.Entitties;
using Germadent.Rma.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.DataAccessService
{

    public class RmaRepository : IRmaRepository
    {
        private const string _connectionString = @"Data Source=.\GAMEOVERSQL;Initial Catalog=Germadent;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private readonly IEntityToDtoConverter _converter;

        public RmaRepository(IEntityToDtoConverter converter)
        {
            _converter = converter;
        }

        public void AddLabOrder(LaboratoryOrder laboratoryOrder)
        {
            throw new NotImplementedException();
        }

        public void AddMcOrder(MillingCenterOrder millingCenterOrder)
        {
            throw new NotImplementedException();
        }

        public Material[] GetMaterials()
        {
            return new Material[]
            {
                new Material{Name = "ZrO"},
                new Material{Name = "PMMA mono"},
                new Material{Name = "PMMA multi"},
                new Material{Name = "WAX"},
                new Material{Name = "MIK"},

                new Material{Name = "CAD-Temp mono"},
                new Material{Name = "CAD-Temp multi"},
                new Material{Name = "Enamic mono"},
                new Material{Name = "Enamic multi"},
                new Material{Name = "SUPRINITY"},

                new Material{Name = "MARK II"},
                new Material{Name = "TriLuxe forte"},
                new Material{Name = "Ti"},
                new Material{Name = "E.Max"},
            };
        }

        public Order GetOrderDetails(int id)
        {
            throw new NotImplementedException();
        }

        public OrderLite[] GetOrders(OrdersFilter filter)
        {
            var builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = _connectionString;
            var cmdText = "select * from GetWorkOrdersList(default, default, default, default, default, default, default, default, default, default)";

            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    Console.WriteLine("{0}\t{1}\t{2}", reader.GetName(0), reader.GetName(2), reader.GetName(3));
                    var orders = new List<OrderLiteEntity>();
                    while (reader.Read())
                    {
                        var orderLite = new OrderLiteEntity();
                        orderLite.BranchType = reader[nameof(orderLite.BranchType)].ToString();
                        orderLite.BranchTypeId = int.Parse(reader[nameof(orderLite.BranchTypeId)].ToString());
                        orderLite.CustomerName = reader[nameof(orderLite.CustomerName)].ToString();
                        orderLite.ResponsiblePersonName = reader[nameof(orderLite.ResponsiblePersonName)].ToString();
                        orderLite.PatientFnp = reader[nameof(orderLite.PatientFnp)].ToString();
                        orderLite.Created = DateTime.Parse(reader[nameof(orderLite.Created)].ToString());

                        var closed = reader[nameof(orderLite.Closed)];
                        if (closed != DBNull.Value)
                            orderLite.Closed = DateTime.Parse(closed.ToString());

                        orders.Add(orderLite);
                    }
                    reader.Close();

                    return orders.Select(x => _converter.ConvertFrom(x)).ToArray();                    
                }
            }
        }     

        public void UpdateLabOrder(LaboratoryOrder laboratoryOrder)
        {
            throw new NotImplementedException();
        }
    }
}
