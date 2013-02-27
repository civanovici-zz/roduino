using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace RoDuino.SMS.Converters
{
    public class ConfigStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string key = "";
            if (parameter != null)
                key = parameter.ToString();
            return ConfigurationManager.AppSettings[key];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
