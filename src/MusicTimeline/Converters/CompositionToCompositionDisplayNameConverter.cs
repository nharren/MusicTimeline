using NathanHarrenstein.MusicTimeline.Data;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace NathanHarrenstein.MusicTimeline.Converters
{
    internal class CompositionToCompositionDisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return CompositionToCompositionDisplayName((Composition)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string CompositionToCompositionDisplayName(Composition composition)
        {
            if (composition == null)
            {
                return null;
            }

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
    }
}