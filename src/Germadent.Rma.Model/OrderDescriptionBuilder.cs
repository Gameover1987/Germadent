using System.Linq;
using System.Text;
using Germadent.Common.Extensions;

namespace Germadent.Rma.Model
{
    public static class OrderDescriptionBuilder
    {
        public static string GetToothDescription(ToothDto tooth)
        {
            return string.Format("{0} - {1}", tooth.ToothNumber, GetToothDescriptionWithoutNumber(tooth));
        }

        private static string GetToothDescriptionWithoutNumber(ToothDto tooth)
        {
            var descriptionBuilder = new StringBuilder();

            if (tooth.ConditionName != null)
                descriptionBuilder.Append(string.Format("{0}/", tooth.ConditionName));

            if (tooth.HasBridge)
                descriptionBuilder.Append("Мост");

            descriptionBuilder.Append(GetDescriptionByProducts(tooth));

            var toothDescription = descriptionBuilder.ToString().Trim(' ', '/');

            return toothDescription.ToString();
        }

        private static string GetDescriptionByProducts(ToothDto tooth)
        {
            if (tooth.Products.IsNullOrEmpty())
                return string.Empty;

            var descriptionBuilder = new StringBuilder();
            foreach (var product in tooth.Products)
            {
                descriptionBuilder.Append(string.Format("{0} / {1}, ", product.ProductName, product.MaterialName));
            }

            var result = string.Format(" ({0})", descriptionBuilder.ToString().Trim(' ', ','));
            return result;
        }

        public static string GetToothCardDescription(ToothDto[] toothCollection)
        {
            if (toothCollection.Length == 0)
                return null;

            var groups = toothCollection.GroupBy(GetToothDescriptionWithoutNumber).ToArray();

            var toothCardBuilder = new StringBuilder();
            foreach (var grouping in groups)
            {
                var groupingBuilder = new StringBuilder();
                foreach (var toothDto in grouping)
                {
                    groupingBuilder.Append($"{toothDto.ToothNumber}, ");
                }

                toothCardBuilder.AppendLine(groupingBuilder.ToString().Trim(',', ' ')+ $" - {grouping.Key};   ");
            }
            var groupedToothDescription = toothCardBuilder.ToString();
            return groupedToothDescription = groupedToothDescription.Remove(groupedToothDescription.Length - 6, 6);
        }

        public static string GetAdditionalEquipmentDescription(OrderDto order)
        {
            if (order.AdditionalEquipment == null)
                return null;

            var builder = new StringBuilder();
            foreach (var dto in order.AdditionalEquipment)
            {
                if (dto.QuantityIn > 0)
                builder.AppendLine(string.Format("{0} - {1}, ", dto.EquipmentName, dto.QuantityIn));
            }

            return builder.ToString().Trim(' ', ',');
        }

        public static string GetUnderstaff(OrderDto order)
        {
            if (order.AdditionalEquipment == null)
                return null;

            var builder = new StringBuilder();
            foreach (var dto in order.AdditionalEquipment)
            {
                if (dto.QuantityOut > 0)
                    builder.AppendLine(string.Format("{0} - {1}, ", dto.EquipmentName, dto.QuantityOut));
            }

            return builder.ToString().Trim(' ', ',');
        }

        public static string GetAttributesValuesToReport(OrderDto order, string attrKeyName)
        {
            if (order.Attributes == null)
                return null;

            var builder = new StringBuilder();
            foreach (var dto in order.Attributes.Where(x => x.AttributeKeyName == attrKeyName))
            {
                builder.Append(string.Format(dto.AttributeValue, ", "));
            }

            return builder.ToString().Trim(' ', ',');
        }
    }
}
