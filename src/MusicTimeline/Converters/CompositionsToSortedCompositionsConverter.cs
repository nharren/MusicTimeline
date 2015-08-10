using NathanHarrenstein.ClassicalMusicDb;
using NathanHarrenstein.MusicTimeline.Comparers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace NathanHarrenstein.MusicTimeline.Converters
{
    internal class CompositionsToSortedCompositionsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return CompositionsToSortedCompositions((IEnumerable<Composition>)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Composition> CompositionsToSortedCompositions(IEnumerable<Composition> value)
        {
            return value.OrderBy(c => c.Name, new LogicalComparer());
        }
    }
}