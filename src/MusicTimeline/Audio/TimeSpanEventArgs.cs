using System;

namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class TimeSpanEventArgs : EventArgs
    {
        private readonly TimeSpan _timeSpan;

        public TimeSpanEventArgs(TimeSpan timeSpan)
        {
            _timeSpan = timeSpan;
        }

        public TimeSpan TimeSpan
        {
            get
            {
                return _timeSpan;
            }
        }
    }
}