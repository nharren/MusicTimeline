using NathanHarrenstein.ComposerTimeline.UI.Initializers;
using System.Windows.Controls;

namespace NathanHarrenstein.ComposerTimeline.UI
{
    public partial class TimelinePage : Page
    {
        public TimelinePage()
        {
            InitializeComponent();

            ComposerTimelinePageInitializer.Initialize(this);
        }
    }
}