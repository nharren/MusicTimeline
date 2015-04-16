using NathanHarrenstein.Timeline;
using System.ExtendedDateTimeFormat;
using System.Windows.Media;

namespace TimelineTest
{
    public class TimelineEvent : ITimelineEvent
    {
        private readonly Brush _background;
        private readonly ExtendedDateTimeInterval _dates;
        private readonly TimelineEra _era;
        private readonly Brush _foreground;
        private readonly string _label;

        public TimelineEvent(string label, ExtendedDateTimeInterval dates, TimelineEra era)
        {
            _dates = dates;
            _background = era.Background;
            _foreground = era.Foreground;
            _label = label;
            _era = era;
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

        public TimelineEra Eras
        {
            get
            {
                return _era;
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