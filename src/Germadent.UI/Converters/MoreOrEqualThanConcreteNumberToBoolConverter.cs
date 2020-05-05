using System;
using System.Globalization;
using System.Windows.Data;

namespace Germadent.UI.Converters
{
    public class MoreOrEqualThanConcreteNumberToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var number = (int) value;
            var threshold = (int) parameter;

            return number >= threshold;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
