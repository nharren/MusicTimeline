using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.MusicTimeline.Converters
{
    public class NationalityToFlagConverter : IValueConverter
    {
        private static readonly Dictionary<string, BitmapImage> _largeFlagCache = new Dictionary<string, BitmapImage>();
        private static readonly Dictionary<string, BitmapImage> _smallFlagCache = new Dictionary<string, BitmapImage>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return NationalityToFlag((string)value, (string)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private BitmapImage NationalityToFlag(string nationality, string size)
        {
            if (nationality == null)
            {
                nationality = "Unknown";
            }

            var flag = (BitmapImage)null;

            if (size == "Small")
            {
                if (_smallFlagCache.TryGetValue(nationality, out flag))
                {
                    return flag;
                }
                else
                {
                    var path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create)}\Music Timeline\Resources\Flags\16\{nationality}.png";
                    var uri = new Uri(path, UriKind.Absolute);
                    flag = new BitmapImage(uri);
                    _smallFlagCache[nationality] = flag;

                    return flag;
                }
            }
            else
            {
                if (_largeFlagCache.TryGetValue(nationality, out flag))
                {
                    return flag;
                }
                else
                {
                    var path = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create)}\Music Timeline\Resources\Flags\32\{nationality}.png";
                    var uri = new Uri(path);
                    flag = new BitmapImage(uri);
                    _largeFlagCache[nationality] = flag;

                    return flag;
                }
            }
        }
    }
}