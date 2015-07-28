using NAudio.Flac;
using NAudio.Wave;
using System;

namespace NathanHarrenstein.MusicTimeline
{
    public class TrackChangedEventArgs : EventArgs
    {
        private readonly ISampleProvider _current;
        private readonly ISampleProvider _previous;

        public TrackChangedEventArgs(ISampleProvider previous, ISampleProvider current)
        {
            _previous = previous;
            _current = current;
        }

        public ISampleProvider Current
        {
            get
            {
                return _current;
            }
        }

        public ISampleProvider Previous
        {
            get
            {
                return _previous;
            }
        }
    }
}