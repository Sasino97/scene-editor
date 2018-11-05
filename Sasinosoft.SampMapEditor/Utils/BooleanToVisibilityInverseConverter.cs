﻿/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Sasinosoft.SampMapEditor.Utils
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
