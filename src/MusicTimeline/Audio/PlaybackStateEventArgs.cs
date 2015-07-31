using NAudio.Wave;
using System;

namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class PlaybackStateEventArgs : EventArgs
    {
        private readonly PlaybackState _playbackState;

        public PlaybackStateEventArgs(PlaybackState playbackState)
        {
            _playbackState = playbackState;
        }

        public PlaybackState PlaybackState
        {
            get
            {
                return _playbackState;
            }
        }
    }
}