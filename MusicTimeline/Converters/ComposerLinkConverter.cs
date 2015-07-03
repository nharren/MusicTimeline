using NathanHarrenstein.MusicDb;
using NathanHarrenstein.MusicTimeline.Providers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace NathanHarrenstein.MusicTimeline.Converters
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