using NAudio.Wave;
using System;

namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class PlaybackStateChangedEventArgs : EventArgs
    {
        private PlaybackState _newState;
        private PlaybackState _oldState;

        public PlaybackStateChangedEventArgs(PlaybackState oldState, PlaybackState newState)
        {
            _oldState = oldState;
            _newState = newState;
        }

        public PlaybackState NewState
        {
            get
            {
                return _newState;
            }

            set
            {
                _newState = value;
            }
        }

        public PlaybackState OldState
        {
            get
            {
                return _oldState;
            }

            set
            {
                _oldState = value;
            }
        }
    }
}