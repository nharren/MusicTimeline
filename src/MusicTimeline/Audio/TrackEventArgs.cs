using NAudio.Wave;
using System;

namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class TrackEventArgs : EventArgs
    {
        private readonly IWaveProvider _track;

        public TrackEventArgs(IWaveProvider track)
        {
            _track = track;
        }

        public IWaveProvider Track
        {
            get
            {
                return _track;
            }
        }
    }
}