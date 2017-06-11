using MusicTimelineWebApi.Models;
using NathanHarrenstein.Timeline;
using System.Collections.Generic;
using System.EDTF;
using System.Windows.Input;
using System.Windows.Media;

namespace NathanHarrenstein.MusicTimeline.ViewModels
{
    public class ComposerEventViewModel : ITimelineEvent
    {
        private readonly Brush background;
        private readonly ICommand clickCommand;
        private readonly Composer composer;
        private readonly ExtendedDateTimeInterval dates;
        private readonly Brush foreground;
        private readonly string label;
        private readonly string thumbnail;

        public ComposerEventViewModel(string label, ExtendedDateTimeInterval dates, Composer composer, Brush background, Brush foreground, ICommand clickCommand, string thumbnail)
        {
            this.dates = dates;
            this.composer = composer;
            this.background = background;
            this.foreground = foreground;
            this.label = label;
            this.clickCommand = clickCommand;
            this.thumbnail = thumbnail;
        }

        public Brush Background
        {
            get
            {
                return background;
            }
        }

        public ICommand ClickCommand
        {
            get
            {
                return clickCommand;
            }
        }

        public Composer Composer
        {
            get
            {
                return composer;
            }
        }

        public ExtendedDateTimeInterval Dates
        {
            get
            {
                return dates;
            }
        }

        public Brush Foreground
        {
            get
            {
                return foreground;
            }
        }

        public string Label
        {
            get
            {
                return label;
            }
        }

        public string Thumbnail => thumbnail;
    }
}