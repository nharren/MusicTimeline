using System;

namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class MuteEventArgs : EventArgs
    {
        private readonly bool _mute;

        public MuteEventArgs(bool mute)
        {
            _mute = mute;
        }

        public bool Mute
        {
            get
            {
                return _mute;
            }
        }
    }
}