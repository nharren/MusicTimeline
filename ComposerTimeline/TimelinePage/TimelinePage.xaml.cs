using NathanHarrenstein.ComposerTimeline.Initializers;
using System.Windows.Controls;

namespace NathanHarrenstein.ComposerTimeline
{
    public partial class TimelinePage : Page
    {
        public TimelinePage()
        {
            InitializeComponent();

            TimelineInitializer.Initialize(timeline);
        }
    }
}