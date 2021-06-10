using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Model;
using Germadent.WebApi.Configuration;
using Germadent.WebApi.DataAccess.UserManagement;
using Germadent.WebApi.Entities;
using Germadent.WebApi.Entities.Conversion;
using Newtonsoft.Json;

namespace Germadent.WebApi.DataAccess.Rma
{
    public partial class RmaDbOperations : IRmaDbOperations
    {
        private readonly IRmaEntityConverter _converter;
        private readonly IServiceConfiguration _configuration;
        private readonly IUmcDbOperations _umcDbOperations;

        public RmaDbOperations(IRmaEntityConverter converter, IServiceConfiguration configuration, IUmcDbOperations umcDbOperations)
        {
            _converter = converter;
            _configuration = configuration;
            _umcDbOperations = umcDbOperations;
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

                default:
                    throw new NotImplementedException("Неизвестный тип словаря");
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

                    return attributes.OrderBy(x => x.AttributeValue).ToArray();
                }
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

                    return materials.Select(x => _converter.ConvertToDictionaryItem(x)).OrderBy(x => x.Name).ToArray();
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

                    return prostheticTypeEntities.Select(x => _converter.ConvertToDictionaryItem(x)).OrderBy(x => x.Name).ToArray();
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

        public ReportListDto[] GetWorkReport(int workOrderId)
        {
            var orderDto = GetWorkOrderById(workOrderId);

            var toothCard = GetToothCard(workOrderId, orderDto.Stl);

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
                    ConstructionColor = OrderDescriptionBuilder.GetAttributesValuesToReport(orderDto, "ConstructionColor"),
                    ImplantSystem = OrderDescriptionBuilder.GetAttributesValuesToReport(orderDto, "ImplantSystem"),
                    Quantity = product.Quantity,
                    ProstheticArticul = orderDto.ProstheticArticul,
                    TotalPrice = orderDto.Cashless ? 0: product.TotalPrice,
                    TotalPriceCashless = orderDto.Cashless ? product.TotalPrice : 0,
                    ResponsiblePerson = orderDto.ResponsiblePerson
                };
                reports.Add(reportListDto);
            }

            return reports.ToArray();
        }
    }
}