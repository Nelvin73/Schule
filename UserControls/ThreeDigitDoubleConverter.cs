using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Groll.UserControls
{
    [ValueConversion(typeof(double), typeof(String))]
    public class ThreeDigitDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                double d = (double)value;
                if (d > 99 || d < -99)
                    return Math.Round(d, 0).ToString();
                else
                    return Math.Round(d, 1).ToString();
            }
            else
                throw new ArgumentException("value is not double");

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
