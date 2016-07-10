using NathanHarrenstein.MusicTimeline.Parsers;
using System;
using System.Globalization;
using System.Windows.Data;

namespace NathanHarrenstein.MusicTimeline.Converters
{
    public class YouTubeToEmbedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var url = (string)value;

            var youTubeParser = new YouTubeParser();
            var videoId = youTubeParser.ParseVideoId(url);

            return $@"https://www.youtube.com/embed/{videoId}?rel=0&showinfo=0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}