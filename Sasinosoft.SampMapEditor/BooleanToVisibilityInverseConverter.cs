﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Sasinosoft.SampMapEditor
{
    public class BooleanToVisibilityInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Visibility)value == Visibility.Collapsed) ? true : false;
        }
    }
}
