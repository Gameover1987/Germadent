using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Germadent.Client.Common.Reporting.PropertyGrid
{
    public interface IPropertyItemsCollector
    {
        PropertyItem[] GetProperties(object obj);
    }

    public class PropertyItemsCollector : IPropertyItemsCollector
    {
        public PropertyItem[] GetProperties(object obj)
        {
            if (obj == null)
                return new PropertyItem[0];

            var type = obj.GetType();

            var properties = type.GetProperties().ToArray();
            var propertyItems = new List<PropertyItem>();
            foreach (var propertyInfo in properties)
            {
                var propertyAttribute = (PrintableOrderPropertyAttribute)propertyInfo.GetCustomAttributes(typeof(PrintableOrderPropertyAttribute)).FirstOrDefault();
                if (propertyAttribute == null)
                    continue;

                var propetyValue = propertyInfo.GetValue(obj);
                if (propetyValue == null || propetyValue.ToString() == string.Empty)
                    continue;

                propertyItems.Add(new PropertyItem
                {
                    DisplayName = propertyAttribute.DisplayName,
                    GroupName = propertyAttribute.GroupName,
                    Value = propetyValue.ToString()
                });
            }

            return propertyItems.ToArray();
        }
    }
}