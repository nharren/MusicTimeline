using NathanHarrenstein.ClassicalMusicDb;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NathanHarrenstein.MusicTimeline.Converters
{
    class CompositionToCompositionDisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return CompositionToCompositionDisplayName((Composition)value);
        }

        private string CompositionToCompositionDisplayName(Composition composition)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(composition.Name);

            var catalogNumber = composition.CatalogNumbers.FirstOrDefault();

            if (catalogNumber != null)
            {
                stringBuilder.Append(", ");
                stringBuilder.Append(catalogNumber.Catalog.Prefix);
                stringBuilder.Append(" ");
                stringBuilder.Append(catalogNumber.Value);
            }

            return stringBuilder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
