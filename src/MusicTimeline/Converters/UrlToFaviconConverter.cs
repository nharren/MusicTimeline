using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.MusicTimeline.Converters
{
    public class UrlToFaviconConverter : IValueConverter
    {
        private static readonly Dictionary<string, BitmapImage> _faviconCache = new Dictionary<string, BitmapImage>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return UrlToFavicon((string)value);
        }

        private BitmapImage UrlToFavicon(string url)
        {
            var favicon = (BitmapImage)null;
            var uri = (Uri)null;

            if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
            {
                throw new ArgumentException($"A uri could not be created from the url: {url}");
            }

            if (_faviconCache.TryGetValue(uri.Host, out favicon))
            {
                return favicon;
            }

            var faviconPath = $@"{Environment.CurrentDirectory}\Resources\Favicons\{uri.Host}.ico";

            if (File.Exists(faviconPath))
            {
                favicon = new BitmapImage(new Uri(faviconPath));
                _faviconCache[url] = favicon;

                return favicon;
            }
            else
            {
                var faviconUrl = $"http://{uri.Host}/favicon.ico";
                var faviconUri = (Uri)null;

                if (Uri.TryCreate(faviconUrl, UriKind.Absolute, out faviconUri))
                {
                    try
                    {
                        favicon = new BitmapImage(faviconUri);
                        _faviconCache[url] = favicon;

                        return favicon;
                    }
                    catch (Exception)
                    {
                    }
                }

                if (_faviconCache.TryGetValue("", out favicon))
                {
                    return favicon;
                }
                else
                {
                    faviconUrl = $@"{Environment.CurrentDirectory}\Resources\Favicons\Default.ico";
                    faviconUri = new Uri(faviconUrl);
                    favicon = new BitmapImage(faviconUri);
                    _faviconCache[url] = favicon;

                    return favicon;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}