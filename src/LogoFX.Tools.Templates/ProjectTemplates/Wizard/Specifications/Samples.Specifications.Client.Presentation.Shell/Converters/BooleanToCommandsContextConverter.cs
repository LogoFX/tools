using System;
using System.Globalization;
using System.Windows.Data;

namespace $safeprojectname$.Converters
{
    public class BooleanToCommandsContextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isNew = (bool) value;
            return isNew ? "New" : "Existing";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}