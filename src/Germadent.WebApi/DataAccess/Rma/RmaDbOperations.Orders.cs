using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Germadent.Common.Extensions;
using Germadent.Model;
using Germadent.Model.Production;
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
                command.Parameters.Add(new SqlParameter("@urgencyRatio", SqlDbType.Float)).Value = order.UrgencyRatio;
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

        public void UnlockWorkOrder(int workOrderId)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                var cmdText = "AddOrDeleteOccupancyWO";
                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@workOrderID", SqlDbType.Int)).Value = workOrderId;
                    command.Parameters.Add(new SqlParameter("@userID", SqlDbType.Int)).Value = DBNull.Value;

                    command.ExecuteNonQuery();
                }
            }
        }

        public WorkDto[] GetWorksByWorkOrder(int workOrderId, int userId)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                var cmdText = string.Format("select * from GetRelevantOperations({0},{1})", userId, workOrderId);
                using (var command = new SqlCommand(cmdText, connection))
                {
                    var operations = new List<WorkDto>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var work = new WorkDto
                            {
                                UserFullName = reader["UserFullName"].ToString(),
                                UserId = reader["UserId"].ToInt(),
                                OperationCost = reader["OperationCost"].ToDecimal(),
                                ProductId = reader["ProductId"].ToIntOrNull(),
                                Quantity = reader["ProductCount"].ToInt(),
                                Rate = reader["Rate"].ToDecimal(),
                                TechnologyOperationId = reader["TechnologyOperationID"].ToInt(),
                                TechnologyOperationName = reader["TechnologyOperationName"].ToString(),
                                TechnologyOperationUserCode = reader["TechnologyOperationUserCode"].ToString(),
                            };
                            operations.Add(work);
                        }
                    }

                    return operations.ToArray();
                }
            }
        }

        public WorkDto[] GetWorksInProgressByWorkOrder(int workOrderId, int userId)
        {
            var cmdText = string.Format("select * from GetWorkListByWOId({0}, {1})", workOrderId, userId);
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var worksCollection = new List<WorkDto>();
                    while (reader.Read())
                    {
                        var work = new WorkDto
                        {
                            WorkOrderId = reader["WorkOrderId"].ToInt(),
                            ProductId = reader["ProductId"].ToIntOrNull(),
                            TechnologyOperationId = reader["TechnologyOperationId"].ToInt(),
                            TechnologyOperationUserCode = reader["TechnologyOperationUserCode"].ToString(),
                            TechnologyOperationName = reader["TechnologyOperationName"].ToString(),
                            UserId = reader["UserId"].ToInt(),
                            UserFullName = reader["UserFullName"].ToString(),
                            Rate = reader["Rate"].ToDecimal(),
                            Quantity = reader["Quantity"].ToInt(),
                            UrgencyRatio = reader["UrgencyRatio"].ToFloat(),
                            OperationCost = reader["OperationCost"].ToDecimal(),
                            WorkStarted = reader["WorkStarted"].ToDateTime(),
                            WorkCompleted = reader["WorkCompleted"].ToDateTimeOrNull()
                        };
                        worksCollection.Add(work);
                    }
                    reader.Close();

                    return worksCollection.ToArray();
                }
            }
        }

        public void StartWorks(WorkDto[] works)
        {
            AddOrUpdateWorkList(works);

            ChangeWorkOrderStatus(works.First().WorkOrderId, OrderStatus.InProgress);
        }

        public void FinishWorks(WorkDto[] works)
        {
            AddOrUpdateWorkList(works);

            ChangeWorkOrderStatus(works.First().WorkOrderId, OrderStatus.Realization);
        }

        private void ChangeWorkOrderStatus(int workOrderId, OrderStatus status)
        {

        }

        private void AddOrUpdateWorkList(WorkDto[] works)
        {
            var workOrderId = works.First().WorkOrderId;
            var worksJson = works.SerializeToJson(Formatting.Indented);

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                var cmdText = "AddOrUpdateWorkList";
                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@workOrderId", SqlDbType.Int)).Value = workOrderId;
                    command.Parameters.Add(new SqlParameter("@jsonWorklistString", SqlDbType.NVarChar)).Value = worksJson;

                    command.ExecuteNonQuery();
                }
            }
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
                command.Parameters.Add(new SqlParameter("@urgencyRatio", SqlDbType.Float)).Value = order.UrgencyRatio;
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

            LockWorkOrder(workOrderId, userId);

            var lockers = GetWorkOrdersLockInfo(workOrderId);
            var lockInfo = lockers.First(x => x.WorkOrderId == workOrderId);
            orderDto.LockedBy = _umcDbOperations.GetUserById(lockInfo.UserId);
            orderDto.LockDate = lockInfo.OccupancyDateTime;

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
                            UrgencyRatio = (float)reader["UrgencyRatio"].ToDecimal(),
                            WorkDescription = reader["WorkDescription"].ToString(),
                            Status = reader["Status"].ToInt(),
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

        private LockWorkOrderInfoEntity[] GetWorkOrdersLockInfo(int? workOrderId = null)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                string idText = workOrderId == null ? "NULL" : workOrderId.Value.ToString();
                var cmdText = string.Format("select * from GetOccupancyWO({0})", idText);
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

                    command.Parameters.Add(new SqlParameter("@customerName", SqlDbType.NVarChar)).Value = filter.Customer.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@patientFullName", SqlDbType.NVarChar)).Value = filter.Patient.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@doctorFullName", SqlDbType.NVarChar)).Value = filter.Doctor.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@createDateFrom", SqlDbType.DateTime)).Value = filter.PeriodBegin.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@createDateTo", SqlDbType.DateTime)).Value = filter.PeriodEnd.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@materialSet", SqlDbType.NVarChar)).Value = filter.Materials.SerializeToJson();

                    return GetOrderLiteCollectionFromReader(command, users);
                }
            }
        }

        private string GetFilterCommandText(OrdersFilter filter)
        {
            var cmdTextDefault = $"SELECT * FROM GetWorkOrdersList(@branchTypeId, default, default, default, @customerName, @patientFullName, @doctorFullName, @createDateFrom, @createDateTo, default, default, default)";
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

                    var lockedBy = reader["LockedBy"];
                    if (lockedBy != DBNull.Value)
                        orderLiteDto.LockedBy = users.First(x => x.UserId == lockedBy.ToInt());

                    var lockDate = reader["LockDate"];
                    if (lockDate != DBNull.Value)
                        orderLiteDto.LockDate = lockDate.ToDateTime();

                    var closed = reader["Closed"];
                    if (closed != DBNull.Value)
                        orderLiteDto.Closed = DateTime.Parse(closed.ToString());

                    orders.Add(orderLiteDto);
                }

                return orders.ToArray();
            }
        }

        private StatusListDto[] GetStatusListForWO(int workOrderId)
        {
            var cmdText = string.Format("select * from GetStatusListForWO({0})", workOrderId);
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();

                    var entities = new List<StatusListEntity>();

                    while (reader.Read())
                    {
                        var statusEntity = new StatusListEntity();

                        statusEntity.WorkOrderId = reader[nameof(statusEntity.WorkOrderId)].ToInt();
                        statusEntity.Status = reader[nameof(statusEntity.Status)].ToInt();
                        statusEntity.StatusName = reader[nameof(statusEntity.StatusName)].ToString();
                        statusEntity.StatusChangeDateTime = reader[nameof(statusEntity.StatusChangeDateTime)].ToDateTime();
                        statusEntity.UserId = reader[nameof(statusEntity.UserId)].ToInt();
                        statusEntity.UserFullName = reader[nameof(statusEntity.UserFullName)].ToString();
                        statusEntity.Remark = reader[nameof(statusEntity.Remark)].ToString();

                        entities.Add(statusEntity);
                    }
                    reader.Close();

                    var statusListDtoCollection = entities.Select(x => _converter.ConvertToStatusList(x)).ToArray();
                    return statusListDtoCollection;
                }
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
