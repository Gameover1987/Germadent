using Germadent.Rma.Model;

namespace Germadent.DataAccessService.Entities.Conversion
{
    public class EntityToDtoConverter : IEntityToDtoConverter
    {
        public OrderDto ConvertToOrder(OrderEntity entity)
        {
            return new OrderDto
            {
                Id = entity.WorkOrderId,
                Status =  entity.Status,
                AdditionalInfo = entity.AdditionalInfo,
                BranchType = (BranchType)entity.BranchTypeId,
                CarcassColor = entity.CarcassColor,
                Customer = entity.CustomerName,
                ColorAndFeatures = entity.ColorAndFeatures,
                Closed = entity.Closed,
                Created = entity.Created,
                FittingDate = entity.FittingDate,
                ImplantSystem = entity.ImplantSystem,
                IndividualAbutmentProcessing = entity.IndividualAbutmentProcessing,
                Number = entity.DocNumber,
                Patient = entity.Patient,
                Age = entity.Age,
                ResponsiblePerson = entity.ResponsiblePerson,
                ResponsiblePersonPhone = entity.ResponsiblePersonPhone,
                Gender = entity.PatientGender == true ? Gender.Male : Gender.Female,
                WorkDescription = entity.WorkDescription,
                WorkAccepted = entity.FlagWorkAccept
            };
        }

        public OrderLiteDto ConvertToOrderLite(OrderLiteEntity entity)
        {
            return new OrderLiteDto
            {
                WorkOrderId = entity.WorkOrderId,
                BranchType = (BranchType)entity.BranchTypeId,
                BranchTypeId = entity.BranchTypeId,
                CustomerName = entity.CustomerName,
                PatientFnp = entity.PatientFnp,
                DocNumber = entity.DocNumber,
                ResponsiblePerson = entity.ResponsiblePerson,
                Status = entity.Status,
                Created = entity.Created,
                Closed = entity.Closed
            };
        }

        public MaterialDto ConvertToMaterial(MaterialEntity entity)
        {
            return new MaterialDto
            {
                Name = entity.MaterialName,
                IsObsolete = entity.FlagUnused
            };
        }
    }
}