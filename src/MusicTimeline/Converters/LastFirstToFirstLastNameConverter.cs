using System;
using System.Globalization;
using System.Windows.Data;

namespace NathanHarrenstein.MusicTimeline.Converters
{
    public class LastFirstToFirstLastNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var name = (string)value;
            var delimiters = new string[] { ", " };

            var nameParts = name.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            if (nameParts.Length > 2)
            {
                throw new InvalidOperationException("The name \"" + name + "\" had more than one delimiter and cannot be properly converted to first name last name format.");
            }

            if (nameParts.Length > 1)
            {
                return nameParts[1] + " " + nameParts[0];
            }
            else
            {
                return nameParts[0];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}