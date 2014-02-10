﻿using System;
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
}
