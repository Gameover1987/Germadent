using Germadent.Rma.Model;

namespace Germadent.DataAccessService.Entities.Conversion
{
    public class EntityToDtoConverter : IEntityToDtoConverter
    {
        public OrderDto ConvertToOrder(OrderEntity entity)
        {
            var orderDto = new OrderDto
            {
                WorkOrderId = entity.WorkOrderId,
                Status = entity.Status,
                AdditionalInfo = entity.AdditionalInfo,
                BranchType = (BranchType) entity.BranchTypeId,
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

        public ToothDto ConvertToTooth(ToothEntity entity)
        {
            return new ToothDto
            {
                ToothNumber = entity.ToothNumber,
                ConditionName = entity.ConditionName.Trim(),
                MaterialName = entity.MaterialName.Trim(),
                ProstheticsName = entity.ProstheticsName.Trim(),
                HasBridge = entity.FlagBridge
            };
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

        public ProstheticConditionDto ConvertToProstheticCondition(ProstheticConditionEntity entity)
        {
            return new ProstheticConditionDto
            {
                Id = entity.ConditionId,
                Name = entity.ConditionName
            };
        }

        public MaterialDto ConvertToMaterial(MaterialEntity entity)
        {
            return new MaterialDto
            {
                Id = entity.MaterialId,
                MaterialName = entity.MaterialName,
                IsObsolete = entity.FlagUnused
            };
        }

        public ProstheticsTypeDto ConvertToProstheticType(ProstheticTypeEntity entity)
        {
            return new ProstheticsTypeDto
            {
                Id = entity.ProstheticsId,
                Name = entity.ProstheticsName
            };
        }
        public TransparencesDto ConvertToTransparences(TransparencesEntity entity)
        {
            return new TransparencesDto
            {
                Id = entity.TransparenceId,
                Name = entity.TransparenceName
            };
        }

        public EquipmentDto ConvertToEquipment(EquipmentEntity entity)
        {
            return new EquipmentDto
            {
                Id = entity.EquipmentId,
                Name = entity.EquipmentName
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
    }
}