using System;

namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class VolumeEventArgs : EventArgs
    {
        private readonly float _volume;

        public VolumeEventArgs(float volume)
        {
            _volume = volume;
        }

        public float Volume
        {
            get
            {
                return _volume;
            }
        }
    }
}