using System.Collections.Generic;
using System.Linq;
using Germadent.Rma.Model;
using Germadent.Rma.Model.Pricing;

namespace Germadent.WebApi.Entities.Conversion
{
    public class RmaEntityConverter : IRmaEntityConverter
    {
        public OrderDto ConvertToOrder(OrderEntity entity)
        {
            var orderDto = new OrderDto
            {
                WorkOrderId = entity.WorkOrderId,
                Status = entity.Status,
                BranchType = (BranchType)entity.BranchTypeId,
                CustomerId = entity.CustomerId,
                Customer = entity.CustomerName,
                Closed = entity.Closed,
                Created = entity.Created,
                FittingDate = entity.FittingDate,
                DocNumber = entity.DocNumber,
                Patient = entity.Patient,
                Age = entity.Age,
                ResponsiblePersonId = entity.ResponsiblePersonId,
                ResponsiblePerson = entity.DoctorFullName,
                ResponsiblePersonPhone = entity.TechnicPhone,
                Gender = entity.PatientGender ? Gender.Male : Gender.Female,
                WorkDescription = entity.WorkDescription,
                OfficeAdminName = entity.OfficeAdminName,
                WorkAccepted = entity.FlagWorkAccept,
                Stl = entity.FlagStl,
                Cashless = entity.FlagCashless,
                ProstheticArticul = entity.ProstheticArticul,
                MaterialsStr = entity.MaterialsEnum,
                DateComment = entity.DateComment,
                DateOfCompletion = entity.DateOfCompletion
            };

            if (orderDto.BranchType == BranchType.Laboratory)
            {
                orderDto.ResponsiblePerson = entity.DoctorFullName;
            }
            else if (orderDto.BranchType == BranchType.MillingCenter)
            {
                orderDto.ResponsiblePerson = entity.TechnicFullName;
                orderDto.ResponsiblePersonPhone = entity.TechnicPhone;
            }

            return orderDto;
        }

        public ToothDto[] ConvertToToothCard(ToothEntity[] entities, bool getStlPrice)
        {
            var groupsByToothNumber = entities.GroupBy(x => x.ToothNumber).ToArray();

            var toothCollection = new List<ToothDto>();

            foreach (var grouping in groupsByToothNumber)
            {
                var toothDto = new ToothDto();
                var prototype = grouping.First();
                toothDto.WorkOrderId = prototype.WorkOrderId;
                toothDto.ToothNumber = prototype.ToothNumber;
                toothDto.HasBridge = prototype.HasBridge;
                toothDto.ConditionId = prototype.ConditionId;
                toothDto.ConditionName = prototype.ConditionName;

                //var groupsByProduct = grouping.GroupBy(x => x.PricePositionId).ToArray();
                var products = new List<ProductDto>();
                foreach (var toothEntity in grouping)
                {
                    //var prototypeByProduct = groupByProduct.First();
                    var productDto = new ProductDto
                    {
                        MaterialId = toothEntity.MaterialId,
                        MaterialName = toothEntity.MaterialName,
                        PriceGroupId = toothEntity.PriceGroupId,
                        PricePositionCode = toothEntity.PricePositionCode,
                        PricePositionId = toothEntity.PricePositionId,
                        ProductId = toothEntity.ProductId,
                        ProductName = toothEntity.ProductName
                    };
                    if (getStlPrice)
                    {
                        productDto.PriceStl = toothEntity.Price;
                    }
                    else
                    {
                        productDto.PriceModel = toothEntity.Price;
                    }

                    products.Add(productDto);
                }

                toothDto.Products = products.ToArray();

                toothCollection.Add(toothDto);
            }

            return toothCollection.ToArray();
        }

        public OrderLiteDto ConvertToOrderLite(OrderLiteEntity entity)
        {
            return new OrderLiteDto
            {
                WorkOrderId = entity.WorkOrderId,
                BranchType = (BranchType)entity.BranchTypeId,
                CustomerName = entity.CustomerName,
                PatientFnp = entity.PatientFullName,
                DocNumber = entity.DocNumber,
                DoctorFullName = entity.DoctorFullName,
                TechnicFullName = entity.TechnicFullName,
                Status = entity.Status,
                Created = entity.Created,
                CreatorFullName = entity.CreatorFullName,
                Closed = entity.Closed,
            };
        }

        public DictionaryItemDto ConvertToDictionaryItem(DictionaryItemEntity entity)
        {
            return new DictionaryItemDto
            {
                Id = entity.Id,
                Name = entity.Name,
                DictionaryName = entity.DictionaryName,
                Dictionary = entity.DictionaryType
            };
        }

        public AdditionalEquipmentDto ConvertToAdditionalEquipment(AdditionalEquipmentEntity entity)
        {
            return new AdditionalEquipmentDto
            {
                WorkOrderId = entity.WorkOrderId,
                EquipmentId = entity.EquipmentId,
                EquipmentName = entity.EquipmentName,
                QuantityIn = entity.QuantityIn,
                QuantityOut = entity.QuantityOut
            };
        }

        public CustomerDto ConvertToCustomer(CustomerEntity entity)
        {
            return new CustomerDto
            {
                Id = entity.CustomerId,
                Name = entity.CustomerName,
                Phone = entity.CustomerPhone,
                Email = entity.CustomerEmail,
                WebSite = entity.CustomerWebSite,
                Description = entity.CustomerDescription
            };
        }

        public ResponsiblePersonDto ConvertToResponsiblePerson(ResponsiblePersonEntity entity)
        {
            return new ResponsiblePersonDto
            {
                Id = entity.ResponsiblePersonId,
                FullName = entity.ResponsiblePerson,
                Position = entity.RP_Position,
                Phone = entity.RP_Phone,
                Email = entity.RP_Email,
                Description = entity.RP_Description
            };
        }

        public AttributeDto ConvertToAttribute(AttributesEntity entity)
        {
            return new AttributeDto
            {
                AttributeId = entity.AttributeId,
                AttributeKeyName = entity.AttributeKeyName,
                AttributeName = entity.AttributeName,
                AttributeValue = entity.AttributeValue,
                AttributeValueId = entity.AttributeValueId,
                IsObsolete = entity.IsObsolete
            };
        }

        public PriceGroupDto ConvertToPriceGroup(PriceGroupEntity entity)
        {
            return new PriceGroupDto
            {
                PriceGroupId = entity.PriceGroupId,
                Name = entity.PriceGroupName
            };
        }


        public PricePositionDto ConvertToPricePosition(PricePositionEntity entity)
        {
            return new PricePositionDto
            {
                PricePositionId = entity.PricePositionId,
                PriceGroupId = entity.PriceGroupId,
                UserCode = entity.PricePositionCode,
                Name = entity.PricePositionName,
                MaterialId = entity.MaterialId,
            };
        }

        public PriceDto ConvertToPrice(PriceEntity entity)
        {
            return new PriceDto
            {
                PricePositionId = entity.PricePositionId,
                DateBeginning = entity.DateBeginning,
                PriceStl = entity.PriceSTL,
                PriceModel = entity.PriceModel
            };
        }

        public ProductDto ConvertToProduct(ProductEntity entity)
        {
            return new ProductDto
            {
                ProductId = entity.ProductId,
                PricePositionId = entity.PricePositionId,
                MaterialId = entity.MaterialId,
                PriceModel = entity.PriceModel,
                PriceStl = entity.PriceStl ?? 0,
                MaterialName = entity.MaterialName,
                PriceGroupId = entity.PriceGroupId,
                ProductName = entity.ProductName,
                PricePositionCode = entity.PricePositionCode
            };
        }

        public ToothEntity[] ConvertFromToothDto(ToothDto toothDto, bool setPriceStl)
        {
            var entities = new List<ToothEntity>();
            foreach (var product in toothDto.Products)
            {
                var entity = new ToothEntity
                {
                    WorkOrderId = toothDto.WorkOrderId,
                    PriceGroupId = product.PriceGroupId,
                    PricePositionCode = product.PricePositionCode,
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ConditionName = toothDto.ConditionName,
                    HasBridge = toothDto.HasBridge,
                    PricePositionId = product.PricePositionId,
                    ConditionId = toothDto.ConditionId,
                    MaterialId = product.MaterialId,
                    ToothNumber = toothDto.ToothNumber,
                    Price = setPriceStl ? product.PriceStl : product.PriceModel
                };
                entities.Add(entity);
            }

            return entities.ToArray();
        }
    }
}