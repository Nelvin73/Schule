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

    public class RadioButtonToEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // value = Enum Wert; Rückgabe True/False
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // value = bool; Rückgabe enum (parameter)
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }


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

    public class BooleanInverterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
            {
                try
                {
                    return !System.Convert.ToBoolean(value);
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("The target must be a boolean");                    
                }

            }               

            return !(bool)value;           
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
    
    /// <summary>
    /// Converter gibt zurück, ob Objekt, String, List oder IEnumerable NULL oder EMPTY ist.
    /// Wenn parameter = TRUE, wird ERgebnis invertiert.
    /// </summary>
    public class IsNullOrEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool Invert = ((parameter ?? "").ToString().ToLower() == "true");

            if (value == null)
                return true ^ Invert;

            if (value is string)
                return String.IsNullOrEmpty((string)value) ^ Invert;

            else if (value is IList<object>)
                return (((IList<object>)value).Count == 0) ^ Invert;

            else if (value is IEnumerable<object>)
                return (((IEnumerable<object>)value).Count() == 0) ^ Invert;

            else
                return new ArgumentOutOfRangeException("value");
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    public class ObjectDictConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var i = value as Dictionary<int, int>;
            if (i == null)
                return null;

            return  i.ToList().ToDictionary( x => x.Key.ToString(), y => (string)  y.Value.ToString() );            
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
