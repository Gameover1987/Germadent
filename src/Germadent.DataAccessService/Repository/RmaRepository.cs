﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Germadent.Common.Extensions;
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

                    while (reader.Read())
                    {
                        var orderEntity = new OrderEntity
                        {
                            WorkOrderId = reader["WorkOrderId"].ToInt(),
                            CustomerName = reader["CustomerName"].ToString(),
                            AdditionalInfo = reader["AdditionalInfo"].ToString(),
                            BranchTypeId = reader["BranchTypeId"].ToInt(),
                            CarcassColor = reader["CarcassColor"].ToString(),
                            DocNumber = reader["DocNumber"].ToString(),
                            ColorAndFeatures = reader["ColorAndFeatures"].ToString(),
                            FlagWorkAccept = reader["FlagWorkAccept"].ToBool(),
                            Created = reader["Created"].ToDateTime(),
                            ImplantSystem = reader["ImplantSystem"].ToString(),
                            IndividualAbutmentProcessing = reader["IndividualAbutmentProcessing"].ToString(),
                            Patient = reader["PatientFullName"].ToString(),
                            Understaff = reader["Understaff"].ToString(),
                            WorkDescription = reader["WorkDescription"].ToString(),
                            Status = reader["Status"].ToInt(),
                            ResponsiblePerson = reader["ResponsiblePerson"].ToString(),
                            ResponsiblePersonPhone = reader["RP_Phone"].ToString(),
                            PatientGender = reader["PatientGender"].ToBool(),
                            Age = reader["PatientAge"].ToInt()
                        };

                        if (DateTime.TryParse(reader[nameof(orderEntity.Closed)].ToString(), out var closed))
                        {
                            orderEntity.Closed = closed;
                        }

                        orderEntity.Created = DateTime.Parse(reader[nameof(orderEntity.Created)].ToString());
                        if (DateTime.TryParse(reader[nameof(orderEntity.Closed)].ToString(), out var dateDelivery))
                        {
                            orderEntity.DateDelivery = dateDelivery;
                        }

                        if (DateTime.TryParse(reader[nameof(orderEntity.Closed)].ToString(), out var fittingDate))
                        {
                            orderEntity.FittingDate = fittingDate;
                        }

                        return _converter.ConvertToOrder(orderEntity);
                    }
                    reader.Close();

                    return null;
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
                        orderLite.WorkOrderId = int.Parse(reader[nameof(orderLite.WorkOrderId)].ToString());
                        orderLite.BranchTypeId = int.Parse(reader[nameof(orderLite.BranchTypeId)].ToString());
                        orderLite.CustomerName = reader[nameof(orderLite.CustomerName)].ToString();
                        orderLite.ResponsiblePerson = reader[nameof(orderLite.ResponsiblePerson)].ToString();
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
