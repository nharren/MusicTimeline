using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace NathanHarrenstein.MusicTimeline.Converters
{
    public class UrlToTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return UrlToTitle((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static string UrlToTitle(string url)
        {
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;

            var htmlSource = (string)null;

            try
            {
                htmlSource = webClient.DownloadString(url);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString(), "MusicTimeline.log");
            }

            var match = Regex.Match(htmlSource, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase);
            var group = match.Groups["Title"];

            return group.Value;
        }
    }
}