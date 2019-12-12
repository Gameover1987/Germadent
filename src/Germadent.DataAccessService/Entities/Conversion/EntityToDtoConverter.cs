using Germadent.Rma.Model;

namespace Germadent.DataAccessService.Entities.Conversion
{
    public class EntityToDtoConverter : IEntityToDtoConverter
    {
        public Order ConvertToOrder(OrderEntity entity)
        {
            return new Order
            {
                Id = entity.WorkOrderId,
                Status =  entity.Status,
                AdditionalInfo = entity.AdditionalInfo,
                BranchType = (BranchType)entity.BranchTypeId,
                CarcassColor = entity.CarcassColor,
                Customer = entity.Customer,
                ColorAndFeatures = entity.ColorAnFeatures,
                Closed = entity.Closed,
                Created = entity.Created,
                FittingDate = entity.FittingDate,
                ImplantSystem = entity.ImplantSystem,
                IndividualAbutmentProcessing = entity.IndividualAbutmentProcessing,
                Number = entity.DocNumber,
                Patient = entity.Patient,
                //Age = 
                //Doctor = entity.
                //Sex = 
                //TechnikPhone = 
                //Mouth = 
                WorkDescription = entity.WorkDescription,
                WorkAccepted = entity.FlagWorkAccepted,
            };
        }

        public OrderLite ConvertToOrderLite(OrderLiteEntity entity)
        {
            return new OrderLite
            {
                BranchType = (BranchType)entity.BranchTypeId,
                BranchTypeId = entity.BranchTypeId,
                CustomerName = entity.CustomerName,
                PatientFnp = entity.PatientFnp,
                DocNumber = entity.DocNumber,
                ResponsiblePerson = entity.ResponsiblePersonName,
                Status = entity.Status,
                Created = entity.Created,
                Closed = entity.Closed
            };
        }

        public Material ConvertToMaterial(MaterialEntity entity)
        {
            return new Material
            {
                Name = entity.MaterialName,
                IsObsolete = entity.FlagUnused
            };
        }
    }
}