using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;
using Germadent.WebApi.Configuration;
using Germadent.WebApi.Entities;
using Germadent.WebApi.Entities.Conversion;
using Newtonsoft.Json;

namespace Germadent.WebApi.DataAccess.Rma
{
    public partial class RmaDbOperations : IRmaDbOperations
    {
        private readonly IRmaEntityConverter _converter;
        private readonly IServiceConfiguration _configuration;

        public RmaDbOperations(IRmaEntityConverter converter, IServiceConfiguration configuration)
        {
            _converter = converter;
            _configuration = configuration;
        }

        public OrderDto AddOrder(OrderDto order)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                OrderDto outputOrder;
                connection.Open();
                outputOrder = AddWorkOrder(order, connection);

                order.ToothCard.ForEach(x => x.WorkOrderId = order.WorkOrderId);
                AddOrUpdateToothCard(order, connection);

                return outputOrder;
            }
        }

        private static OrderDto AddWorkOrder(OrderDto order, SqlConnection connection)
        {
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
                command.Parameters.Add(new SqlParameter("@officeAdminId", SqlDbType.Int)).Value = DBNull.Value;
                command.Parameters.Add(new SqlParameter("@fittingDate", SqlDbType.DateTime)).Value = order.FittingDate;
                command.Parameters.Add(new SqlParameter("@dateOfCompletion", SqlDbType.DateTime)).Value = order.DateOfCompletion;
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
                

        private void AddOrUpdateToothCard(OrderDto orderDto, SqlConnection connection)
        {
            var toothEntities = orderDto.ToothCard.SelectMany(x => _converter.ConvertFromToothDto(x, orderDto.Stl)).ToArray();
            var toothCardJson = toothEntities.SerializeToJson(Formatting.Indented);

            var cmdText = "AddOrUpdateToothCardInWO";

            using (var command = new SqlCommand(cmdText, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@workOrderId", SqlDbType.Int)).Value = orderDto.WorkOrderId;
                command.Parameters.Add(new SqlParameter("@jsonToothCardString", SqlDbType.NVarChar)).Value = toothCardJson;

                command.ExecuteNonQuery();
            }
        }

        public void UpdateOrder(OrderDto order)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                UpdateWorkOrder(order, connection);

                AddOrUpdateToothCard(order, connection);
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
             

        private static void UpdateWorkOrder(OrderDto order, SqlConnection connection)
        {
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
                command.Parameters.Add(new SqlParameter("@jsonEquipmentsString", SqlDbType.NVarChar)).Value = jsonEquipmentsString;
                command.Parameters.Add(new SqlParameter("@jsonAttributesString", SqlDbType.NVarChar)).Value = jsonAttributesString;
                command.ExecuteNonQuery();

                order.Created = command.Parameters["@created"].Value.ToDateTime();
            }

        }

        public OrderDto GetOrderDetails(int id)
        {
            var orderDto = GetWorkOrderById(id);
            orderDto.ToothCard = GetToothCard(id, orderDto.Stl);
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
                            AdditionalInfo = reader["AdditionalInfo"].ToString(),
                            BranchTypeId = reader["BranchTypeId"].ToInt(),
                            DocNumber = reader["DocNumber"].ToString(),
                            FlagWorkAccept = reader["FlagWorkAccept"].ToBool(),
                            FlagStl = reader["FlagStl"].ToBool(),
                            FlagCashless = reader["FlagCashless"].ToBool(),
                            Created = reader["Created"].ToDateTime(),
                            ImplantSystem = reader["ImplantSystem"].ToString(),
                            IndividualAbutmentProcessing = reader["IndividualAbutmentProcessing"].ToString(),
                            Patient = reader["PatientFullName"].ToString(),
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

        private AdditionalEquipmentDto[] GetAdditionalEquipmentByWorkOrder(int workOrderId, SqlConnection connection)
        {
            var cmdText = "select * from GetAdditionalEquipment(@workOrderId)";

            using (var command = new SqlCommand(cmdText, connection))
            {
                command.Parameters.Add(new SqlParameter("@workOrderId", SqlDbType.Int)).Value = workOrderId;
                using (var reader = command.ExecuteReader())
                {
                    var additionalEquipmentEntities = new List<AdditionalEquipmentEntity>();
                    while (reader.Read())
                    {
                        var entity = new AdditionalEquipmentEntity();
                        entity.EquipmentId = reader[nameof(entity.EquipmentId)].ToInt();
                        entity.WorkOrderId = reader[nameof(entity.WorkOrderId)].ToInt();
                        entity.EquipmentName = reader[nameof(entity.EquipmentName)].ToString();
                        entity.QuantityIn = reader[nameof(entity.QuantityIn)].ToInt();
                        entity.QuantityOut = reader[nameof(entity.QuantityOut)].ToInt();
                        additionalEquipmentEntities.Add(entity);
                    }

                    return additionalEquipmentEntities.Select(x => _converter.ConvertToAdditionalEquipment(x)).ToArray();
                }
            }
        }

        private AttributeDto[] GetAttributesByWoId(int workOrderId, SqlConnection connection)
        {
            var cmdText = "select * from GetAttributesValuesByWOId(@workOrderId)";

            using (var command = new SqlCommand(cmdText, connection))
            {
                command.Parameters.Add(new SqlParameter("@workOrderId", SqlDbType.Int)).Value = workOrderId;
                using (var reader = command.ExecuteReader())
                {
                    var attributes = new List<AttributesEntity>();
                    while (reader.Read())
                    {
                        var entity = new AttributesEntity();
                        entity.AttributeId = reader[nameof(AttributesEntity.AttributeId)].ToInt();
                        entity.AttributeKeyName = reader[nameof(AttributesEntity.AttributeKeyName)].ToString();
                        entity.AttributeName = reader[nameof(AttributesEntity.AttributeName)].ToString();
                        entity.AttributeValue = reader[nameof(AttributesEntity.AttributeValue)].ToString();
                        entity.AttributeValueId = reader[nameof(AttributesEntity.AttributeValueId)].ToInt();

                        attributes.Add(entity);
                    }

                    return attributes.Select(x => _converter.ConvertToAttribute(x)).ToArray();
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
            var cmdText = "select * from GetProducts()";

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
                        prostheticTypeEntity.Id = int.Parse(reader["ProductId"].ToString());
                        prostheticTypeEntity.Name = reader["ProductName"].ToString().Trim();
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

            var toothCard = GetToothCard(id, orderDto.Stl);

            var products = toothCard
                .SelectMany(x => x.Products)
                .GroupBy(x => x.ProductName)
                .Select(x => new
                {
                    ProductName = x.Key,
                    Quantity = x.Count(),
                    TotalPrice = x.Sum(y => y.PriceModel == 0 ? y.PriceStl : y.PriceModel) 
                })
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
            foreach (var product in products)
            {
                var reportListDto = new ReportListDto
                {
                    Created = orderDto.Created,
                    DocNumber = orderDto.DocNumber,
                    Customer = orderDto.Customer,
                    EquipmentSubstring = string.Join("; ", equipments),
                    Patient = orderDto.Patient,
                    ProstheticSubstring = product.ProductName,
                    MaterialsStr = orderDto.MaterialsStr,
     //               ColorAndFeatures = OrderDescriptionBuilder.GetAttributesValuesToReport(orderDto, "ConstructionColor"),
                    CarcassColor = OrderDescriptionBuilder.GetAttributesValuesToReport(orderDto, "ConstructionColor"),
                    ImplantSystem = OrderDescriptionBuilder.GetAttributesValuesToReport(orderDto, "ImplantSystem"),
                    Quantity = product.Quantity,
                    ProstheticArticul = orderDto.ProstheticArticul,
                    TotalPrice = orderDto.Cashless ? 0: product.TotalPrice,
                    TotalPriceCashless = orderDto.Cashless ? product.TotalPrice : 0
                };
                reports.Add(reportListDto);
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
                using (var command = new SqlCommand("AddRespPerson", connection))
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

        public DeleteResult DeleteResponsiblePerson(int responsiblePersonId)
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

                    return new DeleteResult()
                    {
                        Id = responsiblePersonId,
                        Count = count
                    };
                }
            }
        }

        public AttributeDto[] GetAllAttributesAndValues()
        {
            var cmdText = "select * from GetAttributesAndValues()";
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var commamd = new SqlCommand(cmdText, connection))
                {
                    var reader = commamd.ExecuteReader();
                    var attributes = new List<AttributeDto>();
                    while (reader.Read())
                    {
                        var entity = new AttributesEntity();
                        entity.AttributeId = reader[nameof(AttributesEntity.AttributeId)].ToInt();
                        entity.AttributeKeyName = reader[nameof(AttributesEntity.AttributeKeyName)].ToString();
                        entity.AttributeName = reader[nameof(AttributesEntity.AttributeName)].ToString();
                        entity.AttributeValue = reader[nameof(AttributesEntity.AttributeValue)].ToString();
                        entity.AttributeValueId = reader[nameof(AttributesEntity.AttributeValueId)].ToInt();
                        entity.IsObsolete = reader[nameof(AttributesEntity.IsObsolete)].ToBool();

                        attributes.Add(_converter.ConvertToAttribute(entity));
                    }

                    return attributes.ToArray();
                }
            }
        }

        public CustomerDto AddCustomer(CustomerDto customer)
        {
            using (var connection = new SqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("AddCustomer", connection))
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

        public DeleteResult DeleteCustomer(int customerId)
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

                    return new DeleteResult
                    {
                        Id = customerId,
                        Count = count
                    };
                }
            }
        }
    }
}