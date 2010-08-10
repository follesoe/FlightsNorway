using System;
using System.Windows.Data;
using System.Globalization;

namespace FlightsNorway.ViewModels
{
    public class StringFormatValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime && parameter != null)
            {
                return ((DateTime)value).ToString(parameter.ToString(), culture);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("This converter only supports one-way bindings.");
        }
    }
}
