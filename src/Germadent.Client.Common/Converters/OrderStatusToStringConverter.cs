using System;
using System.Globalization;
using System.Windows.Data;
using Germadent.Common.Extensions;
using Germadent.Model;

namespace Germadent.Client.Common.Converters
{
    public class OrderStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var orderStatus = (OrderStatus)value;
            return orderStatus.GetDescription();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}