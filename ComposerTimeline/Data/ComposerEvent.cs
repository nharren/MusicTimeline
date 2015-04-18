using Database;
using NathanHarrenstein.Timeline;
using System.Collections.Generic;
using System.ExtendedDateTimeFormat;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.ComposerTimeline
{
    public class ComposerEvent : ITimelineEvent
    {
        private readonly Brush _background;
        private readonly string _born;
        private readonly ICommand _clickCommand;
        private readonly Composer _composer;
        private readonly ExtendedDateTimeInterval _dates;
        private readonly string _died;
        private readonly IEnumerable<MusicEra> _eras;
        private readonly IEnumerable<Flag> _flags;
        private readonly Brush _foreground;
        private readonly BitmapImage _image;
        private readonly string _label;
        private readonly ICommand _playPopularCommand;

        public ComposerEvent(string label, ExtendedDateTimeInterval dates, string born, string died, Composer composer, Brush background, Brush foreground, BitmapImage image, IEnumerable<Flag> flags, IEnumerable<MusicEra> eras, ICommand clickCommand, ICommand playPopularCommand)
        {
            _dates = dates;
            _born = born;
            _died = died;
            _composer = composer;
            _background = background;
            _foreground = foreground;
            _label = label;
            _image = image;
            _flags = flags;
            _eras = eras;
            _clickCommand = clickCommand;
            _playPopularCommand = playPopularCommand;
        }

        public Brush Background
        {
            get
            {
                return _background;
            }
        }

        public string Born
        {
            get
            {
                return _born;
            }
        }

        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand;
            }
        }

        public Composer Composer
        {
            get
            {
                return _composer;
            }
        }

        public ExtendedDateTimeInterval Dates
        {
            get
            {
                return _dates;
            }
        }

        public string Died
        {
            get
            {
                return _died;
            }
        }

        public IEnumerable<MusicEra> Eras
        {
            get
            {
                return _eras;
            }
        }

        public IEnumerable<Flag> Flags
        {
            get
            {
                return _flags;
            }
        }

        public Brush Foreground
        {
            get
            {
                return _foreground;
            }
        }

        public BitmapImage Image
        {
            get
            {
                return _image;
            }
        }

        public string Label
        {
            get
            {
                return _label;
            }
        }

        public ICommand PlayPopularCommand
        {
            get
            {
                return _playPopularCommand;
            }
        }
    }
}