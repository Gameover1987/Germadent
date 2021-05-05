using System;
using System.Globalization;
using System.Windows.Data;
using Germadent.Model.Rights;

namespace Germadent.UserManagementCenter.App.Views.Converters
{
    public class ApplicationModuleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var appModule = (ApplicationModule) value;
            switch (appModule)
            {
                case ApplicationModule.Rma:
                    return "Рабочее место администратора";

                case ApplicationModule.Umc:
                    return "Центр управления пользователями";

                

                default:
                    throw new NotSupportedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
