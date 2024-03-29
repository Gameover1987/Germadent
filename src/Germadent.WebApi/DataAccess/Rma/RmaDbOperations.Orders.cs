﻿using System;
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

            using (var command = new SqlCommand("dbo.AddWorkOrder", connection))
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

        public OrderStatusNotificationDto CloseOrder(int workOrderId, int userId)
        {
            return ExecuteInTransactionScope(transaction =>
                ChangeWorkOrderStatusImpl(transaction, workOrderId, userId, OrderStatus.Closed));
        }

        public WorkDto[] GetRelevantOperations(int workOrderId, int userId)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                var cmdText = string.Format("select * from dbo.GetRelevantOperations({0},{1})", userId, workOrderId);
                using (var command = new SqlCommand(cmdText, connection))
                {
                    var operations = new List<WorkDto>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var work = new WorkDto
                            {
                                UserFullNameStarted = reader["UserFullName"].ToString(),
                                UserIdStarted = reader["UserId"].ToInt(),
                                OperationCost = reader["OperationCost"].ToDecimal(),
                                ProductId = reader["ProductId"].ToIntOrNull(),
                                ProductName = reader["ProductName"].ToString(),
                                Quantity = reader["ProductCount"].ToInt(),
                                Rate = reader["Rate"].ToDecimal(),
                                UrgencyRatio = reader["UrgencyRatio"].ToFloat(),
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
            return ExecuteInTransactionScope(transaction => GetWorksInProgressByWorkOrderImpl(transaction, workOrderId, userId));
        }

        public WorkDto[] GetAllWorksByWorkOrder(int workOrderId)
        {
            return ExecuteInTransactionScope(transaction => GetAllWorksByWorkOrderImpl(transaction, workOrderId));
        }

        private WorkDto[] GetAllWorksByWorkOrderImpl(SqlTransaction transaction, int workOrderId)
        {
            var cmdText = $"select * from dbo.GetWorkListByWOId({workOrderId}, default)";
            using (var command = new SqlCommand(cmdText, transaction.Connection))
            {
                command.Transaction = transaction;
                using (var reader = command.ExecuteReader())
                {
                    return GetWorksFromReader(reader);
                }
            }
        }

        private WorkDto[] GetWorksInProgressByWorkOrderImpl(SqlTransaction transaction, int workOrderId, int? userId)
        {
            var cmdText = string.Format("select * from dbo.GetWorkListByWOId({0}, {1}) where WorkCompleted is NULL", workOrderId, userId == null ? "NULL" : userId.Value.ToString());
            using (var command = new SqlCommand(cmdText, transaction.Connection))
            {
                command.Transaction = transaction;
                using (var reader = command.ExecuteReader())
                {
                    return GetWorksFromReader(reader);
                }
            }
        }

        private static WorkDto[] GetWorksFromReader(IDataReader reader)
        {
            var worksCollection = new List<WorkDto>();
            while (reader.Read())
            {
                var work = new WorkDto
                {
                    WorkOrderId = reader["WorkOrderId"].ToInt(),
                    WorkId = reader["WorkId"].ToInt(),
                    ProductId = reader["ProductId"].ToIntOrNull(),
                    ProductName = reader["ProductName"].ToString(),
                    TechnologyOperationId = reader["TechnologyOperationId"].ToInt(),
                    TechnologyOperationUserCode = reader["TechnologyOperationUserCode"].ToString(),
                    TechnologyOperationName = reader["TechnologyOperationName"].ToString(),
                    UserIdStarted = reader["UserIdStarted"].ToInt(),
                    UserFullNameStarted = reader["UserFullNameStarted"].ToString(),
                    Rate = reader["Rate"].ToDecimal(),
                    Quantity = reader["Quantity"].ToInt(),
                    UrgencyRatio = reader["UrgencyRatio"].ToFloat(),
                    OperationCost = reader["OperationCost"].ToDecimal(),
                    WorkStarted = reader["WorkStarted"].ToDateTime(),
                    WorkCompleted = reader["WorkCompleted"].ToDateTimeOrNull(),
                    Comment = reader["Comment"].ToString()
                };
                worksCollection.Add(work);
            }
            return worksCollection.ToArray();
        }

        public OrderStatusNotificationDto StartWorks(WorkDto[] works)
        {
            var workOrderId = works.First().WorkOrderId;
            var userId = works.First().UserIdStarted;

            return ExecuteInTransactionScope(transaction =>
            {
                foreach (var workDto in works)
                {
                    StartWork(workDto, transaction);
                }

                return ChangeWorkOrderStatusImpl(transaction, workOrderId, userId, OrderStatus.InProgress);
            });
        }

        public OrderStatusNotificationDto FinishWorks(WorkDto[] works)
        {
            if (works.Any(x => x.UserIdCompleted == null))
                throw new InvalidOperationException("У как минимум одной из работ не указан пользователь, завершивший ее");

            var workOrderId = works.First().WorkOrderId;
            var userId = works.First().UserIdCompleted.Value;

            return ExecuteInTransactionScope(transaction =>
            {
                foreach (var workDto in works)
                {
                    FinishWork(workDto, transaction);
                }

                var worksInProgress = GetWorksInProgressByWorkOrderImpl(transaction, workOrderId, null);
                if (!worksInProgress.Any())
                {
                    return ChangeWorkOrderStatusImpl(transaction, workOrderId, userId, OrderStatus.QualityControl);
                }

                return null;
            });
        }

        public OrderStatusNotificationDto PerformQualityControl(int workOrderId, int userId)
        {
            return ExecuteInTransactionScope(tran => ChangeWorkOrderStatusImpl(tran, workOrderId, userId, OrderStatus.Realization));
        }

        private void StartWork(WorkDto work, SqlTransaction transaction)
        {
            var cmdText = "dbo.AddWork";
            using (var command = new SqlCommand(cmdText, transaction.Connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = transaction;

                command.Parameters.Add(new SqlParameter("@workOrderId", SqlDbType.Int)).Value = work.WorkOrderId;
                command.Parameters.Add(new SqlParameter("@productId", SqlDbType.Int)).Value = work.ProductId.GetValueOrDbNull();
                command.Parameters.Add(new SqlParameter("@technologyOperationId", SqlDbType.Int)).Value = work.TechnologyOperationId;
                command.Parameters.Add(new SqlParameter("@rate", SqlDbType.Money)).Value = work.Rate;
                command.Parameters.Add(new SqlParameter("@quantity", SqlDbType.Int)).Value = work.Quantity;
                command.Parameters.Add(new SqlParameter("@operationCost", SqlDbType.Money)).Value = work.OperationCost;
                command.Parameters.Add(new SqlParameter("@userIdStarted", SqlDbType.Int)).Value = work.UserIdStarted;
                command.Parameters.Add(new SqlParameter("@comment", SqlDbType.NVarChar)).Value = work.Comment.GetValueOrDbNull();
                command.Parameters.Add(new SqlParameter("@workId", SqlDbType.Int) { Direction = ParameterDirection.Output });

                command.ExecuteNonQuery();
            }
        }

        private void FinishWork(WorkDto work, SqlTransaction transaction)
        {
            var cmdText = "dbo.FinishWork";
            using (var command = new SqlCommand(cmdText, transaction.Connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = transaction;

                command.Parameters.Add(new SqlParameter("@workId", SqlDbType.Int)).Value = work.WorkId;
                command.Parameters.Add(new SqlParameter("@userIdCompleted", SqlDbType.Int)).Value = work.UserIdCompleted;
                command.Parameters.Add(new SqlParameter("@comment", SqlDbType.NVarChar)).Value = work.Comment.GetValueOrDbNull();
                command.Parameters.Add(new SqlParameter("@statusChangeDateTime", SqlDbType.DateTime) { Direction = ParameterDirection.Output });

                command.ExecuteNonQuery();
            }
        }

        private OrderStatusNotificationDto ChangeWorkOrderStatusImpl(SqlTransaction transaction, int workOrderId, int userId, OrderStatus status)
        {
            var cmdText = "dbo.ChangeStatusWorkOrderEasy";
            using (var command = new SqlCommand(cmdText, transaction.Connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = transaction;
                command.Parameters.Add(new SqlParameter("@workOrderId", SqlDbType.Int)).Value = workOrderId;
                command.Parameters.Add(new SqlParameter("@userId", SqlDbType.Int)).Value = userId;
                command.Parameters.Add(new SqlParameter("@statusNext", SqlDbType.Int)).Value = (int)status;
                command.Parameters.Add(new SqlParameter("@statusChangeDateTime", SqlDbType.DateTime) { Direction = ParameterDirection.Output });

                command.ExecuteNonQuery();

                var statusChangeDateTime = command.Parameters["@statusChangeDateTime"].Value.ToDateTimeOrNull();
                if (statusChangeDateTime == null)
                    return null;

                return new OrderStatusNotificationDto
                {
                    WorkOrderId = workOrderId,
                    UserId = userId,
                    Status = status,
                    StatusChangeDateTime = statusChangeDateTime.Value
                };
            }
        }

        private OrderStatusNotificationDto FinishAllWorksInWorkOrderIml(SqlTransaction transaction, int workOrderId, int userId, OrderStatus status)
        {
            var cmdText = "dbo.FinishAllWorksInWO";
            using (var command = new SqlCommand(cmdText, transaction.Connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = transaction;
                command.Parameters.Add(new SqlParameter("@workOrderId", SqlDbType.Int)).Value = workOrderId;
                command.Parameters.Add(new SqlParameter("@userId", SqlDbType.Int)).Value = userId;
                command.Parameters.Add(new SqlParameter("@statusNext", SqlDbType.Int)).Value = 90;
                command.Parameters.Add(new SqlParameter("@finishWorksDateTime", SqlDbType.DateTime) { Direction = ParameterDirection.Output });

                command.ExecuteNonQuery();


                var statusChangeDateTime = command.Parameters["@finishWorksDateTime"].Value.ToDateTimeOrNull();
                if (statusChangeDateTime == null)
                    return null;

                return new OrderStatusNotificationDto
                {
                    WorkOrderId = workOrderId,
                    UserId = userId,
                    Status = status,
                    StatusChangeDateTime = statusChangeDateTime.Value
                };
            }
        }

        private void UpdateWorkOrder(OrderDto order, SqlConnection connection)
        {
            var jsonToothCardString = order.ToothCard.SelectMany(x => _converter.ConvertFromToothDto(x, order.Stl)).SerializeToJson(Formatting.Indented);
            var jsonEquipmentsString = order.AdditionalEquipment.SerializeToJson();
            var jsonAttributesString = order.Attributes.SerializeToJson(Formatting.Indented);

            using (var command = new SqlCommand("dbo.UpdateWorkOrder", connection))
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

        // TODO: Transcation scope
        public OrderDto GetOrderDetails(int workOrderId, int userId)
        {
            var orderDto = GetWorkOrderById(workOrderId);
            orderDto.ToothCard = GetToothCard(workOrderId, orderDto.Stl);
            orderDto.Works = GetAllWorksByWorkOrder(workOrderId);

            LockWorkOrder(workOrderId, userId);

            var lockers = GetWorkOrdersLockInfo(workOrderId);
            var lockInfo = lockers.FirstOrDefault(x => x.WorkOrderId == workOrderId);
            if (lockInfo == null)
                return orderDto;

            orderDto.LockedBy = _umcDbOperations.GetUserById(lockInfo.UserId);
            orderDto.LockDate = lockInfo.OccupancyDateTime;

            return orderDto;
        }

        private OrderDto GetWorkOrderById(int workOrderId)
        {
            var cmdText = string.Format("select * from dbo.GetWorkOrderById({0})", workOrderId);

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
                            StatusChanged = reader["StatusChangeDateTime"].ToDateTime(),
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
    }
}