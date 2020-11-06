using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Common.FileSystem;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;
using Germadent.WebApi.Configuration;
using Germadent.WebApi.Entities;
using Germadent.WebApi.Entities.Conversion;
using Newtonsoft.Json;

namespace Germadent.WebApi.DataAccess.Rma
{
    public class RmaDbOperations : IRmaDbOperations
    {
        private readonly IAddWorOrderCommand _addWorOrderCommand;
        private readonly IRmaEntityConverter _converter;
        private readonly IServiceConfiguration _configuration;
        private readonly IFileManager _fileManager;

        private readonly string _storageDirectory;

        public RmaDbOperations(IAddWorOrderCommand addWorOrderCommand, IRmaEntityConverter converter, IServiceConfiguration configuration, IFileManager fileManager)
        {
            _addWorOrderCommand = addWorOrderCommand;
            _converter = converter;
            _configuration = configuration;
            _fileManager = fileManager;

            _storageDirectory = GetFileTableFullPath();
        }

        public OrderDto AddOrder(OrderDto order)
        {
            return _addWorOrderCommand.Execute(order);
        }

        public void AttachDataFileToOrder(int id, string fileName, Stream stream)
        {
            var resultFileName = Path.Combine(_storageDirectory, fileName);
            var fileInfo = _fileManager.Save(stream, resultFileName);
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                LinkFileToWorkOrder(id, fileName, fileInfo.CreationTime, connection);
            }
        }

        private void AddOrUpdateToothCard(ToothDto[] toothCard, SqlConnection connection)
        {
            var toothEntities = toothCard.SelectMany(x => _converter.ConvertFromToothDto(x)).ToArray();
            var toothCardJson = toothEntities.SerializeToJson(Formatting.Indented);

            var cmdText = "AddOrUpdateToothCardInWO";

            using (var command = new SqlCommand(cmdText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@jsonString", SqlDbType.NVarChar)).Value = toothCardJson;

                command.ExecuteNonQuery();
            }
        }

        public void UpdateOrder(OrderDto order)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                UpdateWorkOrder(order, connection);

                AddOrUpdateToothCard(order.ToothCard, connection);
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

        private void LinkFileToWorkOrder(int userId, string fileName, DateTime creationTime, SqlConnection connection)
        {
            var cmdText = "AddLink_User_FileStream";
            using (var command = new SqlCommand(cmdText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@fileName", SqlDbType.NVarChar)).Value = fileName;
                command.Parameters.Add(new SqlParameter("@creationTime", SqlDbType.DateTimeOffset)).Value = creationTime;
                command.Parameters.Add(new SqlParameter("@userId", SqlDbType.Int)).Value = userId;

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

        private static void UpdateWorkOrder(OrderDto order, SqlConnection connection)
        {
            using (var command = new SqlCommand("UpdateWorkOrder", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@branchTypeId", SqlDbType.Int)).Value = (int)order.BranchType;
                command.Parameters.Add(new SqlParameter("@workOrderId", SqlDbType.Int)).Value = order.WorkOrderId;
                command.Parameters.Add(new SqlParameter("@docNumber", SqlDbType.NVarChar)).Value = order.DocNumber;
                command.Parameters.Add(new SqlParameter("@customerID", SqlDbType.Int)).Value = order.CustomerId;
                command.Parameters.Add(new SqlParameter("@responsiblePersonId", SqlDbType.Int)).Value = order.ResponsiblePersonId == 0 ? (object)DBNull.Value : order.ResponsiblePersonId;
                command.Parameters.Add(new SqlParameter("@flagWorkAccept", SqlDbType.Bit)).Value = order.WorkAccepted;
                command.Parameters.Add(new SqlParameter("@dateComment", SqlDbType.NVarChar)).Value = order.DateComment;
                command.Parameters.Add(new SqlParameter("@prostheticArticul", SqlDbType.NVarChar)).Value = order.ProstheticArticul.GetValueOrDbNull();
                command.Parameters.Add(new SqlParameter("@workDescription", SqlDbType.NVarChar)).Value = order.WorkDescription.GetValueOrDbNull();
                command.Parameters.Add(new SqlParameter("@officeAdminName", SqlDbType.NVarChar)).Value = order.OfficeAdminName;
                command.Parameters.Add(new SqlParameter("@patientFullName", SqlDbType.NVarChar)).Value = order.Patient;
                command.Parameters.Add(new SqlParameter("@patientGender", SqlDbType.Bit)).Value = (int)order.Gender;
                command.Parameters.Add(new SqlParameter("@patientAge", SqlDbType.SmallInt)).Value = order.Age;
                command.Parameters.Add(new SqlParameter("@fittingDate", SqlDbType.DateTime)).Value = order.FittingDate.GetValueOrDbNull();
                command.Parameters.Add(new SqlParameter("@dateOfCompletion", SqlDbType.DateTime)).Value = order.DateOfCompletion.GetValueOrDbNull();
                command.Parameters.Add(new SqlParameter("@additionalInfo", SqlDbType.NVarChar)).Value = order.AdditionalInfo == null ? (object)DBNull.Value : order.AdditionalInfo;
                command.Parameters.Add(new SqlParameter("@carcassColor", SqlDbType.NVarChar)).Value = order.CarcassColor == null ? (object)DBNull.Value : order.CarcassColor;
                command.Parameters.Add(new SqlParameter("@implantSystem", SqlDbType.NVarChar)).Value = order.ImplantSystem.GetValueOrDbNull();
                command.Parameters.Add(new SqlParameter("@individualAbutmentProcessing", SqlDbType.NVarChar)).Value = order.IndividualAbutmentProcessing == null ? (object)DBNull.Value : order.IndividualAbutmentProcessing;
                command.Parameters.Add(new SqlParameter("@understaff", SqlDbType.NVarChar)).Value = order.Understaff == null ? (object)DBNull.Value : order.Understaff;
                command.Parameters.Add(new SqlParameter("@colorAndFeatures", SqlDbType.NVarChar)).Value = order.ColorAndFeatures == null ? (object)DBNull.Value : order.ColorAndFeatures;
                command.Parameters.Add(new SqlParameter("@created", SqlDbType.DateTime) { Direction = ParameterDirection.Output });

                command.ExecuteNonQuery();

                order.Created = command.Parameters["@created"].Value.ToDateTime();
            }

            AddOrUpdateAdditionalEquipmentInWO(order, connection);
        }

        public OrderDto GetOrderDetails(int id)
        {
            var orderDto = GetWorkOrderById(id);
            orderDto.ToothCard = GetToothCard(id);
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

        public string GetFileByWorkOrder(int id)
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
                    return fullPathToDataFile;
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
                            CustomerId = reader["CustomerId"].ToInt(),
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
                            ResponsiblePersonId = reader["ResponsiblePersonId"].ToInt(),
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

                    var entities = new List<ToothEntity>();
                    while (reader.Read())
                    {
                        var toothEntity = new ToothEntity();

                        toothEntity.WorkOrderId = reader[nameof(toothEntity.WorkOrderId)].ToInt();
                        toothEntity.ToothNumber = reader[nameof(toothEntity.ToothNumber)].ToInt();
                        toothEntity.MaterialId = reader[nameof(toothEntity.MaterialId)].ToInt();
                        toothEntity.MaterialName = reader[nameof(toothEntity.MaterialName)].ToString();
                        toothEntity.ConditionId = reader[nameof(toothEntity.ConditionId)].ToInt();
                        toothEntity.ConditionName = reader[nameof(toothEntity.ConditionName)].ToString();
                        toothEntity.ProstheticsId = reader[nameof(toothEntity.ProstheticsId)].ToInt();
                        toothEntity.ProstheticsName = reader[nameof(toothEntity.ProstheticsName)].ToString();
                        toothEntity.Price = reader[nameof(toothEntity.Price)].ToDecimal();
                        toothEntity.FlagBridge = reader[nameof(toothEntity.FlagBridge)].ToBool();
                        toothEntity.PricePositionId = reader[nameof(toothEntity.PricePositionId)].ToInt();
                        toothEntity.PricePositionCode = reader[nameof(toothEntity.PricePositionCode)].ToString();
                        toothEntity.PricePositionName = reader[nameof(toothEntity.PricePositionName)].ToString();
                        toothEntity.PriceGroupId = reader[nameof(toothEntity.PriceGroupId)].ToInt();

                        entities.Add(toothEntity);
                    }

                    reader.Close();

                    return _converter.ConvertToToothCard(entities.ToArray());
                }
            }
        }

        public PriceGroupDto[] GetPriceGroups(BranchType branchType)
        {
            var cmdText = string.Format("select distinct PriceGroupID, PriceGroupName from GetPriceListForBranch({0})", (int)branchType);
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var priceGroupCollection = new List<PriceGroupDto>();
                    while (reader.Read())
                    {
                        var priceGroupEntity = new PriceGroupEntity();

                        priceGroupEntity.PriceGroupId = reader[nameof(priceGroupEntity.PriceGroupId)].ToInt();
                        priceGroupEntity.PriceGroupName = reader[nameof(priceGroupEntity.PriceGroupName)].ToString();

                        var priceGroupDto = _converter.ConvertToPriceGroup(priceGroupEntity);
                        priceGroupDto.BranchType = branchType;
                        priceGroupCollection.Add(priceGroupDto);
                    }
                    reader.Close();

                    return priceGroupCollection.ToArray();
                }
            }
        }

        public PricePositionDto[] GetPricePositions(BranchType branchType)
        {
            var cmdText = string.Format("select distinct PricePositionID,PriceGroupID, PricePositionCode, PricePositionName, MaterialID from GetPriceListForBranch({0})", (int)branchType);
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var pricePositionCollection = new List<PricePositionDto>();
                    while (reader.Read())
                    {
                        var pricePositionEntity = new PricePositionEntity();

                        pricePositionEntity.PricePositionId = reader[nameof(pricePositionEntity.PricePositionId)].ToInt();
                        pricePositionEntity.PriceGroupId = reader[nameof(pricePositionEntity.PriceGroupId)].ToInt();
                        pricePositionEntity.PricePositionCode = reader[nameof(pricePositionEntity.PricePositionCode)].ToString();
                        pricePositionEntity.PricePositionName = reader[nameof(pricePositionEntity.PricePositionName)].ToString();
                        var materialId = reader[nameof(pricePositionEntity.MaterialId)];
                        if (materialId != DBNull.Value)
                            pricePositionEntity.MaterialId = materialId.ToInt();

                        var pricePositionDto = _converter.ConvertToPricePosition(pricePositionEntity);
                        pricePositionDto.BranchType = branchType;
                        pricePositionCollection.Add(pricePositionDto);
                    }
                    reader.Close();

                    return pricePositionCollection.ToArray();
                }
            }
        }

        public PriceDto[] GetPrices (int branchType)
        {
            var cmdText = string.Format("select PricePositionID, DateBegin, DateEnd, PriceSTL, PriceModel from GetPriceListForBranch({0})", branchType);
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(cmdText, connection))
                {
                    var reader = command.ExecuteReader();
                    var priceCollection = new List<PriceDto>();
                    while (reader.Read())
                    {
                        var priceEntity = new PriceEntity();
                        priceEntity.PricePositionId = reader[nameof(priceEntity.PricePositionId)].ToInt();
                        priceEntity.DateBegin = reader[nameof(priceEntity.DateBegin)].ToDateTime();
                        priceEntity.DateEnd = reader[nameof(priceEntity.DateEnd)].ToDateTime();
                        priceEntity.PriceSTL = reader[nameof(priceEntity.PriceSTL)].ToDecimal();
                        priceEntity.PriceModel = reader[nameof(priceEntity.PriceModel)].ToDecimal();

                        var priceDto = _converter.ConvertToPrice(priceEntity);
                        priceCollection.Add(priceDto);
                    }
                    reader.Close();

                    return priceCollection.ToArray();
                }
            }
        }

        public ProductDto[] GetProducts()
        {
            var cmdText = "SELECT * FROM GetProductCollection()";
            using var connection = new SqlConnection(_configuration.ConnectionString);
            connection.Open();

            using var command = new SqlCommand(cmdText, connection);
            using var reader = command.ExecuteReader();
            var products = new List<ProductDto>();
            while (reader.Read())
            {
                var productEntity = new ProductEntity();

                productEntity.ProductId = reader[nameof(productEntity.ProductId)].ToInt();
                productEntity.PricePositionId = reader[nameof(productEntity.PricePositionId)].ToInt();
                productEntity.ProstheticsName = reader[nameof(productEntity.ProstheticsName)].ToString();

                products.Add(_converter.ConvertToProduct(productEntity));
            }

            return products.ToArray();
        }

        private ProductDto[] GetProductSetForTooth(int branchTypeId, int pricePositionId, bool stlExist)
        {
            return null;
            //var cmdText = string.Format("select * from GetProductSetForTooth({0}, {1}, {2})", branchTypeId, pricePositionId, stlExist);
            //using (var connection = new SqlConnection(_configuration.ConnectionString))
            //{
            //    connection.Open();

            //    using (var command = new SqlCommand(cmdText, connection))
            //    {
            //        var reader = command.ExecuteReader();
            //        var productSetCollection = new List<ProductDto>();
            //        while (reader.Read())
            //        {
            //            var productSetEntity = new ProductSetForToothEntity();

            //            productSetEntity.PricePositionId = reader[nameof(productSetEntity.PricePositionId)].ToInt();
            //            productSetEntity.ProstheticsId = reader[nameof(productSetEntity.ProstheticsId)].ToInt();
            //            productSetEntity.ProstheticsName = reader[nameof(productSetEntity.ProstheticsName)].ToString();
            //            productSetEntity.Price = reader[nameof(productSetEntity.Price)].ToInt();

            //            productSetCollection.Add(_converter.ConvertToProduct(productSetEntity));
            //        }
            //        reader.Close();

            //        return productSetCollection.ToArray();
            //    }
            //}
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

                    var closed = reader[nameof(orderLite.Closed)];
                    if (closed != DBNull.Value)
                        orderLite.Closed = DateTime.Parse(closed.ToString());

                    orderLiteEntities.Add(orderLite);
                }

                var orders = orderLiteEntities.Select(x => _converter.ConvertToOrderLite(x)).ToArray();
                return orders;
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
                        materialEntity.DictionaryType = DictionaryType.Material;

                        materials.Add(materialEntity);
                    }
                    reader.Close();

                    return materials.Select(x => _converter.ConvertToDictionaryItem(x)).ToArray();
                }
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
                        transparenceEntity.DictionaryType = DictionaryType.Transparency;

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
                        equipmentEntity.DictionaryType = DictionaryType.Equipment;

                        equipmentEntities.Add(equipmentEntity);
                    }
                    var equipment = equipmentEntities.Select(x => _converter.ConvertToDictionaryItem(x)).ToArray();
                    return equipment;
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
                        prostheticTypeEntity.DictionaryType = DictionaryType.ProstheticType;

                        prostheticTypeEntities.Add(prostheticTypeEntity);
                    }
                    reader.Close();

                    return prostheticTypeEntities.Select(x => _converter.ConvertToDictionaryItem(x)).ToArray();
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
                        prostheticConditionEntity.DictionaryType = DictionaryType.ProstheticCondition;

                        prostheticConditionEntities.Add(prostheticConditionEntity);
                    }
                    reader.Close();

                    return prostheticConditionEntities.Select(x => _converter.ConvertToDictionaryItem(x)).ToArray();
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
                    CarcassColor = orderDto.CarcassColor,
                    ImplantSystem = orderDto.ImplantSystem,
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

        public ResponsiblePersonDto[] GetResponsiblePersons()
        {
            var cmdText = "select * from GetResponsiblePersons(default, default)";
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

        public ResponsiblePersonDto AddResponsiblePerson(ResponsiblePersonDto responsiblePerson)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("AddNewRespPerson", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@rp_position", SqlDbType.NVarChar)).Value = responsiblePerson.Position;
                    command.Parameters.Add(new SqlParameter("@responsiblePerson", SqlDbType.NVarChar)).Value = responsiblePerson.FullName;
                    command.Parameters.Add(new SqlParameter("@rp_phone", SqlDbType.NVarChar)).Value = responsiblePerson.Phone.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@rp_email", SqlDbType.NVarChar)).Value = responsiblePerson.Email.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@rp_description", SqlDbType.NVarChar)).Value = responsiblePerson.Description.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@responsiblePersonId", SqlDbType.Int) { Direction = ParameterDirection.Output });

                    command.ExecuteNonQuery();

                    responsiblePerson.Id = command.Parameters["@responsiblePersonId"].Value.ToInt();

                    return responsiblePerson;
                }
            }
        }

        public ResponsiblePersonDto UpdateResponsiblePerson(ResponsiblePersonDto responsiblePerson)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("UpdateResponsiblePerson", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@responsiblePersonId", SqlDbType.Int)).Value = responsiblePerson.Id;
                    command.Parameters.Add(new SqlParameter("@rp_Position", SqlDbType.NVarChar)).Value = responsiblePerson.Position;
                    command.Parameters.Add(new SqlParameter("@responsiblePerson", SqlDbType.NVarChar)).Value = responsiblePerson.FullName;
                    command.Parameters.Add(new SqlParameter("@rp_phone", SqlDbType.NVarChar)).Value = responsiblePerson.Phone.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@rp_email", SqlDbType.NVarChar)).Value = responsiblePerson.Email.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@rp_description", SqlDbType.NVarChar)).Value = responsiblePerson.Description.GetValueOrDbNull();

                    command.ExecuteNonQuery();

                    return responsiblePerson;
                }
            }
        }

        public ResponsiblePersonDeleteResult DeleteResponsiblePerson(int responsiblePersonId)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("UnionResponsiblePersons", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@oldResponsiblePersonId", SqlDbType.NVarChar)).Value = responsiblePersonId;
                    command.Parameters.Add(new SqlParameter("@newResponsiblePersonId", SqlDbType.NVarChar)).Value = DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@resultCount", SqlDbType.Int) { Direction = ParameterDirection.Output });

                    command.ExecuteNonQuery();

                    var count = command.Parameters["@resultCount"].Value.ToInt();

                    return new ResponsiblePersonDeleteResult()
                    {
                        ResponsiblePersonId = responsiblePersonId,
                        Count = count
                    };
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

        public CustomerDto UpdateCustomer(CustomerDto customer)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("UpdateCustomer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@customerId", SqlDbType.Int)).Value = customer.Id;
                    command.Parameters.Add(new SqlParameter("@customerName", SqlDbType.NVarChar)).Value = customer.Name;
                    command.Parameters.Add(new SqlParameter("@customerPhone", SqlDbType.NVarChar)).Value = customer.Phone.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@customerEmail", SqlDbType.NVarChar)).Value = customer.Email.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@customerWebsite", SqlDbType.NVarChar)).Value = customer.WebSite.GetValueOrDbNull();
                    command.Parameters.Add(new SqlParameter("@customerDescription", SqlDbType.NVarChar)).Value = customer.Description.GetValueOrDbNull();

                    command.ExecuteNonQuery();

                    return customer;
                }
            }
        }

        public CustomerDeleteResult DeleteCustomer(int customerId)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("UnionCustomers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@oldCustomerId", SqlDbType.NVarChar)).Value = customerId;
                    command.Parameters.Add(new SqlParameter("@newCustomerId", SqlDbType.NVarChar)).Value = DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@resultCount", SqlDbType.Int) { Direction = ParameterDirection.Output });

                    command.ExecuteNonQuery();

                    var count = command.Parameters["@resultCount"].Value.ToInt();

                    return new CustomerDeleteResult
                    {
                        CustomerId = customerId,
                        Count = count
                    };
                }
            }
        }
    }
}