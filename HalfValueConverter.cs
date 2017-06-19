using System;
using System.Globalization;
using System.Windows.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EV3_Test;

namespace EV3_Test
{
    public class HalfValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2)
            {
                throw new ArgumentException("HalfValueConverter expects 2 double values to be passed" +
                    " in this order -> totalWidth, width", "values");
            }
            double totalWidth = (double)values[0];
            double width = (double)values[1];
            return (totalWidth - width) / 2;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}