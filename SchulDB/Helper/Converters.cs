using Groll.Schule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Groll.Schule.SchulDB.Helper
{
    public class HiddenIfEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value != null && !string.IsNullOrEmpty(value.ToString()) ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
    [ValueConversion(typeof(int), typeof(Schuljahr))]
    public class IntToSchuljahrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.Diagnostics.Debug.WriteLine("source:" + value.GetType().FullName + "  => target: " + targetType.FullName);
            if (targetType == typeof(Schuljahr))
            {
                int i = 0;
                try
                {
                    i = System.Convert.ToInt16(value);
                    return new Schuljahr(i);
                }
                catch
                {
                    return null;
                }
            }
            else if (targetType == typeof(Double) || targetType == typeof(int))
            {
                var s = value as Schuljahr;
                return s == null ? 0 : s.Startjahr;
            }
            else
            {
                return value.ToString();
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }

}
