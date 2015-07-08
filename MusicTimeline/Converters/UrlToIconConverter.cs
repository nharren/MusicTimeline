using NathanHarrenstein.MusicTimeline.Utilities;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows.Data;

namespace NathanHarrenstein.MusicTimeline.Converters
{
    public class UrlToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var linkUri = new Uri((string)value);
            var localIconPath = Path.Combine(Environment.CurrentDirectory, "Resources/Favicons/" + linkUri.Host + ".ico");
            var localIconUri = new Uri(localIconPath);

            if (!Directory.Exists("Resources/Favicons"))
            {
                Directory.CreateDirectory("Resources/Favicons");
            }

            if (File.Exists(localIconPath))
            {
                return localIconPath;
            }
            else
            {
                var client = new WebClient();
                var webIconPath = "http://" + linkUri.Host + "/favicon.ico";

                var favicon = FileUtility.GetFile(webIconPath);

                if (favicon == null)
                {
                    return Path.Combine(Environment.CurrentDirectory, "Resources/Favicons/Default.ico");
                }

                File.WriteAllBytes(localIconPath, favicon);

                return localIconPath;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}