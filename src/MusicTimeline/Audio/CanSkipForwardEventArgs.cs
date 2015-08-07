namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class CanSkipForwardEventArgs
    {
        private readonly bool _canSkipForward;

        public CanSkipForwardEventArgs(bool canSkipForward)
        {
            _canSkipForward = canSkipForward;
        }

        public bool CanSkipForward
        {
            get
            {
                return _canSkipForward;
            }
        }
    }
}