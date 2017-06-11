using System;

namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class CanPlayNextEventArgs : EventArgs
    {
        public CanPlayNextEventArgs(bool canPlayNext)
        {
            CanPlayNext = canPlayNext;
        }

        public bool CanPlayNext { get; private set; }
    }
}