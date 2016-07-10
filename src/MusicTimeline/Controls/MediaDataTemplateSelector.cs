using NathanHarrenstein.MusicTimeline.ClassicalMusicDb;
using NathanHarrenstein.MusicTimeline.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class MediaDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SpotifyTemplate { get; set; }
        public DataTemplate YouTubeTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var link = item as Link;

            if (link == null)
            {
                throw new InvalidOperationException("Items handled by MediaDataTemplateSelector must be of type Link.");
            }

            var youTubeParser = new YouTubeParser();

            if (youTubeParser.IsValidVideoUrl(link.Url))
            {
                return YouTubeTemplate;               
            }

            var spotfiyParser = new SpotifyParser();

            if (spotfiyParser.IsValidTrackUrl(link.Url))
            {
                return SpotifyTemplate;
            }

            throw new Exception("Item handled by the MediaDataTemplateSelector must contain either YouTube or Spotify URLs.");
        }
    }
}
