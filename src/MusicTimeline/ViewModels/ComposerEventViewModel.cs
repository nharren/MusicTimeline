using NathanHarrenstein.ClassicalMusicDb;
using NathanHarrenstein.Timeline;
using System.Collections.Generic;
using System.EDTF;
using System.Windows.Input;
using System.Windows.Media;

namespace NathanHarrenstein.MusicTimeline.ViewModels
{
    public class ComposerEventViewModel : ITimelineEvent
    {
        private readonly Brush _background;
        private readonly ICommand _clickCommand;
        private readonly Composer _composer;
        private readonly ExtendedDateTimeInterval _dates;
        private readonly IEnumerable<ComposerEraViewModel> _eras;
        private readonly Brush _foreground;
        private readonly string _label;

        public ComposerEventViewModel(string label, ExtendedDateTimeInterval dates, Composer composer, Brush background, Brush foreground, IEnumerable<ComposerEraViewModel> eras, ICommand clickCommand)
        {
            _dates = dates;
            _composer = composer;
            _background = background;
            _foreground = foreground;
            _label = label;
            _eras = eras;
            _clickCommand = clickCommand;
        }

        public Brush Background
        {
            get
            {
                return _background;
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

        public IEnumerable<ComposerEraViewModel> Eras
        {
            get
            {
                return _eras;
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