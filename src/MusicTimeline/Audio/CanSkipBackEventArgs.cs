namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class CanSkipBackEventArgs
    {
        private readonly bool _canSkipBack;

        public CanSkipBackEventArgs(bool canSkipBack)
        {
            _canSkipBack = canSkipBack;
        }

        public bool CanSkipBack
        {
            get
            {
                return _canSkipBack;
            }
        }
    }
}