using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class Mp3Player : IDisposable, IAudioSessionEventsHandler
    {
        private readonly LinkedList<Mp3FileReader> _playlist;
        private AudioSessionControl _audioSessionControl;
        private bool _canPlay;
        private bool _canSkipBack;
        private bool _canSkipForward;
        private LinkedListNode<Mp3FileReader> _currentPlaylistItem;
        private bool _isDisposed;
        private bool _isMuted;
        private MMDevice _playbackDevice;
        private bool _playbackSubscribed;
        private DispatcherTimer _playbackTimer;
        private WaveOutEvent _player;

        public Mp3Player()
        {
            _playlist = new LinkedList<Mp3FileReader>();
            _playbackTimer = new DispatcherTimer();
            _playbackTimer.Interval = new TimeSpan(1);
            _playbackTimer.Tick += PlaybackTimer_Tick;

            _playbackDevice = GetPlaybackDevice();
            _playbackDevice.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
            _playbackDevice.AudioSessionManager.OnSessionCreated += AudioSessionManager_OnSessionCreated;

            _isMuted = _playbackDevice.AudioEndpointVolume.Mute;
        }

        ~Mp3Player()
        {
            Dispose(false);
        }

        public event EventHandler<CanPlayEventArgs> CanPlayChanged;

        public event EventHandler<CanSkipBackEventArgs> CanSkipBackChanged;

        public event EventHandler<CanSkipForwardEventArgs> CanSkipForwardChanged;

        public event EventHandler<TimeSpanEventArgs> CurrentTimeChanged;

        public event EventHandler<MuteEventArgs> IsMutedChanged;

        public event EventHandler<PlaybackStateEventArgs> PlaybackStateChanged;

        public event EventHandler<TimeSpanEventArgs> TotalTimeChanged;

        public event EventHandler<TrackEventArgs> TrackChanged;

        public event EventHandler<VolumeEventArgs> VolumeChanged;

        public bool CanPlay
        {
            get
            {
                return _canPlay;
            }
        }

        public bool CanSkipBack
        {
            get
            {
                return _canSkipBack;
            }

            set
            {
                _canSkipBack = value;
            }
        }

        public bool CanSkipForward
        {
            get
            {
                return _canSkipForward;
            }

            set
            {
                _canSkipForward = value;
            }
        }

        public LinkedListNode<Mp3FileReader> CurrentPlaylistItem
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

        public TimeSpan CurrentTime
        {
            get
            {
                if (_currentPlaylistItem == null || _currentPlaylistItem.Value == null)
                {
                    throw new InvalidOperationException("The current time cannot be retrieved until a track has been loaded.");
                }

                return _currentPlaylistItem.Value.CurrentTime;
            }

            set
            {
                if (_currentPlaylistItem == null || _currentPlaylistItem.Value == null)
                {
                    throw new InvalidOperationException("The current time cannot be set until a track has been loaded.");
                }

                _currentPlaylistItem.Value.CurrentTime = value;
            }
        }

        public bool IsMuted
        {
            get
            {
                return _isMuted;
            }
        }

        public PlaybackState PlaybackState
        {
            get
            {
                if (_player == null)
                {
                    throw new InvalidOperationException("The playback state cannot be retrieved until a track has been loaded.");
                }

                return _player.PlaybackState;
            }
        }

        public LinkedList<Mp3FileReader> Playlist
        {
            get
            {
                return _playlist;
            }
        }

        public TimeSpan TotalTime
        {
            get
            {
                if (_currentPlaylistItem == null || _currentPlaylistItem.Value == null)
                {
                    throw new InvalidOperationException("The total time cannot be retrieved until a track has been loaded.");
                }

                return _currentPlaylistItem.Value.TotalTime;
            }
        }

        public float Volume
        {
            get
            {
                if (_audioSessionControl != null)
                {
                    return _audioSessionControl.SimpleAudioVolume.Volume;
                }
                else
                {
                    throw new InvalidOperationException("The volume cannot be retrieved because _audioSessionControl is null.");
                }
            }

            set
            {
                if (_audioSessionControl != null)
                {
                    _audioSessionControl.SimpleAudioVolume.Volume = value;
                }
            }
        }

        public void AddToPlaylist(Mp3FileReader mp3FileReader)
        {
            _playlist.AddLast(mp3FileReader);

            if (_playlist.Count == 1)
            {
                Load(_playlist.First);
            }

            _canSkipForward = _currentPlaylistItem.Next != null;

            App.Current.Dispatcher.Invoke(new Action(() => OnCanSkipForwardChanged(new CanSkipForwardEventArgs(_canSkipForward))));
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public void OnChannelVolumeChanged(uint channelCount, IntPtr newVolumes, uint channelIndex)
        {
        }

        public void OnDisplayNameChanged(string displayName)
        {
        }

        public void OnGroupingParamChanged(ref Guid groupingId)
        {
        }

        public void OnIconPathChanged(string iconPath)
        {
        }

        public void OnSessionDisconnected(AudioSessionDisconnectReason disconnectReason)
        {
        }

        public void OnStateChanged(AudioSessionState state)
        {
        }

        public void OnVolumeChanged(float volume, bool isMuted)
        {
            UpdateVolume();

            _isMuted = isMuted;

            UpdateIsMuted();
        }

        public void Pause()
        {
            if (_player == null)
            {
                return;
            }

            _player.Pause();
            UpdatePlaybackState();
        }

        public void Play()
        {
            if (_canPlay)
            {
                InternalPlay();
            }
            else
            {
                if (!_playbackSubscribed)
                {
                    CanPlayChanged += FlacPlayer_CanPlayChanged_StartPlayback;
                }
            }
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
            if (_player == null || _currentPlaylistItem == null || _currentPlaylistItem.Value == null)
            {
                return;
            }

            _playbackTimer?.Stop();
            _player.Stop();

            _currentPlaylistItem.Value.Seek(0, SeekOrigin.Begin);

            UpdatePlaybackState();
            UpdateCurrentTime();
        }

        public void ToggleMute()
        {
            if (_audioSessionControl == null)
            {
                return;
            }

            if (_isMuted && _playbackDevice.AudioEndpointVolume.Mute)
            {
                _playbackDevice.AudioEndpointVolume.Mute = false;
            }

            _audioSessionControl.SimpleAudioVolume.Mute = !_isMuted;
        }

        internal MMDevice GetPlaybackDevice()
        {
            try
            {
                return new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (_player != null)
                {
                    if (_player.PlaybackState != PlaybackState.Stopped)
                    {
                        Stop();
                    }

                    _player.Dispose();
                }

                foreach (var playlistItem in _playlist)
                {
                    playlistItem.Dispose();
                }

                _isDisposed = true;
            }
        }

        protected virtual void OnCanPlayChanged(CanPlayEventArgs e)
        {
            if (CanPlayChanged != null)
            {
                CanPlayChanged(this, e);
            }
        }

        protected virtual void OnCanSkipBackChanged(CanSkipBackEventArgs e)
        {
            if (CanSkipBackChanged != null)
            {
                CanSkipBackChanged(this, e);
            }
        }

        protected virtual void OnCanSkipForwardChanged(CanSkipForwardEventArgs e)
        {
            if (CanSkipForwardChanged != null)
            {
                CanSkipForwardChanged(this, e);
            }
        }

        protected virtual void OnCurrentTimeChanged(TimeSpanEventArgs e)
        {
            if (CurrentTimeChanged != null)
            {
                CurrentTimeChanged(this, e);
            }
        }

        protected virtual void OnIsMutedChanged(MuteEventArgs e)
        {
            if (IsMutedChanged != null)
            {
                IsMutedChanged(this, e);
            }
        }

        protected virtual void OnPlaybackStateChanged(PlaybackStateEventArgs e)
        {
            if (PlaybackStateChanged != null)
            {
                PlaybackStateChanged(this, e);
            }
        }

        protected virtual void OnTotalTimeChanged(TimeSpanEventArgs e)
        {
            if (TotalTimeChanged != null)
            {
                Application.Current.Dispatcher.Invoke(() => TotalTimeChanged(this, e));
            }
        }

        protected virtual void OnTrackChanged(TrackEventArgs e)
        {
            if (TrackChanged != null)
            {
                Application.Current.Dispatcher.Invoke(() => TrackChanged(this, e));
            }
        }

        protected virtual void OnVolumeChanged(VolumeEventArgs e)
        {
            if (VolumeChanged != null)
            {
                VolumeChanged(this, e);
            }
        }

        private void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data)
        {
            _isMuted = data.Muted;

            UpdateIsMuted();
        }

        private void AudioSessionManager_OnSessionCreated(object sender, IAudioSessionControl newSession)
        {
            var audioSessionControl = new AudioSessionControl(newSession);

            if (_audioSessionControl == null && audioSessionControl.GetProcessID == Process.GetCurrentProcess().Id)
            {
                _audioSessionControl = audioSessionControl;
                _audioSessionControl.RegisterEventClient(this);

                if (_audioSessionControl.SimpleAudioVolume.Mute)
                {
                    _isMuted = true;
                }

                UpdateVolume();
                UpdateIsMuted();
            }
        }

        private void FlacPlayer_CanPlayChanged_StartPlayback(object sender, CanPlayEventArgs e)
        {
            if (e.CanPlay)
            {
                InternalPlay();

                CanPlayChanged -= FlacPlayer_CanPlayChanged_StartPlayback;
                _playbackSubscribed = false;
            }
        }

        private void InitializePlayer()
        {
            if (_player != null)
            {
                _player.Dispose();
            }

            _player = new WaveOutEvent();
            _player.Init(_currentPlaylistItem.Value);

            _canPlay = true;

            Application.Current.Dispatcher.Invoke(() => OnCanPlayChanged(new CanPlayEventArgs(_canPlay)));

            UpdateTotalTime();
        }

        private void InternalPlay()
        {
            if (!_playbackTimer.IsEnabled)
            {
                _playbackTimer.Start();
            }

            _player.Play();

            UpdatePlaybackState();
        }

        private void Load(LinkedListNode<Mp3FileReader> playlistItem)
        {
            if (playlistItem == null)
            {
                throw new ArgumentNullException(nameof(playlistItem));
            }

            if (_player != null && PlaybackState != PlaybackState.Stopped)
            {
                Stop();
            }

            _canPlay = false;
            _currentPlaylistItem = playlistItem;

            _canSkipBack = _currentPlaylistItem.Previous != null;
            _canSkipForward = _currentPlaylistItem.Next != null;

            App.Current.Dispatcher.Invoke(new Action(() => OnCanSkipBackChanged(new CanSkipBackEventArgs(_canSkipBack))));
            App.Current.Dispatcher.Invoke(new Action(() => OnCanSkipForwardChanged(new CanSkipForwardEventArgs(_canSkipForward))));

            StartPlaybackThread();
            UpdateTrackChanged();
        }

        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            if (_player.PlaybackState == PlaybackState.Stopped)
            {
                if (_canSkipForward)
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

            UpdateCurrentTime();
        }

        private void StartPlaybackThread()
        {
            var playbackThread = new Thread(new ThreadStart(InitializePlayer));
            playbackThread.Name = "Playback Thread";
            playbackThread.IsBackground = true;
            playbackThread.Priority = ThreadPriority.Highest;
            playbackThread.SetApartmentState(ApartmentState.MTA);
            playbackThread.Start();
        }

        private void UpdateCurrentTime()
        {
            Application.Current.Dispatcher.Invoke(() => OnCurrentTimeChanged(new TimeSpanEventArgs(_currentPlaylistItem.Value.CurrentTime)));
        }

        private void UpdateIsMuted()
        {
            Application.Current.Dispatcher.Invoke(() => OnIsMutedChanged(new MuteEventArgs(_isMuted)));
        }

        private void UpdatePlaybackState()
        {
            Application.Current.Dispatcher.Invoke(() => OnPlaybackStateChanged(new PlaybackStateEventArgs(_player.PlaybackState)));
        }

        private void UpdateTotalTime()
        {
            Application.Current.Dispatcher.Invoke(() => OnTotalTimeChanged(new TimeSpanEventArgs(_currentPlaylistItem.Value.TotalTime)));
        }

        private void UpdateTrackChanged()
        {
            Application.Current.Dispatcher.Invoke(() => OnTrackChanged(new TrackEventArgs(_currentPlaylistItem.Value)));
        }

        private void UpdateVolume()
        {
            Application.Current.Dispatcher.Invoke(() => OnVolumeChanged(new VolumeEventArgs(_audioSessionControl.SimpleAudioVolume.Volume)));
        }
    }
}