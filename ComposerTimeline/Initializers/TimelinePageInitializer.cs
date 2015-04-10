namespace NathanHarrenstein.ComposerTimeline.Initializers
{
    public static class TimelinePageInitializer
    {
        public static void Initialize(TimelinePage timelinePage)
        {
            timelinePage.rootGrid.Children.Add(TimelineProvider.GetTimeline());
        }
    }
}