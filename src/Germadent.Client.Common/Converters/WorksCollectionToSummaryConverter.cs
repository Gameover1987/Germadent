using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Germadent.Client.Common.ViewModels;

namespace Germadent.Client.Common.Converters
{
    public class WorksCollectionToSummaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var works = (IEnumerable) value;
            return works.Cast<WorkViewModel>().Select(x => x.OperationCost).Sum();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
