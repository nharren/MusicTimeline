using NathanHarrenstein.Timeline;
using System.ExtendedDateTimeFormat;
using System.Windows.Media;

namespace TimelineTest
{
    public class TimelineEra : ITimelineEra
    {
        private readonly Brush _background;
        private readonly ExtendedDateTimeInterval _dates;
        private readonly Brush _foreground;
        private readonly string _label;

        public TimelineEra(string label, ExtendedDateTimeInterval dates, Brush background, Brush foreground)
        {
            _dates = dates;
            _background = background;
            _foreground = foreground;
            _label = label;
        }

        public Brush Background
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

        public Brush Foreground
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