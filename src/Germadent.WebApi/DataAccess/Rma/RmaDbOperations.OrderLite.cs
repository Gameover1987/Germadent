using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Germadent.Common.Extensions;
using Germadent.Model;
using Germadent.WebApi.Entities;
using Newtonsoft.Json;

namespace Germadent.WebApi.DataAccess.Rma
{
    public partial class RmaDbOperations
    {
        public OrderLiteDto[] GetOrders(OrdersFilter filter)
        {
            return GetOrdersByFilter(filter);
        }

        private OrderLiteDto[] GetOrdersByFilter(OrdersFilter filter)
        {
            var cmdText = GetFilterCommandText(filter);

            var users = _umcDbOperations.GetUsers();

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    if (filter.Laboratory && filter.MillingCenter)
                    {
                        command.Parameters.Add(new SqlParameter("@branchTypeId", SqlDbType.Int)).Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@branchTypeId", SqlDbType.Int)).Value = filter.Laboratory ? 2 : 1;
                    }

                    var statusesJson = filter.Statuses.Select(x => new { StatusNumber = (int)x }).ToArray().SerializeToJson(Formatting.Indented);

                    command.Parameters.Add(new SqlParameter("@customerName", SqlDbType.NVarChar)).Value = filter.Customer.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@patientFullName", SqlDbType.NVarChar)).Value = filter.Patient.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@doctorFullName", SqlDbType.NVarChar)).Value = filter.Doctor.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@createDateFrom", SqlDbType.DateTime)).Value = filter.PeriodBegin.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@createDateTo", SqlDbType.DateTime)).Value = filter.PeriodEnd.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@jsonStringStatus", SqlDbType.NVarChar)).Value = statusesJson;
                    command.Parameters.Add(new SqlParameter("@materialSet", SqlDbType.NVarChar)).Value = filter.Materials.SerializeToJson();

                    return GetOrderLiteCollectionFromReader(command, users);
                }
            }
        }

        private string GetFilterCommandText(OrdersFilter filter)
        {
            var cmdTextDefault = $"SELECT * FROM GetWorkOrdersList(@branchTypeId, default, default, default, @customerName, @patientFullName, @doctorFullName, @createDateFrom, @createDateTo, @userId, @jsonStringStatus)";
            var cmdTextAdditional = $"WHERE WorkOrderID IN (SELECT * FROM GetWorkOrderIdForMaterialSelect(@materialSet))";

            if (filter.Materials.Length == 0)
            {
                return cmdTextDefault;
            }

            return cmdTextDefault + cmdTextAdditional;
        }

        private OrderLiteDto[] GetOrderLiteCollectionFromReader(SqlCommand command, UserDto[] users)
        {
            using (var reader = command.ExecuteReader())
            {
                var orders = new List<OrderLiteDto>();
                while (reader.Read())
                {
                    var orderLiteDto = new OrderLiteDto();
                    orderLiteDto.WorkOrderId = int.Parse(reader["WorkOrderId"].ToString());
                    orderLiteDto.BranchType = (BranchType)int.Parse(reader["BranchTypeId"].ToString());
                    orderLiteDto.CustomerName = reader["CustomerName"].ToString();
                    orderLiteDto.PatientFnp = reader["PatientFullName"].ToString();
                    orderLiteDto.DoctorFullName = reader["DoctorFullName"].ToString();
                    orderLiteDto.DocNumber = reader["DocNumber"].ToString();
                    orderLiteDto.Created = DateTime.Parse(reader["Created"].ToString());
                    orderLiteDto.CreatorFullName = reader["CreatorFullName"].ToString();

                    orderLiteDto.Status = (OrderStatus)reader[nameof(OrderLiteEntity.Status)].ToInt();
                    orderLiteDto.StatusChanged = DateTime.Parse(reader["StatusChangeDateTime"].ToString());

                    var lockedBy = reader["LockedBy"];
                    if (lockedBy != DBNull.Value)
                        orderLiteDto.LockedBy = users.First(x => x.UserId == lockedBy.ToInt());

                    var lockDate = reader["LockDate"];
                    if (lockDate != DBNull.Value)
                        orderLiteDto.LockDate = lockDate.ToDateTime();

                    orders.Add(orderLiteDto);
                }

                return orders.ToArray();
            }
        }

        private ToothDto[] GetToothCard(int id, bool getPricesAsStl)
        {
            var cmdText = string.Format("select * from GetToothCardByWOId({0})", id);
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();

                    var entities = new List<ToothEntity>();
                    while (reader.Read())
                    {
                        var toothEntity = new ToothEntity();

                        toothEntity.WorkOrderId = reader[nameof(toothEntity.WorkOrderId)].ToInt();
                        toothEntity.ToothNumber = reader[nameof(toothEntity.ToothNumber)].ToInt();
                        toothEntity.MaterialId = reader[nameof(toothEntity.MaterialId)].ToIntOrNull();
                        toothEntity.MaterialName = reader[nameof(toothEntity.MaterialName)].ToString();
                        toothEntity.ConditionId = reader[nameof(toothEntity.ConditionId)].ToInt();
                        toothEntity.ConditionName = reader[nameof(toothEntity.ConditionName)].ToString();
                        toothEntity.ProductId = reader[nameof(toothEntity.ProductId)].ToInt();
                        toothEntity.ProductName = reader[nameof(toothEntity.ProductName)].ToString();
                        toothEntity.Price = reader[nameof(toothEntity.Price)].ToDecimal();
                        toothEntity.HasBridge = reader[nameof(toothEntity.HasBridge)].ToBool();
                        toothEntity.PricePositionId = reader[nameof(toothEntity.PricePositionId)].ToInt();
                        toothEntity.PricePositionCode = reader[nameof(toothEntity.PricePositionCode)].ToString();
                        toothEntity.PricePositionName = reader[nameof(toothEntity.PricePositionName)].ToString();
                        toothEntity.PriceGroupId = reader[nameof(toothEntity.PriceGroupId)].ToInt();

                        entities.Add(toothEntity);
                    }

                    reader.Close();

                    var toothDtoCollection = _converter.ConvertToToothCard(entities.ToArray(), getPricesAsStl);
                    return toothDtoCollection;
                }
            }
        }
    }
}
