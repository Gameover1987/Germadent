using System;
using System.Globalization;
using System.Windows.Data;
using Germadent.Common.Extensions;
using Germadent.Model;

namespace Germadent.Client.Common.Converters
{
    public class BranchTypeToStringConverter : IValueConverter
    {
        private static BranchTypeToStringConverter _instance;

        public static BranchTypeToStringConverter Instance
        {
            get { return _instance ??= new BranchTypeToStringConverter(); }
        }

        public static string Convert(BranchType branchType)
        {
            return branchType.GetDescription();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert((BranchType) value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
