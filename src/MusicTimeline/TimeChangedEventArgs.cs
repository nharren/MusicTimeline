using System;

namespace NathanHarrenstein.MusicTimeline
{
    public class TimeChangedEventArgs : EventArgs
    {
        private readonly TimeSpan? _newTime;
        private readonly TimeSpan? _oldTime;

        public TimeChangedEventArgs(TimeSpan? oldTime, TimeSpan? newTime)
        {
            _oldTime = oldTime;
            _newTime = newTime;
        }

        public TimeSpan? NewTime
        {
            get
            {
                return _newTime;
            }
        }

        public TimeSpan? OldTime
        {
            get
            {
                return _oldTime;
            }
        }
    }
}