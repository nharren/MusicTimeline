using System;

namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class CanPlayPreviousEventArgs : EventArgs
    {
        public CanPlayPreviousEventArgs(bool canSkipBack)
        {
            CanPlayPrevious = canSkipBack;
        }

        public bool CanPlayPrevious { get; private set; }
    }
}