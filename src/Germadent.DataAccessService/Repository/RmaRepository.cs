using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Germadent.DataAccessService.Configuration;
using Germadent.DataAccessService.Entities;
using Germadent.DataAccessService.Entities.Conversion;
using Germadent.Rma.Model;

namespace Germadent.DataAccessService.Repository
{
    public class RmaRepository : IRmaRepository
    {
        private readonly IEntityToDtoConverter _converter;
        private readonly IServiceConfiguration _configuration;

        public RmaRepository(IEntityToDtoConverter converter, IServiceConfiguration configuration)
        {
            _converter = converter;
            _configuration = configuration;
        }

        public void AddOrder(OrderDto order)
        {
            
        }

        public void UpdateOrder(OrderDto order)
        {
            throw new NotImplementedException();
        }

        public MaterialDto[] GetMaterials()
        {
            var cmdText = "select * from GetMaterialsList()";

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var materials = new List<MaterialEntity>();
                    while (reader.Read())
                    {
                        var materialEntity = new MaterialEntity();
                        materialEntity.MaterialId = int.Parse(reader[nameof(materialEntity.MaterialId)].ToString());
                        materialEntity.MaterialName = reader[nameof(materialEntity.MaterialName)].ToString();

                        if (bool.TryParse(reader[nameof(materialEntity.FlagUnused)].ToString(), out bool flagUnused))
                        {
                            materialEntity.FlagUnused = flagUnused;
                        }

                        materials.Add(materialEntity);
                    }
                    reader.Close();

                    return materials.Select(x => _converter.ConvertToMaterial(x)).ToArray();
                }
            }
        }

        public OrderDto GetOrderDetails(int id)
        {
            var cmdText = string.Format("select * from GetWorkOrderById({0})", id);

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var orderEntity = new OrderEntity();
                    while (reader.Read())
                    {
                        orderEntity.WorkOrderId = int.Parse(reader[nameof(orderEntity.WorkOrderId)].ToString());
                    }
                    reader.Close();

                    return _converter.ConvertToOrder(orderEntity);
                }
            }
        }

        public OrderLiteDto[] GetOrders(OrdersFilter filter)
        {
            var cmdText = "select * from GetWorkOrdersList(default, default, default, default, default, default, default, default, default, default)";

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var orderLiteEntities = new List<OrderLiteEntity>();
                    while (reader.Read())
                    {
                        var orderLite = new OrderLiteEntity();
                        orderLite.BranchTypeId = int.Parse(reader[nameof(orderLite.BranchTypeId)].ToString());
                        orderLite.CustomerName = reader[nameof(orderLite.CustomerName)].ToString();
                        orderLite.ResponsiblePersonName = reader[nameof(orderLite.ResponsiblePersonName)].ToString();
                        orderLite.PatientFnp = reader[nameof(orderLite.PatientFnp)].ToString();
                        orderLite.DocNumber = reader[nameof(orderLite.DocNumber)].ToString();
                        orderLite.Created = DateTime.Parse(reader[nameof(orderLite.Created)].ToString());

                        var closed = reader[nameof(orderLite.Closed)];
                        if (closed != DBNull.Value)
                            orderLite.Closed = DateTime.Parse(closed.ToString());

                        orderLiteEntities.Add(orderLite);
                    }
                    reader.Close();

                    var orders = orderLiteEntities.Select(x => _converter.ConvertToOrderLite(x)).ToArray();
                    return orders;
                }
            }
        }
    }
}
