﻿using System;
using System.Linq;
using System.Text;
using Germadent.Common.Extensions;
using Germadent.Rma.App.ServiceClient;
using Germadent.Rma.App.ServiceClient.Repository;
using Germadent.Rma.Model;

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
                AdditionalInfo = order.AdditionalInfo,
                BranchType = GetBranchTypeName(order.BranchType),
                FittingDate = order.FittingDate,
                DateOfCompletion = order.DateOfCompletion,
                DateComment = order.DateComment,
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
                FlagWorkAccept = order.WorkAccepted.ToYesNo(),
                OfficeAdmin = order.OfficeAdminName,
                PatientFullName = order.Patient,
                PatientGender = GetGenderName(order.Gender),
                TechnicPhone = order.ResponsiblePersonPhone,
                TransparenceName = order.BranchType == BranchType.Laboratory ? GetTransparenceName(order.Transparency) : null,
                WorkOrderID = order.WorkOrderId,
                ProstheticArticul = order.ProstheticArticul,
                WorkDescription = order.WorkDescription,
                MaterialsStr = order.MaterialsStr,
                ToothCardDescription = GetToothCardDescription(order),
                AdditionalEquipment = OrderDescriptionBuilder.GetAdditionalEquipmentDescription(order)
            };

            if (order.Patient != null)
            {
                printableOrder.PatientAge = order.Age;
                printableOrder.PatientGender = GetGenderName(order.Gender);
            }

            return printableOrder;
        }

        private string GetToothCardDescription(OrderDto order)
        {
            var descriptions = order.ToothCard.Select(OrderDescriptionBuilder.GetToothCardDescription).ToArray();
            
            var builder = new StringBuilder();
            foreach (var description in descriptions)
            {
                builder.AppendLine(description + ";    ");
            }

            var result = builder.ToString();
            return result;
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

        private string GetTransparenceName(int transparenceId)
        {
            var transparences = _dictionaryRepository.GetItems(DictionaryType.Transparency);
            return transparences.First(x => x.Id == transparenceId).Name;
        }
    }
}