using System;
using System.Linq;
using Germadent.Client.Common.ServiceClient;
using Germadent.Common.Extensions;
using Germadent.Model;
using Germadent.Rma.App.ServiceClient.Repository;

namespace Germadent.Rma.App.Reporting
{
    public interface IPrintableOrderConverter
    {
        PrintableOrder ConvertFrom(OrderDto order);
    }

    public class PrintableOrderConverter : IPrintableOrderConverter
    {
        private readonly IDictionaryRepository _dictionaryRepository;

        public PrintableOrderConverter(IDictionaryRepository dictionaryRepository)
        {
            _dictionaryRepository = dictionaryRepository;
        }

        public PrintableOrder ConvertFrom(OrderDto order)
        {
            var printableOrder = new PrintableOrder
            {
                AdditionalInfo = OrderDescriptionBuilder.GetAttributesValuesToReport(order, "AdditionalInfo"),
                BranchType = GetBranchTypeName(order.BranchType),
                FittingDate = order.FittingDate,
                DateOfCompletion = order.DateOfCompletion,
                DateComment = order.DateComment,
                DocNumber = order.DocNumber,
                CarcassColor = OrderDescriptionBuilder.GetAttributesValuesToReport(order, "ConstructionColor"),
                Closed = order.Closed,
                ColorAndFeatures = OrderDescriptionBuilder.GetAttributesValuesToReport(order, "ConstructionColor"),
                ResponsiblePerson = order.ResponsiblePerson,
                Created = order.Created,
                CustomerName = order.Customer,
                Understaff = OrderDescriptionBuilder.GetUnderstaff(order),
                ImplantSystem = OrderDescriptionBuilder.GetAttributesValuesToReport(order, "ImplantSystem"),
                FlagWorkAccept = order.WorkAccepted.ToYesNo(),
                OfficeAdminName = order.CreatorFullName,
                PatientFullName = order.Patient,
                PatientGender = GetGenderName(order.Gender),
                TechnicPhone = order.ResponsiblePersonPhone,
                TransparenceName = order.BranchType == BranchType.Laboratory ? OrderDescriptionBuilder.GetAttributesValuesToReport(order, "Trasparency") : null,
                WorkOrderID = order.WorkOrderId,
                ProstheticArticul = order.ProstheticArticul,
                WorkDescription = order.WorkDescription,
                MaterialsStr = order.MaterialsStr,
                ToothCardDescription = OrderDescriptionBuilder.GetToothCardDescription(order.ToothCard),
                AdditionalEquipment = OrderDescriptionBuilder.GetAdditionalEquipmentDescription(order)
            };

            if (order.Patient != null)
            {
                printableOrder.PatientAge = order.Age;
                printableOrder.PatientGender = GetGenderName(order.Gender);
            }

            return printableOrder;
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
                    //TODO Nekrasov: NotSupportedException
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
                    //TODO Nekrasov:same shit
                    throw new ArgumentException(nameof(gender));
            }
        }
    }
}