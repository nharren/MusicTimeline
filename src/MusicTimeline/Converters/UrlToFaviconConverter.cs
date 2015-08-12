using NathanHarrenstein.MusicTimeline.Utilities;
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
                favicon = new BitmapImage();
                favicon.BeginInit();
                favicon.DecodePixelHeight = 16;
                favicon.DecodePixelWidth = 16;
                favicon.StreamSource = new MemoryStream(File.ReadAllBytes(faviconPath));
                favicon.EndInit();
                favicon.Freeze();

                _faviconCache[url] = favicon;

                return favicon;
            }
            else
            {
                var bytes = FileUtility.GetImageAsync($"http://{uri.Host}/favicon.ico").Result;

                if (bytes != null)
                {
                    File.WriteAllBytes(faviconPath, bytes);

                    favicon = new BitmapImage();
                    favicon.BeginInit();
                    favicon.DecodePixelHeight = 16;
                    favicon.DecodePixelWidth = 16;
                    favicon.StreamSource = new MemoryStream(bytes);
                    favicon.EndInit();
                    favicon.Freeze();

                    _faviconCache[url] = favicon;
                }
                else
                {
                    if (!_faviconCache.TryGetValue("", out favicon))
                    {
                        favicon = new BitmapImage();
                        favicon.BeginInit();
                        favicon.DecodePixelHeight = 16;
                        favicon.DecodePixelWidth = 16;
                        favicon.StreamSource = new MemoryStream(File.ReadAllBytes($@"{Environment.CurrentDirectory}\Resources\Favicons\Default.ico"));
                        favicon.EndInit();
                        favicon.Freeze();

                        _faviconCache[url] = favicon;
                    }
                }

                return favicon;              
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}