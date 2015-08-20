using NathanHarrenstein.Timeline;
using System.EDTF;
using System.Windows.Media;

namespace NathanHarrenstein.MusicTimeline.ViewModels
{
    public class ComposerEraViewModel : ITimelineEra
    {
        private readonly Brush _background;
        private readonly SolidColorBrush _foreground;
        private readonly string _label;
        private ExtendedDateTimeInterval _dates;

        public ComposerEraViewModel(string label, ExtendedDateTimeInterval dates, Brush background, SolidColorBrush foreground)
        {
            _dates = dates;

            if ((ExtendedDateTime)dates.End == ExtendedDateTime.Open)
            {
                _dates.End = ExtendedDateTime.Now;
            }

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

            set
            {
                _dates = value;
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