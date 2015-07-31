using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using NAudio.Flac;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Threading;

namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class AudioPlayer : IDisposable, IAudioSessionEventsHandler
    {
        private readonly LinkedList<FlacReader> _playlist;
        private AudioSessionControl _audioSessionControl;
        private LinkedListNode<FlacReader> _currentPlaylistItem;
        private bool _disposed;
        private EventWaitHandle _initializationWaitHandle;
        private MMDevice _playbackDevice;
        private DispatcherTimer _playbackTimer;
        private WaveOutEvent _waveOutEvent;
        private Thread _waveOutEventThread;

        public AudioPlayer()
        {
            _playlist = new LinkedList<FlacReader>();
            _playbackTimer = new DispatcherTimer();
            _playbackTimer.Interval = new TimeSpan(1);
            _playbackTimer.Tick += PlaybackTimer_Tick;

            _playbackDevice = GetPlaybackDevice();
            _playbackDevice.AudioSessionManager.OnSessionCreated += AudioSessionManager_OnSessionCreated;
        }

        ~AudioPlayer()
        {
            Dispose(false);
        }

        public event EventHandler<TimeSpanEventArgs> CurrentTimeChanged;

        public event EventHandler<MuteEventArgs> MuteChanged;

        public event EventHandler<PlaybackStateEventArgs> PlaybackStateChanged;

        public event EventHandler<TimeSpanEventArgs> TotalTimeChanged;

        public event EventHandler<TrackEventArgs> TrackChanged;

        public event EventHandler<VolumeEventArgs> VolumeChanged;

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

        public bool Mute
        {
            get
            {
                if (_audioSessionControl != null)
                {
                    return _audioSessionControl.SimpleAudioVolume.Mute;
                }
                else
                {
                    throw new InvalidOperationException("Mute cannot be retrieved because _audioSessionControl is null.");
                }
            }

            set
            {
                if (_audioSessionControl != null)
                {
                    _audioSessionControl.SimpleAudioVolume.Mute = value;
                }
                else
                {
                    throw new InvalidOperationException("Mute cannot be retrieved because _audioSessionControl is null.");
                }
            }
        }

        public PlaybackState PlaybackState
        {
            get
            {
                if (_waveOutEvent == null)
                {
                    throw new InvalidOperationException("The playback state cannot be retrieved until a track has been loaded.");
                }

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
                else
                {
                    throw new InvalidOperationException("The volume cannot be set because _audioSessionControl is null.");
                }
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
            UpdateMute();
        }

        public void Pause()
        {
            if (_waveOutEvent == null)
            {
                return;
            }

            _waveOutEvent.Pause();
            UpdatePlaybackState();
        }

        public void Play()
        {
            _initializationWaitHandle.WaitOne();

            if (!_playbackTimer.IsEnabled)
            {
                _playbackTimer.Start();
            }

            _waveOutEvent.Play();
            UpdatePlaybackState();
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

            _playbackTimer?.Stop();
            _waveOutEvent.Stop();

            _currentPlaylistItem.Value.Seek(0, SeekOrigin.Begin);

            UpdatePlaybackState();
            UpdateCurrentTime();
        }

        internal MMDevice GetPlaybackDevice()
        {
            try
            {
                MMDeviceEnumerator multimediaDeviceEnumerator = new MMDeviceEnumerator();

                return multimediaDeviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_waveOutEvent != null)
                {
                    _waveOutEvent.Dispose();
                }

                _currentPlaylistItem = null;

                foreach (var playlistItem in _playlist)
                {
                    playlistItem.Dispose();
                }

                _disposed = true;
            }
        }

        protected virtual void OnCurrentTimeChanged(TimeSpanEventArgs e)
        {
            if (CurrentTimeChanged != null)
            {
                CurrentTimeChanged(this, e);
            }
        }

        protected virtual void OnMuteChanged(MuteEventArgs e)
        {
            if (MuteChanged != null)
            {
                MuteChanged(this, e);
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
                System.Windows.Application.Current.Dispatcher.Invoke(() => TotalTimeChanged(this, e));
            }
        }

        protected virtual void OnTrackChanged(TrackEventArgs e)
        {
            if (TrackChanged != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() => TrackChanged(this, e));
            }
        }

        protected virtual void OnVolumeChanged(VolumeEventArgs e)
        {
            if (VolumeChanged != null)
            {
                VolumeChanged(this, e);
            }
        }

        private void AudioSessionManager_OnSessionCreated(object sender, IAudioSessionControl newSession)
        {
            var audioSessionControl = new AudioSessionControl(newSession);

            if (_audioSessionControl == null && audioSessionControl.GetProcessID == Process.GetCurrentProcess().Id)
            {
                _audioSessionControl = audioSessionControl;
                _audioSessionControl.RegisterEventClient(this);

                UpdateVolume();
                UpdateMute();
            }
        }

        private void InitializeWaveOut()
        {
            if (_waveOutEvent != null)
            {
                _waveOutEvent.Dispose();
            }

            _waveOutEvent = new WaveOutEvent();
            _waveOutEvent.Init(_currentPlaylistItem.Value);

            _initializationWaitHandle.Set();

            UpdateTotalTime();
        }

        private void Load(LinkedListNode<FlacReader> playlistItem)
        {
            if (_waveOutEventThread != null)
            {
                Stop();
            }

            _initializationWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            _currentPlaylistItem = playlistItem;

            StartWaveOutThread();
            UpdateTrackChanged();
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

            UpdateCurrentTime();
        }

        private void StartWaveOutThread()
        {
            _waveOutEventThread = new Thread(new ThreadStart(InitializeWaveOut));
            _waveOutEventThread.IsBackground = true;
            _waveOutEventThread.Priority = ThreadPriority.Highest;
            _waveOutEventThread.SetApartmentState(ApartmentState.MTA);
            _waveOutEventThread.Start();
        }

        private void UpdateCurrentTime()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() => OnCurrentTimeChanged(new TimeSpanEventArgs(_currentPlaylistItem.Value.CurrentTime)));
        }

        private void UpdateMute()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() => OnMuteChanged(new MuteEventArgs(_audioSessionControl.SimpleAudioVolume.Mute)));
        }

        private void UpdatePlaybackState()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() => OnPlaybackStateChanged(new PlaybackStateEventArgs(_waveOutEvent.PlaybackState)));
        }

        private void UpdateTotalTime()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() => OnTotalTimeChanged(new TimeSpanEventArgs(_currentPlaylistItem.Value.TotalTime)));
        }

        private void UpdateTrackChanged()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() => OnTrackChanged(new TrackEventArgs(_currentPlaylistItem.Value)));
        }

        private void UpdateVolume()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() => OnVolumeChanged(new VolumeEventArgs(_audioSessionControl.SimpleAudioVolume.Volume)));
        }
    }
}