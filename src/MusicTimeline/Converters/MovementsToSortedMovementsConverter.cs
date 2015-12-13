using NathanHarrenstein.MusicTimeline.ClassicalMusicDb;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace NathanHarrenstein.MusicTimeline.Converters
{
    internal class MovementsToSortedMovementsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return MovementsToSortedMovements((IEnumerable<Movement>)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Movement> MovementsToSortedMovements(IEnumerable<Movement> value)
        {
            return value.OrderBy(m => m.Number);
        }
    }
}