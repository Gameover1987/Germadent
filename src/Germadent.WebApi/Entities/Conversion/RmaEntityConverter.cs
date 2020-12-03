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
                AdditionalInfo = entity.AdditionalInfo,
                BranchType = (BranchType)entity.BranchTypeId,
                CarcassColor = entity.CarcassColor,
                CustomerId = entity.CustomerId,
                Customer = entity.CustomerName,
                ColorAndFeatures = entity.ColorAndFeatures,
                Closed = entity.Closed,
                Created = entity.Created,
                FittingDate = entity.FittingDate,
                ImplantSystem = entity.ImplantSystem,
                IndividualAbutmentProcessing = entity.IndividualAbutmentProcessing,
                DocNumber = entity.DocNumber,
                Patient = entity.Patient,
                Age = entity.Age,
                ResponsiblePersonId = entity.ResponsiblePersonId,
                ResponsiblePerson = entity.DoctorFullName,
                ResponsiblePersonPhone = entity.TechnicPhone,
                Gender = entity.PatientGender ? Gender.Male : Gender.Female,
                Transparency = entity.Transparency,
                WorkDescription = entity.WorkDescription,
                WorkAccepted = entity.FlagWorkAccept,
                Understaff = entity.Understaff,
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

        public ToothDto[] ConvertToToothCard(ToothEntity[] entities)
        {
            var groupings = entities.GroupBy(x => x.ToothNumber).ToArray();

            var toothCollection = new List<ToothDto>();

            foreach (var grouping in groupings)
            {
                var toothDto = new ToothDto();
                var prototype = grouping.First();
                toothDto.WorkOrderId = prototype.WorkOrderId;
                toothDto.ToothNumber = prototype.ToothNumber;
                toothDto.HasBridge = prototype.HasBridge;
                toothDto.MaterialId = prototype.MaterialId;
                toothDto.MaterialName = prototype.MaterialName;
                toothDto.ProstheticsId = prototype.ProstheticsId;
                toothDto.ProstheticsName = prototype.ProstheticsName;
                toothDto.ConditionId = prototype.ConditionId;
                toothDto.ConditionName = prototype.ConditionName;

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
                Quantity = entity.Quantity
            };
        }

        public ReportListDto ConvertToExcel(ExcelEntity entity)
        {
            return new ReportListDto
            {
                Created = entity.Created,
                DocNumber = entity.DocNumber,
                Customer = entity.Customer,
                EquipmentSubstring = entity.EquipmentSubstring,
                Patient = entity.Patient,
                ProstheticSubstring = entity.ProstheticSubstring,
                MaterialsStr = entity.MaterialsStr,
                ColorAndFeatures = entity.ColorAndFeatures,
                CarcassColor = entity.CarcassColor,
                ImplantSystem = entity.ImplantSystem,
                Quantity = entity.Quantity,
                ProstheticArticul = entity.ProstheticArticul
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
                BranchType = entity.BranchTypeId,
                MaterialId = entity.MaterialId,
                PriceModel = entity.PriceModel,
                PriceStl = entity.PriceStl ?? 0,
                MaterialName = entity.MaterialName,
                PriceGroupId = entity.PriceGroupId,
                ProductName = entity.ProductName,
                PricePositionCode = entity.PricePositionCode
            };
        }

        public ToothEntity[] ConvertFromToothDto(ToothDto toothDto)
        {
            var entities = new List<ToothEntity>();
            foreach (var pricePosition in toothDto.Products)
            {
                var entity = new ToothEntity
                {
                    WorkOrderId = toothDto.WorkOrderId,
                    ConditionName = toothDto.ConditionName,
                    HasBridge = toothDto.HasBridge,
                    MaterialName = toothDto.MaterialName,
                    Price = toothDto.Price,
                    PricePositionId = pricePosition.PricePositionId,
                    ConditionId = toothDto.ConditionId,
                    MaterialId = pricePosition.MaterialId,
                    ProstheticsId = toothDto.ProstheticsId,
                    ProstheticsName = toothDto.ProstheticsName,
                    ToothNumber = toothDto.ToothNumber
                };
                entities.Add(entity);
            }

            return entities.ToArray();
        }
    }
}