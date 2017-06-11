using NathanHarrenstein.MusicTimeline.Parsers;
using NathanHarrenstein.MusicTimeline.Utilities;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.MusicTimeline.Converters
{
    public class YouTubeToThumbConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var url = (string)value;

            var youTubeParser = new YouTubeParser();
            var videoId = youTubeParser.ParseVideoId(url);

            return $"https://i.ytimg.com/vi/{videoId}/hqdefault.jpg";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}