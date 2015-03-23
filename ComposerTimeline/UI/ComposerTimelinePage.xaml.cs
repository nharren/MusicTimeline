using NathanHarrenstein.ComposerTimeline.UI.Initializers;
using System.Windows.Controls;

namespace NathanHarrenstein.ComposerTimeline.UI
{
    /// <summary>
    /// Interaction logic for ComposerTimelinePage.xaml
    /// </summary>
    public partial class ComposerTimelinePage : Page
    {
        public ComposerTimelinePage()
        {
            ComposerTimelinePageInitializer.Initialize(this);

            InitializeComponent();
        }
    }
}