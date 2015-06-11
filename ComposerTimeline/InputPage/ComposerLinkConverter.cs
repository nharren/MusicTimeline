using Database;
using NathanHarrenstein.ComposerTimeline.Providers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Linq;

namespace NathanHarrenstein.ComposerTimeline
{
    internal class ComposerLinkConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {         
            return ((HashSet<ComposerLink>)value).Select(cl => LinkProvider.GetLink(cl.URL));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}