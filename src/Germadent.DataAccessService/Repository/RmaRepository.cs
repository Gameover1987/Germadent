using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.DataAccessService.Configuration;
using Germadent.DataAccessService.Entities;
using Germadent.DataAccessService.Entities.Conversion;
using Germadent.Rma.Model;
using Newtonsoft.Json;

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

        public OrderDto AddOrder(OrderDto order)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                OrderDto outputOrder = null;
                connection.Open();

                switch (order.BranchType)
                {
                    case BranchType.Laboratory:
                        outputOrder = AddWorkOrderDL(order, connection);
                        break;

                    case BranchType.MillingCenter:
                        outputOrder = AddWorkOrderMC(order, connection);
                        break;

                    default:
                        throw new NotSupportedException("Неизвестный тип филиала");
                }

                order.ToothCard.ForEach(x => x.WorkOrderId = order.WorkOrderId);
                AddOrUpdateToothCard(order.ToothCard, connection);

                return outputOrder;
            }
        }

        private ToothDto[] AddOrUpdateToothCard(ToothDto[] toothCard, SqlConnection connection)
        {
            var toothCardJson = toothCard.SerializeToJson(Formatting.Indented);

            var cmdText = "AddOrUpdateToothCardInWO";

            using (var command = new SqlCommand(cmdText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@jsonString", SqlDbType.NVarChar)).Value = toothCardJson;

                command.ExecuteNonQuery();

                return toothCard;
            }
        }

        private static OrderDto AddWorkOrderDL(OrderDto order, SqlConnection connection)
        {
            using (var command = new SqlCommand("AddNewWorkOrderDL", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@customerId", SqlDbType.Int)).Value = DBNull.Value;
                command.Parameters.Add(new SqlParameter("@customerName", SqlDbType.NVarChar)).Value = order.Customer;
                command.Parameters.Add(new SqlParameter("@responsiblePersonId", SqlDbType.Int)).Value = DBNull.Value;
                command.Parameters.Add(new SqlParameter("@doctorFullName", SqlDbType.NVarChar)).Value = order.ResponsiblePerson;
                command.Parameters.Add(new SqlParameter("@patientId", SqlDbType.Int)).Value = DBNull.Value;
                command.Parameters.Add(new SqlParameter("@patientFullName", SqlDbType.NVarChar)).Value = order.Patient;
                command.Parameters.Add(new SqlParameter("@patientAge", SqlDbType.TinyInt)).Value = order.Age;
                command.Parameters.Add(new SqlParameter("@workDescription", SqlDbType.NVarChar)).Value = order.WorkDescription;
                command.Parameters.Add(new SqlParameter("@officeAdminId", SqlDbType.Int)).Value = DBNull.Value;
                command.Parameters.Add(new SqlParameter("@officeAdminName", SqlDbType.NVarChar)).Value = "Мега администратор";
                command.Parameters.Add(new SqlParameter("@transparenceId", SqlDbType.Int)).Value = order.Transparency;
                command.Parameters.Add(new SqlParameter("@fittingDate", SqlDbType.DateTime)).Value = order.FittingDate;
                command.Parameters.Add(new SqlParameter("@dateOfCompletion", SqlDbType.DateTime)).Value = order.Closed;
                command.Parameters.Add(new SqlParameter("@colorAndFeatures", SqlDbType.NVarChar)).Value = order.ColorAndFeatures;
                command.Parameters.Add(new SqlParameter("@workOrderId", SqlDbType.Int) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new SqlParameter("@docNumber", SqlDbType.NVarChar) { Direction = ParameterDirection.Output, Size = 10 });
                command.Parameters.Add(new SqlParameter("@prostheticArticul", SqlDbType.NVarChar)).Value = order.ProstheticArticul;

                command.ExecuteNonQuery();

                order.WorkOrderId = command.Parameters["@workOrderId"].Value.ToInt();
                order.DocNumber = command.Parameters["@docNumber"].Value.ToString();

                return order;
            }
        }

        private static OrderDto AddWorkOrderMC(OrderDto order, SqlConnection connection)
        {
            using (var command = new SqlCommand("AddNewWorkOrderMC", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@customerId", SqlDbType.Int)).Value = DBNull.Value;
                command.Parameters.Add(new SqlParameter("@customerName", SqlDbType.NVarChar)).Value = order.Customer;
                command.Parameters.Add(new SqlParameter("@responsiblePersonId", SqlDbType.Int)).Value = DBNull.Value;
                command.Parameters.Add(new SqlParameter("@technicFullName", SqlDbType.NVarChar)).Value = order.ResponsiblePerson;
                command.Parameters.Add(new SqlParameter("@technicPhone", SqlDbType.NVarChar)).Value = order.ResponsiblePersonPhone;
                command.Parameters.Add(new SqlParameter("@patientID", SqlDbType.NVarChar)).Value = DBNull.Value;
                command.Parameters.Add(new SqlParameter("@patientFullName", SqlDbType.NVarChar)).Value = order.Patient;
                command.Parameters.Add(new SqlParameter("@prostheticArticul", SqlDbType.NVarChar)).Value = order.ProstheticArticul;
                command.Parameters.Add(new SqlParameter("@workDescription", SqlDbType.NVarChar)).Value = order.WorkDescription;
                command.Parameters.Add(new SqlParameter("@officeAdminId", SqlDbType.Int)).Value = DBNull.Value;
                command.Parameters.Add(new SqlParameter("@officeAdminName", SqlDbType.NVarChar)).Value = "Мега администратор";
                command.Parameters.Add(new SqlParameter("@additionalInfo", SqlDbType.NVarChar)).Value = order.AdditionalInfo;
                command.Parameters.Add(new SqlParameter("@carcassColor", SqlDbType.NVarChar)).Value = order.CarcassColor;
                command.Parameters.Add(new SqlParameter("@implantSystem", SqlDbType.NVarChar)).Value = order.ImplantSystem;
                command.Parameters.Add(new SqlParameter("@individualAbutmentProcessing", SqlDbType.NVarChar)).Value = order.IndividualAbutmentProcessing;
                command.Parameters.Add(new SqlParameter("@understaff", SqlDbType.NVarChar)).Value = order.Understaff;
                command.Parameters.Add(new SqlParameter("@workOrderId", SqlDbType.Int) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new SqlParameter("@docNumber", SqlDbType.NVarChar) { Direction = ParameterDirection.Output, Size = 10 });

                command.ExecuteNonQuery();

                order.WorkOrderId = command.Parameters["@workOrderId"].Value.ToInt();
                order.DocNumber = command.Parameters["@docNumber"].Value.ToString();

                return order;
            }
        }

        public void UpdateOrder(OrderDto order)
        {
            var cmdText = "";

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    switch (order.BranchType)
                    {
                        case BranchType.Laboratory:
                            UpdateWorkOrderDL(order, connection);
                            break;

                        case BranchType.MillingCenter:
                            UpdateWorkOrderMC(order, connection);
                            break;

                        default:
                            throw new NotSupportedException("Неизвестный тип филиала");
                    }

                    AddOrUpdateToothCard(order.ToothCard, connection);
                }
            }
        }

        public ProstheticConditionDto[] GetProstheticConditions()
        {
            var cmdText = "select * from GetConditionsOfProsthetics()";

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var prostheticConditionEntities = new List<ProstheticConditionEntity>();
                    while (reader.Read())
                    {
                        var prostheticConditionEntity = new ProstheticConditionEntity();
                        prostheticConditionEntity.ConditionId = int.Parse(reader[nameof(prostheticConditionEntity.ConditionId)].ToString());
                        prostheticConditionEntity.ConditionName = reader[nameof(prostheticConditionEntity.ConditionName)].ToString().Trim();

                        prostheticConditionEntities.Add(prostheticConditionEntity);
                    }
                    reader.Close();

                    return prostheticConditionEntities.Select(x => _converter.ConvertToProstheticCondition(x)).ToArray();
                }
            }
        }

        private static void UpdateWorkOrderMC(OrderDto order, SqlConnection connection)
        {
            using (var command = new SqlCommand("UpdateWorkOrderMC", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@workOrderId", SqlDbType.Int)).Value = order.WorkOrderId;
                command.Parameters.Add(new SqlParameter("@status", SqlDbType.SmallInt)).Value = order.Status;
                command.Parameters.Add(new SqlParameter("@docNumber", SqlDbType.NVarChar)).Value = order.DocNumber;
                command.Parameters.Add(new SqlParameter("@customerName", SqlDbType.NVarChar)).Value = order.Customer;

                // TODO: Разобраться с ебаными датами!!! Блджад!
                command.Parameters.Add(new SqlParameter("@dateDelivery", SqlDbType.DateTime)).Value = DBNull.Value;
                //command.Parameters.Add(new SqlParameter("@dateOfCompletion", SqlDbType.NVarChar)).Value = DBNull.Value;

                command.Parameters.Add(new SqlParameter("@flagWorkAccept", SqlDbType.Bit)).Value = order.WorkAccepted;
                command.Parameters.Add(new SqlParameter("@workDescription", SqlDbType.NVarChar)).Value = order.WorkDescription;
                command.Parameters.Add(new SqlParameter("@officeAdminName", SqlDbType.NVarChar)).Value = order.OfficeAdminName;
                command.Parameters.Add(new SqlParameter("@closed", SqlDbType.DateTime)).Value = DBNull.Value;
                command.Parameters.Add(new SqlParameter("@technicFullName", SqlDbType.NVarChar)).Value = order.ResponsiblePerson;
                command.Parameters.Add(new SqlParameter("@technicPhone", SqlDbType.NVarChar)).Value = order.ResponsiblePersonPhone.GetValueOrDbNull();
                command.Parameters.Add(new SqlParameter("@additionalInfo", SqlDbType.NVarChar)).Value = order.AdditionalInfo;
                command.Parameters.Add(new SqlParameter("@carcassColor", SqlDbType.NVarChar)).Value = order.CarcassColor;
                command.Parameters.Add(new SqlParameter("@implantSystem", SqlDbType.NVarChar)).Value = order.ImplantSystem.GetValueOrDbNull();
                command.Parameters.Add(new SqlParameter("@individualAbutmentProcessing", SqlDbType.NVarChar)).Value = order.IndividualAbutmentProcessing;
                command.Parameters.Add(new SqlParameter("@understaff", SqlDbType.NVarChar)).Value = order.Understaff;
                command.Parameters.Add(new SqlParameter("@prostheticArticul", SqlDbType.NVarChar)).Value = order.ProstheticArticul;

                command.ExecuteNonQuery();
            }
        }

        private static void UpdateWorkOrderDL(OrderDto order, SqlConnection connection)
        {
            using (var command = new SqlCommand("UpdateWorkOrderDL", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@workOrderId", SqlDbType.Int)).Value = order.WorkOrderId;
                command.Parameters.Add(new SqlParameter("@status", SqlDbType.SmallInt)).Value = order.Status;
                command.Parameters.Add(new SqlParameter("@docNumber", SqlDbType.NVarChar)).Value = order.DocNumber;
                command.Parameters.Add(new SqlParameter("@customerName", SqlDbType.NVarChar)).Value = order.Customer;

                // TODO: Разобраться с ебаными датами!!! Блджад!
                command.Parameters.Add(new SqlParameter("@dateDelivery", SqlDbType.DateTime)).Value = DBNull.Value;
                command.Parameters.Add(new SqlParameter("@dateOfCompletion", SqlDbType.NVarChar)).Value = DBNull.Value;

                command.Parameters.Add(new SqlParameter("@flagWorkAccept", SqlDbType.Bit)).Value = order.WorkAccepted;
                command.Parameters.Add(new SqlParameter("@workDescription", SqlDbType.NVarChar)).Value = order.WorkDescription;
                command.Parameters.Add(new SqlParameter("@officeAdminName", SqlDbType.NVarChar)).Value = order.OfficeAdminName;
                command.Parameters.Add(new SqlParameter("@closed", SqlDbType.DateTime)).Value = DBNull.Value;
                command.Parameters.Add(new SqlParameter("@doctorFullName", SqlDbType.NVarChar)).Value = order.ResponsiblePerson;
                command.Parameters.Add(new SqlParameter("@patientFullName", SqlDbType.NVarChar)).Value = order.Patient;
                command.Parameters.Add(new SqlParameter("@patientAge", SqlDbType.SmallInt)).Value = order.Age;
                command.Parameters.Add(new SqlParameter("@transparenceID", SqlDbType.Int)).Value = order.Transparency;

                command.Parameters.Add(new SqlParameter("@fittingDate", SqlDbType.DateTime)).Value = order.FittingDate.GetValueOrDbNull();
                command.Parameters.Add(new SqlParameter("@colorAndFeatures", SqlDbType.NVarChar)).Value = order.ColorAndFeatures;
                command.Parameters.Add(new SqlParameter("@prostheticArticul", SqlDbType.NVarChar)).Value = order.ProstheticArticul;

                command.ExecuteNonQuery();
            }
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
                        materialEntity.MaterialName = reader[nameof(materialEntity.MaterialName)].ToString().Trim();

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

        public ProstheticsTypeDto[] GetProstheticTypes()
        {
            var cmdText = "select * from GetTypesOfProsthetics()";

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var prostheticTypeEntities = new List<ProstheticTypeEntity>();
                    while (reader.Read())
                    {
                        var prostheticTypeEntity = new ProstheticTypeEntity();
                        prostheticTypeEntity.ProstheticsId = int.Parse(reader[nameof(prostheticTypeEntity.ProstheticsId)].ToString());
                        prostheticTypeEntity.ProstheticsName = reader[nameof(prostheticTypeEntity.ProstheticsName)].ToString().Trim();

                        prostheticTypeEntities.Add(prostheticTypeEntity);
                    }
                    reader.Close();

                    return prostheticTypeEntities.Select(x => _converter.ConvertToProstheticType(x)).ToArray();
                }
            }
        }

        public OrderDto GetOrderDetails(int id)
        {
            var orderDto = GetWorkOrderById(id);
            orderDto.ToothCard = GetToothCard(id);
            return orderDto;
        }

        private OrderDto GetWorkOrderById(int id)
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
                            DoctorFullName = reader["DoctorFullName"].ToString(),
                            TechnicFullName = reader["TechnicFullName"].ToString(),
                            TechnicPhone = reader["TechnicPhone"].ToString(),
                            //PatientGender = reader["PatientGender"].ToBool(),
                            Age = reader["PatientAge"].ToInt(),
                            Transparency = reader["TransparenceId"].ToInt(),
                            ProstheticArticul = reader["ProstheticArticul"].ToString(),
                            MaterialsEnum = reader["MaterialsEnum"].ToString(),
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

        private ToothDto[] GetToothCard(int id)
        {
            var cmdText = string.Format("select * from GetToothCardByWOId({0})", id);

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();

                    var toothCollection = new List<ToothDto>();
                    while (reader.Read())
                    {
                        var toothEntity = new ToothEntity();

                        toothEntity.ToothNumber = reader[nameof(toothEntity.ToothNumber)].ToInt();
                        toothEntity.MaterialName = reader[nameof(toothEntity.MaterialName)].ToString();
                        toothEntity.ConditionName = reader[nameof(toothEntity.ConditionName)].ToString();
                        toothEntity.ProstheticsName = reader[nameof(toothEntity.ProstheticsName)].ToString();
                        toothEntity.FlagBridge = reader[nameof(toothEntity.FlagBridge)].ToBool();

                        toothCollection.Add(_converter.ConvertToTooth(toothEntity));
                    }

                    reader.Close();

                    return toothCollection.ToArray();
                }
            }
        }

        public OrderLiteDto[] GetOrders(OrdersFilter filter)
        {
            if (filter.IsEmpty())
                return GetOrdersByEmptyFilter();

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
            var cmdTextDefault = $"SELECT * FROM GetWorkOrdersList(@branchTypeId, default, default, default, @customerName, @patientFullName, @doctorFullName, @createDateFrom, createDateTo, default, default)";
            var cmdTextAdditional = $"WHERE WorkOrderID IN (SELECT * FROM GetWorkOrderIdForMaterialSelect(@materialSet))";

            if (filter.Materials.Length == 0)
            {
                return cmdTextDefault;
            }

            return cmdTextDefault + cmdTextAdditional;
        }

        private OrderLiteDto[] GetOrdersByEmptyFilter()
        {
            var cmdText = "select * from GetWorkOrdersList(default, default, default, default, default, default, default, default, default, default, default)";

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    return GetOrderLiteCollectionFromReader(command);
                }
            }
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
                    orderLite.DocNumber = reader[nameof(orderLite.DocNumber)].ToString();
                    orderLite.Created = DateTime.Parse(reader[nameof(orderLite.Created)].ToString());

                    var closed = reader[nameof(orderLite.Closed)];
                    if (closed != DBNull.Value)
                        orderLite.Closed = DateTime.Parse(closed.ToString());

                    orderLiteEntities.Add(orderLite);
                }

                var orders = orderLiteEntities.Select(x => _converter.ConvertToOrderLite(x)).ToArray();
                return orders;
            }
        }
        public TransparencesDto[] GetTransparences()
        {
            var cmdText = "select * from GetTransparencesList()";
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var commamd = new SqlCommand(cmdText, connection))
                {
                    var reader = commamd.ExecuteReader();
                    var transparencesEntities = new List<TransparencesEntity>();
                    while (reader.Read())
                    {
                        var transparenceEntity = new TransparencesEntity();
                        transparenceEntity.TransparenceId = int.Parse(reader[nameof(transparenceEntity.TransparenceId)].ToString());
                        transparenceEntity.TransparenceName = reader[nameof(transparenceEntity.TransparenceName)].ToString();

                        transparencesEntities.Add(transparenceEntity);
                    }
                    var transparences = transparencesEntities.Select(x => _converter.ConvertToTransparences(x)).ToArray();
                    return transparences;
                }
            }
        }
        public EquipmentDto[] GetEquipment()
        {
            var cmdText = "select * from GetEquipmentsList()";
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var commamd = new SqlCommand(cmdText, connection))
                {
                    var reader = commamd.ExecuteReader();
                    var equipmentEntities = new List<EquipmentEntity>();
                    while (reader.Read())
                    {
                        var equipmentEntity = new EquipmentEntity();
                        equipmentEntity.EquipmentId = int.Parse(reader[nameof(equipmentEntity.EquipmentId)].ToString());
                        equipmentEntity.EquipmentName = reader[nameof(equipmentEntity.EquipmentName)].ToString();

                        equipmentEntities.Add(equipmentEntity);
                    }
                    var equipment = equipmentEntities.Select(x => _converter.ConvertToEquipment(x)).ToArray();
                    return equipment;
                }
            }
        }
    }
}
