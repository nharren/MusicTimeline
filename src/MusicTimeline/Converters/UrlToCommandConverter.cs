using NathanHarrenstein.MusicTimeline.Input;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace NathanHarrenstein.MusicTimeline.Converters
{
    public class UrlToCommandConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new DelegateCommand(o => Process.Start((string)value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}