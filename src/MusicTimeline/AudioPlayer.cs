using NAudio.Flac;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace NathanHarrenstein.MusicTimeline
{
    public class AudioPlayer : IDisposable
    {
        private readonly LinkedList<FlacReader> _playlist;
        private LinkedListNode<FlacReader> _currentPlaylistItem;
        private TimeSpan? _currentTime;
        private DispatcherTimer _playbackTimer;
        private WaveOutEvent _waveOutEvent;
        private TimeSpan? _totalTime;
        private float _volume;
        private bool _disposedValue = false;
        private Thread _audioProcessingThread;
        private bool _isInitialized;

        public AudioPlayer()
        {
            _playlist = new LinkedList<FlacReader>();
            _playbackTimer = new DispatcherTimer();
            _playbackTimer.Interval = new TimeSpan(1);
            _playbackTimer.Tick += PlaybackTimer_Tick;
        }

        ~AudioPlayer()
        {
            Dispose(false);
        }

        public event EventHandler<TimeChangedEventArgs> CurrentTimeChanged;

        public event EventHandler<PlaybackStateChangedEventArgs> PlaybackStateChanged;

        public event EventHandler<TimeChangedEventArgs> TotalTimeChanged;

        public event EventHandler<TrackChangedEventArgs> TrackChanged;

        public LinkedListNode<FlacReader> CurrentPlaylistItem
        {
            get
            {
                return _currentPlaylistItem;
            }

            set
            {
                _currentPlaylistItem = value;
            }
        }

        public TimeSpan? CurrentTime
        {
            get
            {
                return _currentTime;
            }

            set
            {
                _currentPlaylistItem.Value.CurrentTime = value.Value;
            }
        }

        public PlaybackState PlaybackState
        {
            get
            {
                return _waveOutEvent.PlaybackState;
            }
        }

        public LinkedList<FlacReader> Playlist
        {
            get
            {
                return _playlist;
            }
        }

        public TimeSpan? TotalTime
        {
            get
            {
                return _totalTime;
            }
        }

        public float Volume
        {
            get
            {
                return _waveOutEvent.Volume;
            }

            set
            {
                _waveOutEvent.Volume = value;
            }
        }

        public void AddToPlaylist(FlacReader flacReader)
        {
            _playlist.AddLast(flacReader);

            if (_playlist.Count == 1)
            {
                Load(_playlist.First);
            }
        }

        public bool CanSkipBack()
        {
            if (_currentPlaylistItem != null)
            {
                return _currentPlaylistItem.Previous != null;
            }

            return false;
        }

        public bool CanSkipForward()
        {
            if (_currentPlaylistItem != null)
            {
                return _currentPlaylistItem.Next != null;
            }

            return false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Pause()
        {
            if (_waveOutEvent == null)
            {
                return;
            }

            var oldState = _waveOutEvent.PlaybackState;

            _waveOutEvent.Pause();

            OnPlaybackStateChanged(new PlaybackStateChangedEventArgs(oldState, PlaybackState.Paused));
        }

        public void Play()
        {
            while (!_isInitialized)
            {

            }

            var oldPlaybackState = PlaybackState;

            if (!_playbackTimer.IsEnabled)
            {
                _playbackTimer.Start();
            }

            _waveOutEvent.Play();

            OnPlaybackStateChanged(new PlaybackStateChangedEventArgs(oldPlaybackState, PlaybackState.Playing));

        }

        public void SkipBack()
        {
            if (_currentPlaylistItem.Previous != null)
            {
                Stop();
                Load(_currentPlaylistItem.Previous);
                Play();
            }
        }

        public void SkipForward()
        {
            if (_currentPlaylistItem.Next != null)
            {
                Stop();
                Load(_currentPlaylistItem.Next);
                Play();
            }
        }

        public void Stop()
        {
            if (_waveOutEvent == null || _currentPlaylistItem == null || _currentPlaylistItem.Value == null)
            {
                return;
            }

            var oldState = _waveOutEvent.PlaybackState;

            _playbackTimer?.Stop();
            _waveOutEvent.Stop();
            
            _currentPlaylistItem.Value.Seek(0, SeekOrigin.Begin);

            OnPlaybackStateChanged(new PlaybackStateChangedEventArgs(oldState, PlaybackState.Stopped));
            OnCurrentTimeChanged(new TimeChangedEventArgs(_currentTime, _currentTime = _currentPlaylistItem.Value.CurrentTime));
        }

        public void ToggleMute()
        {
            if (_waveOutEvent.Volume > 0)
            {
                _volume = _waveOutEvent.Volume;

                _waveOutEvent.Volume = 0;
            }
            else
            {
                _waveOutEvent.Volume = _volume;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                _waveOutEvent.Dispose();
                _currentPlaylistItem = null;

                foreach (var playlistItem in _playlist)
                {
                    playlistItem.Dispose();
                }

                _disposedValue = true;
            }
        }

        protected virtual void OnCurrentTimeChanged(TimeChangedEventArgs e)
        {
            if (CurrentTimeChanged != null)
            {
                CurrentTimeChanged(this, e);
            }
        }

        protected virtual void OnPlaybackStateChanged(PlaybackStateChangedEventArgs e)
        {
            if (PlaybackStateChanged != null)
            {
                Application.Current.Dispatcher.Invoke(() => PlaybackStateChanged(this, e));
            }
        }

        protected virtual void OnTotalTimeChanged(TimeChangedEventArgs e)
        {
            if (TotalTimeChanged != null)
            {
                Application.Current.Dispatcher.Invoke(() => TotalTimeChanged(this, e));
            }
        }

        protected virtual void OnTrackChanged(TrackChangedEventArgs e)
        {
            if (TrackChanged != null)
            {
                Application.Current.Dispatcher.Invoke(() => TrackChanged(this, e));
            }
        }

        private void Load(LinkedListNode<FlacReader> playlistItem)
        {
            if (_audioProcessingThread != null)
            {
                Stop();
            }

            _isInitialized = false;

            var oldTrack = _currentPlaylistItem?.Value;

            _currentPlaylistItem = playlistItem;

            StartAudioProcessingThread(playlistItem.Value);

            OnTrackChanged(new TrackChangedEventArgs(oldTrack, _currentPlaylistItem.Value));
        }

        private void StartAudioProcessingThread(FlacReader value)
        {
            _audioProcessingThread = new Thread(new ThreadStart(InitializeAudioProccessor));
            _audioProcessingThread.Name = "AudioProcessingThread";
            _audioProcessingThread.IsBackground = true;
            _audioProcessingThread.Priority = ThreadPriority.Highest;
            _audioProcessingThread.SetApartmentState(ApartmentState.MTA);
            _audioProcessingThread.Start();
        }

        private void InitializeAudioProccessor()
        {
            if (_waveOutEvent != null)
            {
                _waveOutEvent.Dispose();
            }

            _waveOutEvent = new WaveOutEvent();
            _waveOutEvent.Volume = 1f;
            _waveOutEvent.Init(_currentPlaylistItem.Value);

            _isInitialized = true;

            OnTotalTimeChanged(new TimeChangedEventArgs(_totalTime, _totalTime = _currentPlaylistItem.Value.TotalTime));
        }

        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            if (_waveOutEvent.PlaybackState == PlaybackState.Stopped)
            {
                if (CanSkipForward())
                {
                    SkipForward();
                }
                else
                {
                    _playbackTimer.Stop();
                    Stop();
                }

                return;
            }

            OnCurrentTimeChanged(new TimeChangedEventArgs(_currentTime, _currentTime = _currentPlaylistItem.Value.CurrentTime));
        }
    }
}