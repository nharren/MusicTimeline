﻿using NathanHarrenstein.MusicTimeline.Utilities;
using System;
using System.Globalization;
using System.Windows.Data;

namespace NathanHarrenstein.MusicTimeline.Converters
{
    public class UrlToTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return WebUtility.GetTitle((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}