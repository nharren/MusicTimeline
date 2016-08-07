using NAudio.Wave;
using System;

namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class PlaybackStateEventArgs : EventArgs
    {
        private readonly StreamingPlaybackState _playbackState;

        public PlaybackStateEventArgs(StreamingPlaybackState playbackState)
        {
            _playbackState = playbackState;
        }

        public StreamingPlaybackState PlaybackState
        {
            get
            {
                return _playbackState;
            }
        }
    }
}