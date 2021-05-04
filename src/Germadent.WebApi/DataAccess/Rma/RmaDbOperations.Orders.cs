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
        public OrderDto AddOrder(OrderDto order)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                OrderDto outputOrder;
                connection.Open();
                outputOrder = AddWorkOrder(order, connection);

                return outputOrder;
            }
        }

        private OrderDto AddWorkOrder(OrderDto order, SqlConnection connection)
        {
            var jsonToothCardString = order.ToothCard.SelectMany(x => _converter.ConvertFromToothDto(x, order.Stl)).SerializeToJson(Formatting.Indented);
            var jsonEquipmentsString = order.AdditionalEquipment.SerializeToJson();
            var jsonAttributesString = order.Attributes.SerializeToJson(Formatting.Indented);

            using (var command = new SqlCommand("AddWorkOrder", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@branchTypeId", SqlDbType.Int)).Value = (int)order.BranchType;
                command.Parameters.Add(new SqlParameter("@customerId", SqlDbType.Int)).Value = order.CustomerId;
                command.Parameters.Add(new SqlParameter("@responsiblePersonId", SqlDbType.Int)).Value = order.ResponsiblePersonId == 0 ? (object)DBNull.Value : order.ResponsiblePersonId;
                command.Parameters.Add(new SqlParameter("@patientFullName", SqlDbType.NVarChar)).Value = order.Patient;
                command.Parameters.Add(new SqlParameter("@patientGender", SqlDbType.TinyInt)).Value = (int)order.Gender;
                command.Parameters.Add(new SqlParameter("@patientAge", SqlDbType.TinyInt)).Value = order.Age;
                command.Parameters.Add(new SqlParameter("@dateComment", SqlDbType.NVarChar)).Value = order.DateComment;
                command.Parameters.Add(new SqlParameter("@prostheticArticul", SqlDbType.NVarChar)).Value = order.ProstheticArticul;
                command.Parameters.Add(new SqlParameter("@workDescription", SqlDbType.NVarChar)).Value = order.WorkDescription;
                command.Parameters.Add(new SqlParameter("@flagStl", SqlDbType.Bit)).Value = order.Stl;
                command.Parameters.Add(new SqlParameter("@flagCashless", SqlDbType.Bit)).Value = order.Cashless;
                command.Parameters.Add(new SqlParameter("@creatorId", SqlDbType.Int)).Value = order.CreatorId;
                command.Parameters.Add(new SqlParameter("@fittingDate", SqlDbType.DateTime)).Value = order.FittingDate;
                command.Parameters.Add(new SqlParameter("@dateOfCompletion", SqlDbType.DateTime)).Value = order.DateOfCompletion;
                command.Parameters.Add(new SqlParameter("@jsonToothCardString", SqlDbType.NVarChar)).Value = jsonToothCardString;
                command.Parameters.Add(new SqlParameter("@jsonEquipmentsString", SqlDbType.NVarChar)).Value = jsonEquipmentsString;
                command.Parameters.Add(new SqlParameter("@jsonAttributesString", SqlDbType.NVarChar)).Value = jsonAttributesString;
                command.Parameters.Add(new SqlParameter("@workOrderId", SqlDbType.Int) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new SqlParameter("@docNumber", SqlDbType.NVarChar) { Direction = ParameterDirection.Output, Size = 10 });
                command.Parameters.Add(new SqlParameter("@created", SqlDbType.DateTime) { Direction = ParameterDirection.Output });

                command.ExecuteNonQuery();

                order.WorkOrderId = command.Parameters["@workOrderId"].Value.ToInt();
                order.DocNumber = command.Parameters["@docNumber"].Value.ToString();
                order.Created = command.Parameters["@created"].Value.ToDateTime();
            }

            return order;
        }

        public void UpdateOrder(OrderDto order)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                UpdateWorkOrder(order, connection);
            }
        }

        public OrderDto CloseOrder(int id)
        {
            var cmdText = "CloseWorkOrder";
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@workOrderId", SqlDbType.Int)).Value = id;

                    command.ExecuteNonQuery();
                }
            }
            var orderDto = GetWorkOrderById(id);
            return orderDto;
        }

        private void UpdateWorkOrder(OrderDto order, SqlConnection connection)
        {
            var jsonToothCardString = order.ToothCard.SelectMany(x => _converter.ConvertFromToothDto(x, order.Stl)).SerializeToJson(Formatting.Indented);
            var jsonEquipmentsString = order.AdditionalEquipment.SerializeToJson();
            var jsonAttributesString = order.Attributes.SerializeToJson(Formatting.Indented);

            using (var command = new SqlCommand("UpdateWorkOrder", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@branchTypeId", SqlDbType.Int)).Value = (int)order.BranchType;
                command.Parameters.Add(new SqlParameter("@workOrderId", SqlDbType.Int)).Value = order.WorkOrderId;
                command.Parameters.Add(new SqlParameter("@customerID", SqlDbType.Int)).Value = order.CustomerId;
                command.Parameters.Add(new SqlParameter("@responsiblePersonId", SqlDbType.Int)).Value = order.ResponsiblePersonId == 0 ? (object)DBNull.Value : order.ResponsiblePersonId;
                command.Parameters.Add(new SqlParameter("@flagWorkAccept", SqlDbType.Bit)).Value = order.WorkAccepted;
                command.Parameters.Add(new SqlParameter("@flagStl", SqlDbType.Bit)).Value = order.Stl;
                command.Parameters.Add(new SqlParameter("@flagCashless", SqlDbType.Bit)).Value = order.Cashless;
                command.Parameters.Add(new SqlParameter("@dateComment", SqlDbType.NVarChar)).Value = order.DateComment;
                command.Parameters.Add(new SqlParameter("@prostheticArticul", SqlDbType.NVarChar)).Value = order.ProstheticArticul.GetValueOrDbNull();
                command.Parameters.Add(new SqlParameter("@workDescription", SqlDbType.NVarChar)).Value = order.WorkDescription.GetValueOrDbNull();
                command.Parameters.Add(new SqlParameter("@patientFullName", SqlDbType.NVarChar)).Value = order.Patient;
                command.Parameters.Add(new SqlParameter("@patientGender", SqlDbType.Bit)).Value = (int)order.Gender;
                command.Parameters.Add(new SqlParameter("@patientAge", SqlDbType.SmallInt)).Value = order.Age;
                command.Parameters.Add(new SqlParameter("@fittingDate", SqlDbType.DateTime)).Value = order.FittingDate.GetValueOrDbNull();
                command.Parameters.Add(new SqlParameter("@dateOfCompletion", SqlDbType.DateTime)).Value = order.DateOfCompletion.GetValueOrDbNull();
                command.Parameters.Add(new SqlParameter("@created", SqlDbType.DateTime) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new SqlParameter("@jsonToothCardString", SqlDbType.NVarChar)).Value = jsonToothCardString;
                command.Parameters.Add(new SqlParameter("@jsonEquipmentsString", SqlDbType.NVarChar)).Value = jsonEquipmentsString;
                command.Parameters.Add(new SqlParameter("@jsonAttributesString", SqlDbType.NVarChar)).Value = jsonAttributesString;

                command.ExecuteNonQuery();

                order.Created = command.Parameters["@created"].Value.ToDateTime();
            }

        }

        public OrderDto GetOrderDetails(int workOrderId, int userId)
        {
            var orderDto = GetWorkOrderById(workOrderId);
            orderDto.ToothCard = GetToothCard(workOrderId, orderDto.Stl);

            var lockers = GetWorkOrdersLockInfo();
            var lockInfo = lockers.FirstOrDefault(x => x.WorkOrderId == workOrderId);
            if (lockInfo != null)
            {
                orderDto.LockedBy = _umcDbOperations.GetUserById(lockInfo.UserId);
                orderDto.LockDate = lockInfo.OccupancyDateTime;
            }

            if (lockInfo == null)
            {
                LockWorkOrder(workOrderId, userId);
            }

            return orderDto;
        }

        private OrderDto GetWorkOrderById(int workOrderId)
        {
            var cmdText = string.Format("select * from GetWorkOrderById({0})", workOrderId);

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                OrderDto order = null;

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var orderEntity = new OrderEntity
                        {
                            WorkOrderId = reader["WorkOrderId"].ToInt(),
                            CustomerId = reader["CustomerId"].ToInt(),
                            CustomerName = reader["CustomerName"].ToString(),
                            BranchTypeId = reader["BranchTypeId"].ToInt(),
                            DocNumber = reader["DocNumber"].ToString(),
                            FlagWorkAccept = reader["FlagWorkAccept"].ToBool(),
                            FlagStl = reader["FlagStl"].ToBool(),
                            FlagCashless = reader["FlagCashless"].ToBool(),
                            Created = reader["Created"].ToDateTime(),
                            Patient = reader["PatientFullName"].ToString(),
                            WorkDescription = reader["WorkDescription"].ToString(),
                            Status = reader["Status"].ToString(),
                            ResponsiblePersonId = reader["ResponsiblePersonId"].ToInt(),
                            DoctorFullName = reader["DoctorFullName"].ToString(),
                            TechnicFullName = reader["TechnicFullName"].ToString(),
                            TechnicPhone = reader["TechnicPhone"].ToString(),
                            PatientGender = reader["PatientGender"].ToBool(),
                            Age = reader["PatientAge"].ToInt(),
                            ProstheticArticul = reader["ProstheticArticul"].ToString(),
                            MaterialsEnum = reader["MaterialsEnum"].ToString(),
                            DateComment = reader["DateComment"].ToString(),
                            CreatorFullName = reader["CreatorFullName"].ToString(),
                        };

                        if (DateTime.TryParse(reader[nameof(orderEntity.Closed)].ToString(), out var closed))
                        {
                            orderEntity.Closed = closed;
                        }

                        if (DateTime.TryParse(reader[nameof(orderEntity.FittingDate)].ToString(), out var fittingDate))
                        {
                            orderEntity.FittingDate = fittingDate;
                        }

                        if (DateTime.TryParse(reader[nameof(orderEntity.DateOfCompletion)].ToString(), out var dateOfCompletion))
                        {
                            orderEntity.DateOfCompletion = dateOfCompletion;
                        }

                        order = _converter.ConvertToOrder(orderEntity);
                    }

                    reader.Close();
                }

                order.AdditionalEquipment = GetAdditionalEquipmentByWorkOrder(workOrderId, connection);
                order.Attributes = GetAttributesByWoId(workOrderId, connection);
                return order;
            }
        }

        private LockWorkOrderInfoEntity[] GetWorkOrdersLockInfo()
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                var cmdText = "select * from GetOccupancyWO(NULL)";
                var lockers = new List<LockWorkOrderInfoEntity>();
                using (var command = new SqlCommand(cmdText, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entity = new LockWorkOrderInfoEntity();
                            entity.WorkOrderId = reader[nameof(entity.WorkOrderId)].ToInt();
                            entity.UserId = reader[nameof(entity.UserId)].ToInt();
                            entity.OccupancyDateTime = reader[nameof(entity.OccupancyDateTime)].ToDateTime();
                            lockers.Add(entity);
                        }
                    }
                }

                return lockers.ToArray();
            }
        }

        private void LockWorkOrder(int workOrderId, int userId)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                var cmdText = "AddOrDeleteOccupancyWO";
                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@workOrderID", SqlDbType.Int)).Value = workOrderId;
                    command.Parameters.Add(new SqlParameter("@userID", SqlDbType.Int)).Value = userId;

                    command.ExecuteNonQuery();
                }
            }
        }

        public OrderLiteDto[] GetOrders(OrdersFilter filter)
        {
            return GetOrdersByFilter(filter);
        }

        private OrderLiteDto[] GetOrdersByFilter(OrdersFilter filter)
        {
            var cmdText = GetFilterCommandText(filter);

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

                    command.Parameters.Add(new SqlParameter("@customerName", SqlDbType.NVarChar)).Value = filter.Customer.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@patientFullName", SqlDbType.NVarChar)).Value = filter.Patient.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@doctorFullName", SqlDbType.NVarChar)).Value = filter.Doctor.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@createDateFrom", SqlDbType.DateTime)).Value = filter.PeriodBegin.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@createDateTo", SqlDbType.DateTime)).Value = filter.PeriodEnd.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@materialSet", SqlDbType.NVarChar)).Value = filter.Materials.SerializeToJson();

                    return GetOrderLiteCollectionFromReader(command);
                }
            }
        }

        private string GetFilterCommandText(OrdersFilter filter)
        {
            var cmdTextDefault = $"SELECT * FROM GetWorkOrdersList(@branchTypeId, default, default, default, @customerName, @patientFullName, @doctorFullName, @createDateFrom, @createDateTo, default, default)";
            var cmdTextAdditional = $"WHERE WorkOrderID IN (SELECT * FROM GetWorkOrderIdForMaterialSelect(@materialSet))";

            if (filter.Materials.Length == 0)
            {
                return cmdTextDefault;
            }

            return cmdTextDefault + cmdTextAdditional;
        }

        private OrderLiteDto[] GetOrderLiteCollectionFromReader(SqlCommand command)
        {
            using (var reader = command.ExecuteReader())
            {
                var orderLiteEntities = new List<OrderLiteEntity>();
                while (reader.Read())
                {
                    var orderLite = new OrderLiteEntity();
                    orderLite.WorkOrderId = int.Parse(reader[nameof(orderLite.WorkOrderId)].ToString());
                    orderLite.BranchTypeId = int.Parse(reader[nameof(orderLite.BranchTypeId)].ToString());
                    orderLite.CustomerName = reader[nameof(orderLite.CustomerName)].ToString();
                    orderLite.PatientFullName = reader[nameof(orderLite.PatientFullName)].ToString();
                    orderLite.DoctorFullName = reader[nameof(orderLite.DoctorFullName)].ToString();
                    orderLite.DocNumber = reader[nameof(orderLite.DocNumber)].ToString();
                    orderLite.Created = DateTime.Parse(reader[nameof(orderLite.Created)].ToString());
                    orderLite.CreatorFullName = reader[nameof(orderLite.CreatorFullName)].ToString();
                    orderLite.Status = reader[nameof(orderLite.Status)].ToString();

                    var closed = reader[nameof(orderLite.Closed)];
                    if (closed != DBNull.Value)
                        orderLite.Closed = DateTime.Parse(closed.ToString());

                    orderLiteEntities.Add(orderLite);
                }

                var orders = orderLiteEntities.Select(x => _converter.ConvertToOrderLite(x)).ToArray();
                return orders;
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
