using System.Threading.Tasks;
using System.Windows.Threading;
namespace NathanHarrenstein.ComposerTimeline.Initializers
{
    public static class TimelinePageInitializer
    {
        public static void Initialize(TimelinePage timelinePage)
        {
            TimelineProvider.GetTimeline().ContinueWith(t => App.Current.Dispatcher.Invoke(() => timelinePage.rootGrid.Children.Add(t.Result)));
        }
    }
}