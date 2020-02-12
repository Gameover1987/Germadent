using System;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Printing
{
    public interface IPrintableOrderConverter
    {
        PrintableOrder ConvertFrom(OrderDto order);
    }

    public class PrintableOrderConverter : IPrintableOrderConverter
    {
        public PrintableOrder ConvertFrom(OrderDto order)
        {
            return new PrintableOrder
            {
                AdditionalInfo = order.AdditionalInfo,
                BranchType = GetBranchTypeName(order.BranchType),
                FittingDate = order.FittingDate,
                DocNumber = order.DocNumber,
                CarcassColor = order.CarcassColor,
                Closed = order.Closed,
                ColorAndFeatures = order.ColorAndFeatures,
                ResponsiblePerson = order.ResponsiblePerson,
                IndividualAbutmentProcessing = order.IndividualAbutmentProcessing,
                Created = order.Created,
                CustomerName = order.Customer,
                Understaff = order.Understaff,
                ImplantSystem = order.ImplantSystem,
                WorkDescription = order.WorkDescription,
                FlagWorkAccept = order.WorkAccepted,
                OfficeAdmin = order.OfficeAdminName,
                PatientAge = order.Age,
                PatientFullName = order.Patient,
                PatientGender = GetGenderName(order.Gender),
                TechnicPhone = order.ResponsiblePersonPhone,
                TransparenceName = GetTransparenceName(order.Transparency),
                WorkOrderID = order.WorkOrderId,
                ProstheticArticul = order.ProstheticArticul,
                MaterialsStr = order.MaterialsStr
            };
        }

        private string GetBranchTypeName(BranchType branchType)
        {
            switch (branchType)
            {
                case BranchType.Laboratory:
                    return "Лаборатория";

                case BranchType.MillingCenter:
                    return "Фрезерный центр";

                default:
                    throw new NotImplementedException("Неизвестный тип подразделения");
            }
        }

        private string GetGenderName(Gender gender)
        {
            switch (gender)
            {
                case Gender.Female:
                    return "жен.";

                case Gender.Male:
                    return "муж.";

                default:
                    throw new ArgumentException(nameof(gender));
            }
        }

        private string GetTransparenceName(int transparenceId)
        {
            return "";
        }
    }
}