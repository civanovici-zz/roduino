using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace RoDuino.SMS.Converters
{
    public class GridConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null) parameter = "1";
            if (value == null) value = 0;
            int count = 1;
             int.TryParse(parameter.ToString(),out count );
            double val = 0;
            double.TryParse(value.ToString(), out val);
            return val/count-30;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
