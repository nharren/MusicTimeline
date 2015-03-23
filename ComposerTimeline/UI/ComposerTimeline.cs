using NathanHarrenstein.ComposerTimeline.UI.Initializers;
using NathanHarrenstein.Timeline;
using System.Windows;

namespace NathanHarrenstein.ComposerTimeline.UI
{
    public class ComposerTimeline : Timeline.Timeline
    {
        static ComposerTimeline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComposerTimeline), new FrameworkPropertyMetadata(typeof(ComposerTimeline)));
        }

        public ComposerTimeline()
        {
            ComposerTimelineInitializer.Initialize(this);
        }

        public override void OnApplyTemplate()
        {
            var composerControlPanel = (ComposerControlPanel)GetTemplateChild("PART_ComposerControlPanel");
            composerControlPanel.Start = Start;
            composerControlPanel.End = End;
            composerControlPanel.EraSettings = EraSettings;
            composerControlPanel.Eras = Eras;
            composerControlPanel.LoadComposerControls();

            var timeDisplay = (TimeDisplay)GetTemplateChild("PART_TimeDisplay");
            timeDisplay.Start = Start;
            timeDisplay.End = End;

            var eraDisplay = (EraDisplay)GetTemplateChild("PART_EraDisplay");
            eraDisplay.Start = Start;
            eraDisplay.End = End;
            eraDisplay.EraSettings = EraSettings;
            eraDisplay.Eras = Eras;
        }
    }
}