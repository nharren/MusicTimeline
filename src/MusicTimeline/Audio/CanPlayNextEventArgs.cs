namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class CanPlayNextEventArgs
    {
        public CanPlayNextEventArgs(bool canPlayNext)
        {
            CanPlayNext = canPlayNext;
        }

        public bool CanPlayNext { get; private set; }
    }
}