﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Common.FileSystem;
using Germadent.Rma.Model;
using Germadent.WebApi.Configuration;
using Germadent.WebApi.Entities;
using Germadent.WebApi.Entities.Conversion;
using Newtonsoft.Json;

namespace Germadent.WebApi.Repository
{
    public class RmaDbOperations : IRmaDbOperations
    {
        private readonly IEntityToDtoConverter _converter;
        private readonly IServiceConfiguration _configuration;
        private readonly IFileManager _fileManager;

        private readonly string _storageDirectory;

        public RmaDbOperations(IEntityToDtoConverter converter, IServiceConfiguration configuration, IFileManager fileManager)
        {
            _converter = converter;
            _configuration = configuration;
            _fileManager = fileManager;

            _storageDirectory = GetFileTableFullPath();
        }

        public OrderDto AddOrder(OrderDto order, Stream stream)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                OrderDto outputOrder;
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
                SaveOrderDataFile(order, connection, stream);

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
                command.Parameters.Add(new SqlParameter("@responsiblePersonId", SqlDbType.Int)).Value = DBNull.Value;
                command.Parameters.Add(new SqlParameter("@patientFullName", SqlDbType.NVarChar)).Value = order.Patient;
                command.Parameters.Add(new SqlParameter("@patientGender", SqlDbType.TinyInt)).Value = (int)order.Gender;
                command.Parameters.Add(new SqlParameter("@patientAge", SqlDbType.TinyInt)).Value = order.Age;
                command.Parameters.Add(new SqlParameter("@dateComment", SqlDbType.NVarChar)).Value = order.DateComment;
                command.Parameters.Add(new SqlParameter("@workDescription", SqlDbType.NVarChar)).Value = order.WorkDescription;
                command.Parameters.Add(new SqlParameter("@officeAdminId", SqlDbType.Int)).Value = DBNull.Value;
                command.Parameters.Add(new SqlParameter("@officeAdminName", SqlDbType.NVarChar)).Value = DBNull.Value;
                command.Parameters.Add(new SqlParameter("@transparenceId", SqlDbType.Int)).Value = order.Transparency;
                command.Parameters.Add(new SqlParameter("@fittingDate", SqlDbType.DateTime)).Value = order.FittingDate;
                command.Parameters.Add(new SqlParameter("@dateOfCompletion", SqlDbType.DateTime)).Value = order.DateOfCompletion;
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
                command.Parameters.Add(new SqlParameter("@customerId", SqlDbType.Int)).Value = DBNull.Value; //только не NULL!!!
                //command.Parameters.Add(new SqlParameter("@customerName", SqlDbType.NVarChar)).Value = order.Customer; //больше не нужен
                command.Parameters.Add(new SqlParameter("@responsiblePersonId", SqlDbType.Int)).Value = DBNull.Value;
                //command.Parameters.Add(new SqlParameter("@technicFullName", SqlDbType.NVarChar)).Value = order.ResponsiblePerson; //больше не нужен
                //command.Parameters.Add(new SqlParameter("@technicPhone", SqlDbType.NVarChar)).Value = order.ResponsiblePersonPhone; //больше не нужен
                //command.Parameters.Add(new SqlParameter("@patientID", SqlDbType.NVarChar)).Value = DBNull.Value; //справочника пациентов не будет
                command.Parameters.Add(new SqlParameter("@patientFullName", SqlDbType.NVarChar)).Value = order.Patient;
                command.Parameters.Add(new SqlParameter("@dateComment", SqlDbType.NVarChar)).Value = order.DateComment;
                command.Parameters.Add(new SqlParameter("@prostheticArticul", SqlDbType.NVarChar)).Value = order.ProstheticArticul;
                command.Parameters.Add(new SqlParameter("@workDescription", SqlDbType.NVarChar)).Value = order.WorkDescription;
                command.Parameters.Add(new SqlParameter("@officeAdminId", SqlDbType.Int)).Value = DBNull.Value;
                command.Parameters.Add(new SqlParameter("@officeAdminName", SqlDbType.NVarChar)).Value = DBNull.Value;
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
            }

            order.AdditionalEquipment.ForEach(x => x.WorkOrderId = order.WorkOrderId);
            AddOrUpdateAdditionalEquipmentInWO(order, connection);

            return order;
        }

        public void UpdateOrder(OrderDto order, Stream stream)
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
                    SaveOrderDataFile(order, connection, stream);
                }
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

        public DictionaryItemDto[] GetDictionary(DictionaryType dictionaryType)
        {
            switch (dictionaryType)
            {
                case DictionaryType.Equipment:
                    return GetEquipment();

                case DictionaryType.Material:
                    return GetMaterials();

                case DictionaryType.ProstheticCondition:
                    return GetProstheticConditions();

                case DictionaryType.ProstheticType:
                    return GetProstheticTypes();

                case DictionaryType.Transparency:
                    return GetTransparences();

                default:
                    throw new NotImplementedException("Неизвестный тип словаря");
            }
        }

        private void SaveOrderDataFile(OrderDto order, SqlConnection connection, Stream stream)
        {
            if (stream == null)
                return;

            var fileName = Path.GetFileName(order.DataFileName);
            var resultFileName = Path.Combine(_storageDirectory, fileName);
            var fileInfo = _fileManager.Save(stream, resultFileName);
            LinkFileToWorkOrder(order.WorkOrderId, fileName, fileInfo.CreationTime, connection);
        }

        private void LinkFileToWorkOrder(int workOrderId, string fileName, DateTime creationTime, SqlConnection connection)
        {
            var cmdText = "AddLinkWO_FileStream";
            using (var command = new SqlCommand(cmdText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@fileName", SqlDbType.NVarChar)).Value = fileName;
                command.Parameters.Add(new SqlParameter("@creationTime", SqlDbType.DateTimeOffset)).Value = creationTime;
                command.Parameters.Add(new SqlParameter("@workOrderId", SqlDbType.Int)).Value = workOrderId;

                command.ExecuteNonQuery();
            }
        }

        private string GetFileTableFullPath()
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                var cmdText = "select dbo.GetFileTableFullPath()";
                using (var command = new SqlCommand(cmdText, connection))
                {
                    var commandResult = command.ExecuteScalar();
                    return commandResult.ToString();
                }
            }
        }

        public DictionaryItemDto[] GetProstheticConditions()
        {
            var cmdText = "select * from GetConditionsOfProsthetics()";

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var prostheticConditionEntities = new List<DictionaryItemEntity>();
                    while (reader.Read())
                    {
                        var prostheticConditionEntity = new DictionaryItemEntity();
                        prostheticConditionEntity.Id = int.Parse(reader["ConditionId"].ToString());
                        prostheticConditionEntity.Name = reader["ConditionName"].ToString().Trim();
                        prostheticConditionEntity.DictionaryName = DictionaryType.ProstheticCondition.GetDescription();

                        prostheticConditionEntities.Add(prostheticConditionEntity);
                    }
                    reader.Close();

                    return prostheticConditionEntities.Select(x => _converter.ConvertToDictionaryItem(x)).ToArray();
                }
            }
        }

        private static void UpdateWorkOrderMC(OrderDto order, SqlConnection connection)
        {
            using (var command = new SqlCommand("UpdateWorkOrderMC", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@workOrderId", SqlDbType.Int)).Value = order.WorkOrderId;
                command.Parameters.Add(new SqlParameter("@docNumber", SqlDbType.NVarChar)).Value = order.DocNumber;
                command.Parameters.Add(new SqlParameter("@patientFullName", SqlDbType.NVarChar)).Value = order.Patient;
                command.Parameters.Add(new SqlParameter("@dateDelivery", SqlDbType.DateTime)).Value = DBNull.Value;
                command.Parameters.Add(new SqlParameter("@flagWorkAccept", SqlDbType.Bit)).Value = order.WorkAccepted;
                command.Parameters.Add(new SqlParameter("@workDescription", SqlDbType.NVarChar)).Value = order.WorkDescription.GetValueOrDbNull();
                command.Parameters.Add(new SqlParameter("@officeAdminName", SqlDbType.NVarChar)).Value = order.OfficeAdminName;
                command.Parameters.Add(new SqlParameter("@additionalInfo", SqlDbType.NVarChar)).Value = order.AdditionalInfo;
                command.Parameters.Add(new SqlParameter("@carcassColor", SqlDbType.NVarChar)).Value = order.CarcassColor;
                command.Parameters.Add(new SqlParameter("@implantSystem", SqlDbType.NVarChar)).Value = order.ImplantSystem.GetValueOrDbNull();
                command.Parameters.Add(new SqlParameter("@individualAbutmentProcessing", SqlDbType.NVarChar)).Value = order.IndividualAbutmentProcessing;
                command.Parameters.Add(new SqlParameter("@understaff", SqlDbType.NVarChar)).Value = order.Understaff;
                command.Parameters.Add(new SqlParameter("@prostheticArticul", SqlDbType.NVarChar)).Value = order.ProstheticArticul;
                command.Parameters.Add(new SqlParameter("@dateComment", SqlDbType.NVarChar)).Value = order.DateComment;

                command.ExecuteNonQuery();
            }

            AddOrUpdateAdditionalEquipmentInWO(order, connection);
        }

        private static void AddOrUpdateAdditionalEquipmentInWO(OrderDto order, SqlConnection connection)
        {
            using (var command = new SqlCommand("AddOrUpdateAdditionalEquipmentInWO", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                var jsonEquipments = order.AdditionalEquipment.SerializeToJson();

                command.Parameters.Add(new SqlParameter("@jsonEquipments", SqlDbType.NVarChar)).Value = jsonEquipments;

                command.ExecuteNonQuery();
            }
        }

        private static void UpdateWorkOrderDL(OrderDto order, SqlConnection connection)
        {
            using (var command = new SqlCommand("UpdateWorkOrderDL", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@workOrderId", SqlDbType.Int)).Value = order.WorkOrderId;
                command.Parameters.Add(new SqlParameter("@docNumber", SqlDbType.NVarChar)).Value = order.DocNumber;
                command.Parameters.Add(new SqlParameter("@dateDelivery", SqlDbType.DateTime)).Value = DBNull.Value;
                command.Parameters.Add(new SqlParameter("@flagWorkAccept", SqlDbType.Bit)).Value = order.WorkAccepted;
                command.Parameters.Add(new SqlParameter("@workDescription", SqlDbType.NVarChar)).Value = order.WorkDescription.GetValueOrDbNull();
                command.Parameters.Add(new SqlParameter("@officeAdminName", SqlDbType.NVarChar)).Value = order.OfficeAdminName;
                command.Parameters.Add(new SqlParameter("@patientFullName", SqlDbType.NVarChar)).Value = order.Patient;
                command.Parameters.Add(new SqlParameter("@patientGender", SqlDbType.Bit)).Value = (int)order.Gender;
                command.Parameters.Add(new SqlParameter("@patientAge", SqlDbType.SmallInt)).Value = order.Age;
                command.Parameters.Add(new SqlParameter("@transparenceID", SqlDbType.Int)).Value = order.Transparency;
                command.Parameters.Add(new SqlParameter("@fittingDate", SqlDbType.DateTime)).Value = order.FittingDate.GetValueOrDbNull();
                command.Parameters.Add(new SqlParameter("@colorAndFeatures", SqlDbType.NVarChar)).Value = order.ColorAndFeatures;
                command.Parameters.Add(new SqlParameter("@prostheticArticul", SqlDbType.NVarChar)).Value = order.ProstheticArticul;
                command.Parameters.Add(new SqlParameter("@dateComment", SqlDbType.NVarChar)).Value = order.DateComment;
                command.Parameters.Add(new SqlParameter("@dateOfCompletion", SqlDbType.NVarChar)).Value = order.DateOfCompletion.GetValueOrDbNull();

                command.ExecuteNonQuery();
            }
        }

        public DictionaryItemDto[] GetMaterials()
        {
            var cmdText = "select * from GetMaterialsList()";

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var materials = new List<DictionaryItemEntity>();
                    while (reader.Read())
                    {
                        var materialEntity = new DictionaryItemEntity();
                        materialEntity.Id = int.Parse(reader["MaterialId"].ToString());
                        materialEntity.Name = reader["MaterialName"].ToString().Trim();
                        materialEntity.DictionaryName = DictionaryType.Material.GetDescription();

                        //if (bool.TryParse(reader[nameof(materialEntity.FlagUnused)].ToString(), out bool flagUnused))
                        //{
                        //    materialEntity.FlagUnused = flagUnused;
                        //}

                        materials.Add(materialEntity);
                    }
                    reader.Close();

                    return materials.Select(x => _converter.ConvertToDictionaryItem(x)).ToArray();
                }
            }
        }

        public DictionaryItemDto[] GetProstheticTypes()
        {
            var cmdText = "select * from GetTypesOfProsthetics()";

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var prostheticTypeEntities = new List<DictionaryItemEntity>();
                    while (reader.Read())
                    {
                        var prostheticTypeEntity = new DictionaryItemEntity();
                        prostheticTypeEntity.Id = int.Parse(reader["ProstheticsId"].ToString());
                        prostheticTypeEntity.Name = reader["ProstheticsName"].ToString().Trim();
                        prostheticTypeEntity.DictionaryName = DictionaryType.ProstheticType.GetDescription();

                        prostheticTypeEntities.Add(prostheticTypeEntity);
                    }
                    reader.Close();

                    return prostheticTypeEntities.Select(x => _converter.ConvertToDictionaryItem(x)).ToArray();
                }
            }
        }

        public OrderDto GetOrderDetails(int id)
        {
            var orderDto = GetWorkOrderById(id);
            orderDto.ToothCard = GetToothCard(id);
            FillHasDataFile(orderDto);
            return orderDto;
        }

        private void FillHasDataFile(OrderDto order)
        {
            var cmdText = string.Format("select * from GetFileAttributesByWOId({0})", order.WorkOrderId);

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var dataFileAttributes = new DataFileAttributes();
                    while (reader.Read())
                    {
                        dataFileAttributes.FileName = reader["name"].ToString();
                        dataFileAttributes.StreamId = reader["stream_id"].ToGuid();
                    }

                    if (dataFileAttributes.FileName == null)
                        return;

                    order.DataFileName = dataFileAttributes.FileName;
                }
            }
        }

        public Stream GetFileByWorkOrder(int id)
        {
            var cmdText = string.Format("select * from GetFileAttributesByWOId({0})", id);

            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var dataFileAttributes = new DataFileAttributes();
                    while (reader.Read())
                    {
                        dataFileAttributes.FileName = reader["name"].ToString();
                        dataFileAttributes.StreamId = reader["stream_id"].ToGuid();
                    }

                    if (dataFileAttributes.FileName == null)
                        return null;

                    var fullPathToDataFile = Path.Combine(_storageDirectory, dataFileAttributes.FileName);
                    return _fileManager.OpenFile(fullPathToDataFile);
                }
            }
        }

        private OrderDto GetWorkOrderById(int id)
        {
            var cmdText = string.Format("select * from GetWorkOrderById({0})", id);

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
                            PatientGender = reader["PatientGender"].ToBool(),
                            Age = reader["PatientAge"].ToInt(),
                            Transparency = reader["TransparenceId"].ToInt(),
                            ProstheticArticul = reader["ProstheticArticul"].ToString(),
                            MaterialsEnum = reader["MaterialsEnum"].ToString(),
                            DateComment = reader["DateComment"].ToString(),
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

                order.AdditionalEquipment = GetAdditionalEquipmentByWorkOrder(order, connection);
                return order;
            }
        }

        private AdditionalEquipmentDto[] GetAdditionalEquipmentByWorkOrder(OrderDto order, SqlConnection connection)
        {
            var cmdText = "select * from GetAdditionalEquipment(@workOrderId)";

            using (var command = new SqlCommand(cmdText, connection))
            {
                command.Parameters.Add(new SqlParameter("@workOrderId", SqlDbType.Int)).Value = order.WorkOrderId;
                using (var reader = command.ExecuteReader())
                {
                    var additionalEquipmentEntities = new List<AdditionalEquipmentEntity>();
                    while (reader.Read())
                    {
                        var entity = new AdditionalEquipmentEntity();
                        entity.EquipmentId = reader[nameof(entity.EquipmentId)].ToInt();
                        entity.WorkOrderId = reader[nameof(entity.WorkOrderId)].ToInt();
                        entity.EquipmentName = reader[nameof(entity.EquipmentName)].ToString();
                        entity.Quantity = reader[nameof(entity.Quantity)].ToInt();
                        additionalEquipmentEntities.Add(entity);
                    }

                    return additionalEquipmentEntities.Select(x => _converter.ConvertToAdditionalEquipment(x)).ToArray();
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
            var cmdTextDefault = $"SELECT * FROM GetWorkOrdersList(@branchTypeId, default, default, default, @customerName, @patientFullName, @doctorFullName, @createDateFrom, @createDateTo, default, default)";
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
                    orderLite.DoctorFullName = reader[nameof(orderLite.DoctorFullName)].ToString();
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
        public DictionaryItemDto[] GetTransparences()
        {
            var cmdText = "select * from GetTransparencesList()";
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var commamd = new SqlCommand(cmdText, connection))
                {
                    var reader = commamd.ExecuteReader();
                    var transparencesEntities = new List<DictionaryItemEntity>();
                    while (reader.Read())
                    {
                        var transparenceEntity = new DictionaryItemEntity();
                        transparenceEntity.Id = int.Parse(reader["TransparenceId"].ToString());
                        transparenceEntity.Name = reader["TransparenceName"].ToString();
                        transparenceEntity.DictionaryName = DictionaryType.Transparency.GetDescription();

                        transparencesEntities.Add(transparenceEntity);
                    }
                    var transparences = transparencesEntities.Select(x => _converter.ConvertToDictionaryItem(x)).ToArray();
                    return transparences;
                }
            }
        }
        public DictionaryItemDto[] GetEquipment()
        {
            var cmdText = "select * from GetEquipmentsList()";
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var commamd = new SqlCommand(cmdText, connection))
                {
                    var reader = commamd.ExecuteReader();
                    var equipmentEntities = new List<DictionaryItemEntity>();
                    while (reader.Read())
                    {
                        var equipmentEntity = new DictionaryItemEntity();
                        equipmentEntity.Id = int.Parse(reader["EquipmentId"].ToString());
                        equipmentEntity.Name = reader["EquipmentName"].ToString();
                        equipmentEntity.DictionaryName = DictionaryType.Equipment.GetDescription();

                        equipmentEntities.Add(equipmentEntity);
                    }
                    var equipment = equipmentEntities.Select(x => _converter.ConvertToDictionaryItem(x)).ToArray();
                    return equipment;
                }
            }
        }

        public ReportListDto[] GetWorkReport(int id)
        {
            var orderDto = GetWorkOrderById(id);

            var toothCard = GetToothCard(id);

            var prosteticsCollection = toothCard
                .GroupBy(x => x.ProstheticsName)
                .Select(x => new { quantity = x.Count(), pName = x.Key })
                .ToArray();

            var equipmentNames = new[]
            {
                "STL", "Слепок", "Модель"
            };

            var equipments = orderDto.AdditionalEquipment
                .Where(x => equipmentNames.Contains(x.EquipmentName))
                .Select(x => x.EquipmentName)
                .ToArray();

            var reports = new List<ReportListDto>();
            foreach (var prosthetics in prosteticsCollection)
            {
                var entity = new ExcelEntity
                {
                    Created = orderDto.Created,
                    DocNumber = orderDto.DocNumber,
                    Customer = orderDto.Customer,
                    EquipmentSubstring = string.Join("; ", equipments),
                    Patient = orderDto.Patient,
                    ProstheticSubstring = prosthetics.pName,
                    MaterialsStr = orderDto.MaterialsStr,
                    ColorAndFeatures = orderDto.ColorAndFeatures,
                    Quantity = prosthetics.quantity,
                    ProstheticArticul = orderDto.ProstheticArticul
                };
                var reportDto = _converter.ConvertToExcel(entity);
                reports.Add(reportDto);
            }

            return reports.ToArray();

        }

        public CustomerDto[] GetCustomers(string name)
        {
            var cmdText = string.Format("select * from GetCustomers(default, '{0}')", name);
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var commamd = new SqlCommand(cmdText, connection))
                {
                    var reader = commamd.ExecuteReader();
                    var customerEntities = new List<CustomerEntity>();
                    while (reader.Read())
                    {
                        var customerEntity = new CustomerEntity();
                        customerEntity.CustomerId = int.Parse(reader[nameof(customerEntity.CustomerId)].ToString());
                        customerEntity.CustomerName = reader[nameof(customerEntity.CustomerName)].ToString();
                        customerEntity.CustomerPhone = reader[nameof(customerEntity.CustomerPhone)].ToString();
                        customerEntity.CustomerEmail = reader[nameof(customerEntity.CustomerEmail)].ToString();
                        customerEntity.CustomerWebSite = reader[nameof(customerEntity.CustomerWebSite)].ToString();
                        customerEntity.CustomerDescription = reader[nameof(customerEntity.CustomerDescription)].ToString();

                        customerEntities.Add(customerEntity);
                    }

                    var customers = customerEntities.Select(x => _converter.ConvertToCustomer(x)).ToArray();
                    return customers;
                }
            }
        }

        public ResponsiblePersonDto[] GetResponsiblePersons(int customerId)
        {
            var cmdText = string.Format("select * from GetResponsiblePersons(default, {0}, default)", customerId);
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var commamd = new SqlCommand(cmdText, connection))
                {
                    var reader = commamd.ExecuteReader();
                    var responsiblePersonEntities = new List<ResponsiblePersonEntity>();
                    while (reader.Read())
                    {
                        var rpEntity = new ResponsiblePersonEntity();
                        rpEntity.ResponsiblePersonId = int.Parse(reader[nameof(rpEntity.ResponsiblePersonId)].ToString());
                        rpEntity.CustomerId = int.Parse(reader[nameof(rpEntity.CustomerId)].ToString());
                        rpEntity.ResponsiblePerson = reader[nameof(rpEntity.ResponsiblePerson)].ToString();
                        rpEntity.RP_Position = reader[nameof(rpEntity.RP_Position)].ToString();
                        rpEntity.RP_Phone = reader[nameof(rpEntity.RP_Phone)].ToString();
                        rpEntity.RP_Email = reader[nameof(rpEntity.RP_Email)].ToString();
                        rpEntity.RP_Description = reader[nameof(rpEntity.RP_Description)].ToString();

                        responsiblePersonEntities.Add(rpEntity);
                    }
                    var responsiblePersons = responsiblePersonEntities.Select(x => _converter.ConvertToResponsiblePerson(x)).ToArray();
                    return responsiblePersons;
                }
            }
        }

        public CustomerDto AddCustomer(CustomerDto customer)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("AddNewCustomer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@customerName", SqlDbType.NVarChar)).Value = customer.Name;
                    command.Parameters.Add(new SqlParameter("@customerPhone", SqlDbType.NVarChar)).Value = customer.Phone.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@customerEmail", SqlDbType.NVarChar)).Value = customer.Email.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@customerWebsite", SqlDbType.NVarChar)).Value = customer.WebSite.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@customerDescription", SqlDbType.NVarChar)).Value = customer.Description.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@customerId", SqlDbType.Int) { Direction = ParameterDirection.Output });

                    command.ExecuteNonQuery();

                    customer.Id = command.Parameters["@customerId"].Value.ToInt();

                    return customer;
                }
            }
        }
    }
}