namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class CanPlayPreviousEventArgs
    {
        public CanPlayPreviousEventArgs(bool canSkipBack)
        {
            CanPlayPrevious = canSkipBack;
        }

        public bool CanPlayPrevious { get; private set; }
    }
}