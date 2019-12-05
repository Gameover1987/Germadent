using System;
using System.Globalization;
using System.Windows.Data;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Converters
{
    public class BranchTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var branchType = (BranchType) value;
            switch (branchType)
            {
                case BranchType.MillingCenter:
                    return "Фрезерный центр";

                case BranchType.Laboratory:
                    return "Лаборатория";

                default:
                    throw new NotImplementedException("Неизвестный тип филиала");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
