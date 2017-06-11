using System;

namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class CanPlayEventArgs : EventArgs
    {
        private readonly bool _canPlay;

        public CanPlayEventArgs(bool canPlay)
        {
            _canPlay = canPlay;
        }

        public bool CanPlay
        {
            get
            {
                return _canPlay;
            }
        }
    }
}