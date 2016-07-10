using NathanHarrenstein.MusicTimeline.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.MusicTimeline.Converters
{
    public class UrlToFaviconConverter : IValueConverter
    {
        private static readonly Dictionary<string, BitmapImage> cache = new Dictionary<string, BitmapImage>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return UrlToFavicon((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private BitmapImage UrlToFavicon(string url)
        {
            var favicon = (BitmapImage)null;
            var uri = (Uri)null;

            if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
            {
                return GetDefaultFavicon(url);
            }

            if (cache.TryGetValue(uri.Host, out favicon))
            {
                return favicon;
            }

            byte[] bytes;

            if (FileUtility.TryGetImage($"http://{uri.Host}/favicon.ico", out bytes))
            {
                favicon = new BitmapImage();
                favicon.BeginInit();
                favicon.DecodePixelHeight = 16;
                favicon.DecodePixelWidth = 16;
                favicon.StreamSource = new MemoryStream(bytes);
                favicon.EndInit();
                favicon.Freeze();

                cache[url] = favicon;

                return favicon;
            }
            else
            {
                return GetDefaultFavicon(url);
            }
        }

        private static BitmapImage GetDefaultFavicon(string url)
        {
            BitmapImage favicon;

            if (!cache.TryGetValue("", out favicon))
            {
                var defaultFaviconUri = new Uri("pack://application:,,,/Resources/Favicons/Default.png", UriKind.Absolute);
                var stream = Application.GetResourceStream(defaultFaviconUri).Stream;

                favicon = new BitmapImage();
                favicon.BeginInit();
                favicon.DecodePixelHeight = 16;
                favicon.DecodePixelWidth = 16;
                favicon.StreamSource = stream;
                favicon.EndInit();
                favicon.Freeze();

                cache[""] = favicon;
            }

            return favicon;
        }
    }
}