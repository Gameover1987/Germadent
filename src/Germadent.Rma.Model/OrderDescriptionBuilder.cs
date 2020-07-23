﻿using System.Collections.Generic;
using System.Text;

namespace Germadent.Rma.Model
{
    public static class OrderDescriptionBuilder
    {
        public static string[] GetToothCardDescription(ToothDto tooth)
        {
            var descriptionBuilder = new StringBuilder();

            if (tooth.ConditionName != null)
                descriptionBuilder.Append(string.Format("{0}/", tooth.ConditionName));

            if (tooth.ProstheticsName != null)
                descriptionBuilder.Append(string.Format("{0}/", tooth.ProstheticsName));

            if (tooth.MaterialName != null)
                descriptionBuilder.Append(string.Format("{0}/", tooth.MaterialName));

            if (tooth.HasBridge)
                descriptionBuilder.Append("Мост");

            var toothDescription = descriptionBuilder.ToString().Trim(' ', '/');

            string[] toothArray = new string[] { tooth.ToothNumber.ToString(), toothDescription };
 
            return toothArray;

            //return string.Format("{0} - {1}", tooth.ToothNumber, toothDescription);
        }

        public static string GetAdditionalEquipmentDescription(OrderDto order)
        {
            if (order.AdditionalEquipment == null)
                return null;

            var builder = new StringBuilder();
            foreach (var dto in order.AdditionalEquipment)
            {
                builder.AppendLine(string.Format("{0} - {1}, ", dto.EquipmentName, dto.Quantity));
            }

            return builder.ToString().Trim(' ', ',');
        }
    }
}
