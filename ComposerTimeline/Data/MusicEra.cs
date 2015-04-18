using NathanHarrenstein.Timeline;
using System.ExtendedDateTimeFormat;
using System.Windows.Media;

namespace NathanHarrenstein.ComposerTimeline
{
    public class MusicEra : ITimelineEra
    {
        private readonly SolidColorBrush _background;
        private readonly ExtendedDateTimeInterval _dates;
        private readonly SolidColorBrush _foreground;
        private readonly string _label;

        public MusicEra(string label, ExtendedDateTimeInterval dates, SolidColorBrush background, SolidColorBrush foreground)
        {
            _dates = dates;
            _background = background;
            _foreground = foreground;
            _label = label;
        }

        public SolidColorBrush Background
        {
            get
            {
                return _background;
            }
        }

        public ExtendedDateTimeInterval Dates
        {
            get
            {
                return _dates;
            }
        }

        public SolidColorBrush Foreground
        {
            get
            {
                return _foreground;
            }
        }

        public string Label
        {
            get
            {
                return _label;
            }
        }
    }
}