using NathanHarrenstein.MusicTimeline.Parsers;
using System;
using System.Globalization;
using System.Windows.Data;

namespace NathanHarrenstein.MusicTimeline.Converters
{
    public class SpotifyToEmbedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var url = (string)value;

            var spotifyParser = new SpotifyParser();
            var trackId = spotifyParser.ParseTrackId(url);

            return $@"<iframe src=""https://embed.spotify.com/?uri=spotify:track:{trackId}"" frameborder=""0"" allowtransparency=""true""></iframe>";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}