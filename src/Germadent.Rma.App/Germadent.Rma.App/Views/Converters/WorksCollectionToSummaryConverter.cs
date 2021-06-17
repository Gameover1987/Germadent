using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Germadent.Rma.App.ViewModels.Salary;

namespace Germadent.Rma.App.Views.Converters
{
    public class WorksCollectionToSummaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var works = (IEnumerable) value;
            return works.Cast<WorkViewModel>().Select(xx => xx.OperationCost).Sum();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
