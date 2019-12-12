﻿using Germadent.Rma.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Germadent.DataAccessService.Entities;

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

        public void UpdateOrder(Order laboratoryOrder)
        {
            throw new NotImplementedException();
        }

        public Material[] GetMaterials()
        {
            var cmdText = "select * from GetMaterialsList()";

            using (var connection = new SqlConnection(_connectionString))
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

        public Order GetOrderDetails(int id)
        {
            throw new NotImplementedException();
        }

        public OrderLite[] GetOrders(OrdersFilter filter)
        {
            var cmdText = "select * from GetWorkOrdersList(default, default, default, default, default, default, default, default, default, default)";

            using (var connection = new SqlConnection(_connectionString))
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

        public void AddOrder(Order laboratoryOrder)
        {
            throw new NotImplementedException();
        }
    }
}
