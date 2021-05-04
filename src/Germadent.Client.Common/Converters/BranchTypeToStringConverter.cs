using System;
using System.Globalization;
using System.Windows.Data;
using Germadent.Common.Extensions;
using Germadent.Model;

namespace Germadent.Client.Common.Converters
{
    public class BranchTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var branchType = (BranchType) value;
            return EnumExtensions.GetDescription(branchType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
