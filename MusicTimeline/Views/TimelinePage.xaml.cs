using NathanHarrenstein.MusicTimeline.Initializers;
using System.Windows.Controls;

namespace NathanHarrenstein.MusicTimeline.Views
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