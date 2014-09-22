using Groll.Schule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Groll.Schule.SchulDB.Helper
{
    [ValueConversion(typeof(Stundenplan), typeof(Unterrichtsstunde))]
    public class StundenplanToSchulstundeConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (! (value is Stundenplan && parameter is Stundenbezeichnung))
                return null;

            Stundenplan p = (Stundenplan)value;
            Stundenbezeichnung b = (Stundenbezeichnung)parameter;
                        
            System.Diagnostics.Debug.WriteLine("source:" + value.GetType().FullName + "  => target: " + targetType.FullName);
            System.Diagnostics.Debug.WriteLine( p.GetStunde(b));
            return p.GetStunde(b);

          

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException(); return Convert(value, targetType, parameter, culture);
        }
    }


}
