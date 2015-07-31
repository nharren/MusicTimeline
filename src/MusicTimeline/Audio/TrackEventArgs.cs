using NAudio.Wave;
using System;

namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class TrackEventArgs : EventArgs
    {
        private readonly ISampleProvider _track;

        public TrackEventArgs(ISampleProvider track)
        {
            _track = track;
        }

        public ISampleProvider Track
        {
            get
            {
                return _track;
            }
        }
    }
}