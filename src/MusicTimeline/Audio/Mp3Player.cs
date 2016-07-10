using NathanHarrenstein.MusicTimeline.Generic;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using NAudio.Wave;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class Mp3Player : IDisposable, IAudioSessionEventsHandler
    {
        private AudioSessionControl audioSessionControl;
        private bool isDisposed;
        private MMDevice playbackDevice;
        private bool playbackSubscribed;
        private DispatcherTimer playbackTimer;
        private WaveOutEvent player;

        public Mp3Player()
        {
            Playlist = new Mp3Playlist();
            Playlist.CurrentItemChanged += Playlist_CurrentItemChanged;
            Playlist.ItemAdded += Playlist_ItemAdded;
            Playlist.ItemRemoved += Playlist_ItemRemoved;

            playbackTimer = new DispatcherTimer();
            playbackTimer.Interval = new TimeSpan(1);
            playbackTimer.Tick += PlaybackTimer_Tick;

            playbackDevice = GetPlaybackDevice();
            playbackDevice.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
            playbackDevice.AudioSessionManager.OnSessionCreated += AudioSessionManager_OnSessionCreated;

            IsMuted = playbackDevice.AudioEndpointVolume.Mute;
        }

        ~Mp3Player()
        {
            Dispose(false);
        }

        public event EventHandler<CanPlayEventArgs> CanPlayChanged;
        public event EventHandler<CanPlayNextEventArgs> CanPlayNextChanged;
        public event EventHandler<CanPlayPreviousEventArgs> CanPlayPreviousChanged;
        public event EventHandler<TimeSpanEventArgs> CurrentTimeChanged;
        public event EventHandler<MuteEventArgs> IsMutedChanged;
        public event EventHandler<PlaybackStateEventArgs> PlaybackStateChanged;
        public event EventHandler<TimeSpanEventArgs> TotalTimeChanged;
        public event EventHandler<TrackEventArgs> TrackChanged;
        public event EventHandler<VolumeEventArgs> VolumeChanged;
        public bool CanPlay { get; private set; }
        public bool CanPlayNext { get; set; }
        public bool CanPlayPrevious { get; set; }

        public TimeSpan CurrentTime
        {
            get
            {
                if (Playlist.CurrentItem == null)
                {
                    throw new InvalidOperationException("The current playlist item is null.");
                }

                if (Playlist.CurrentItem.Stream == null)
                {
                    throw new InvalidOperationException("Could not load playlist item because it contains a null stream.");
                }

                return Playlist.CurrentItem.Stream.CurrentTime;
            }

            set
            {
                if (Playlist.CurrentItem == null)
                {
                    throw new InvalidOperationException("The current playlist item is null.");
                }

                if (Playlist.CurrentItem.Stream == null)
                {
                    throw new InvalidOperationException("Could not load playlist item because it contains a null stream.");
                }

                Playlist.CurrentItem.Stream.CurrentTime = value;
            }
        }

        public bool IsMuted { get; private set; }

        public PlaybackState PlaybackState
        {
            get
            {
                if (player == null)
                {
                    throw new InvalidOperationException("The playback state cannot be retrieved until a track has been loaded.");
                }

                return player.PlaybackState;
            }
        }

        public Mp3Playlist Playlist { get; private set; }

        public TimeSpan TotalTime
        {
            get
            {
                if (Playlist.CurrentItem == null)
                {
                    throw new InvalidOperationException("The current playlist item is null.");
                }

                if (Playlist.CurrentItem.Stream == null)
                {
                    throw new InvalidOperationException("Could not load playlist item because it contains a null stream.");
                }

                return Playlist.CurrentItem.Stream.TotalTime;
            }
        }

        public float Volume
        {
            get
            {
                if (audioSessionControl != null)
                {
                    return audioSessionControl.SimpleAudioVolume.Volume;
                }
                else
                {
                    throw new InvalidOperationException("The volume cannot be retrieved because _audioSessionControl is null.");
                }
            }

            set
            {
                if (audioSessionControl != null)
                {
                    audioSessionControl.SimpleAudioVolume.Volume = value;
                }
            }
        }

        public void AddToPlaylist(Mp3PlaylistItem mp3PlaylistItem)
        {
            Playlist.Add(mp3PlaylistItem);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public void Next()
        {
            Playlist.Next();
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
            Application.Current.Dispatcher.Invoke(() => OnVolumeChanged(new VolumeEventArgs(audioSessionControl.SimpleAudioVolume.Volume)));

            if (IsMuted != isMuted)
            {
                IsMuted = isMuted;

                Application.Current.Dispatcher.Invoke(() => OnIsMutedChanged(new MuteEventArgs(isMuted)));
            }
        }

        public void Pause()
        {
            if (player == null)
            {
                return;
            }

            player.Pause();

            Application.Current.Dispatcher.Invoke(() => OnPlaybackStateChanged(new PlaybackStateEventArgs(player.PlaybackState)));
        }

        public void Play()
        {
            if (CanPlay)
            {
                InternalPlay();
            }
            else
            {
                if (!playbackSubscribed)
                {
                    CanPlayChanged += Mp3Player_CanPlayChanged;

                    playbackSubscribed = true;
                }
            }
        }

        public void Previous()
        {
            Playlist.Previous();
        }

        public void Stop()
        {
            if (player == null || Playlist.CurrentItem == null || Playlist.CurrentItem.Stream == null || PlaybackState == PlaybackState.Stopped)
            {
                return;
            }

            playbackTimer?.Stop();
            player.Stop();

            Playlist.CurrentItem.Stream.Seek(0, SeekOrigin.Begin);

            Application.Current.Dispatcher.Invoke(() => OnPlaybackStateChanged(new PlaybackStateEventArgs(player.PlaybackState)));
            Application.Current.Dispatcher.Invoke(() => OnCurrentTimeChanged(new TimeSpanEventArgs(Playlist.CurrentItem.Stream.CurrentTime)));
        }

        public void ToggleMute()
        {
            if (audioSessionControl == null)
            {
                return;
            }

            if (IsMuted && playbackDevice.AudioEndpointVolume.Mute)
            {
                playbackDevice.AudioEndpointVolume.Mute = false;
            }

            audioSessionControl.SimpleAudioVolume.Mute = !IsMuted;
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
            if (!isDisposed)
            {
                if (player != null)
                {
                    Stop();

                    player.Dispose();
                }

                Playlist.Dispose();

                isDisposed = true;
            }
        }

        protected virtual void OnCanPlayChanged(CanPlayEventArgs e)
        {
            if (CanPlayChanged != null)
            {
                CanPlayChanged(this, e);
            }
        }

        protected virtual void OnCanPlayNextChanged(CanPlayNextEventArgs e)
        {
            if (CanPlayNextChanged != null)
            {
                CanPlayNextChanged(this, e);
            }
        }

        protected virtual void OnCanPlayPreviousChanged(CanPlayPreviousEventArgs e)
        {
            if (CanPlayPreviousChanged != null)
            {
                CanPlayPreviousChanged(this, e);
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
            IsMuted = data.Muted;

            Application.Current.Dispatcher.Invoke(() => OnIsMutedChanged(new MuteEventArgs(IsMuted)));
        }

        private void AudioSessionManager_OnSessionCreated(object sender, IAudioSessionControl newSession)
        {
            var audioSessionControl = new AudioSessionControl(newSession);

            if (this.audioSessionControl == null && audioSessionControl.GetProcessID == Process.GetCurrentProcess().Id)
            {
                this.audioSessionControl = audioSessionControl;
                this.audioSessionControl.RegisterEventClient(this);

                if (this.audioSessionControl.SimpleAudioVolume.Mute)
                {
                    IsMuted = true;
                }

                Application.Current.Dispatcher.Invoke(() => OnVolumeChanged(new VolumeEventArgs(audioSessionControl.SimpleAudioVolume.Volume)));
                Application.Current.Dispatcher.Invoke(() => OnIsMutedChanged(new MuteEventArgs(IsMuted)));
            }
        }

        private void InitializePlayer()
        {
            player = new WaveOutEvent();
            player.Init(Playlist.CurrentItem.Stream);

            CanPlay = true;

            Application.Current.Dispatcher.Invoke(() => OnCanPlayChanged(new CanPlayEventArgs(CanPlay)));
            Application.Current.Dispatcher.Invoke(() => OnTotalTimeChanged(new TimeSpanEventArgs(Playlist.CurrentItem.Stream.TotalTime)));
        }

        private void InternalPlay()
        {
            if (!playbackTimer.IsEnabled)
            {
                playbackTimer.Start();
            }

            player.Play();

            Application.Current.Dispatcher.Invoke(() => OnPlaybackStateChanged(new PlaybackStateEventArgs(player.PlaybackState)));
        }

        private void Load()
        {
            if (Playlist.CurrentItem == null)
            {
                throw new InvalidOperationException("The current playlist item is null.");
            }

            if (Playlist.CurrentItem.Stream == null)
            {
                throw new InvalidOperationException("Could not load playlist item because it contains a null stream.");
            }

            if (player != null && PlaybackState != PlaybackState.Stopped)
            {
                Stop();
                player.Dispose();
            }

            CanPlay = false;

            App.Current.Dispatcher.Invoke(new Action(() => OnCanPlayPreviousChanged(new CanPlayPreviousEventArgs(Playlist.HasPrevious()))));
            App.Current.Dispatcher.Invoke(new Action(() => OnCanPlayNextChanged(new CanPlayNextEventArgs(Playlist.HasNext()))));

            StartPlaybackThread();

            Application.Current.Dispatcher.Invoke(() => OnTrackChanged(new TrackEventArgs(Playlist.CurrentItem.Stream)));
        }

        private void Mp3Player_CanPlayChanged(object sender, CanPlayEventArgs e)
        {
            if (e.CanPlay)
            {
                InternalPlay();

                CanPlayChanged -= Mp3Player_CanPlayChanged;

                playbackSubscribed = false;
            }
        }

        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            if (player.PlaybackState == PlaybackState.Stopped)
            {
                if (Playlist.Next())
                {
                    player.Init(Playlist.CurrentItem.Stream);
                    Play();

                    Application.Current.Dispatcher.Invoke(() => OnTotalTimeChanged(new TimeSpanEventArgs(Playlist.CurrentItem.Stream.TotalTime)));
                    App.Current.Dispatcher.Invoke(new Action(() => OnCanPlayPreviousChanged(new CanPlayPreviousEventArgs(Playlist.HasPrevious()))));
                    App.Current.Dispatcher.Invoke(new Action(() => OnCanPlayNextChanged(new CanPlayNextEventArgs(Playlist.HasNext()))));
                }
                else
                {
                    Stop();
                }

                return;
            }

            Application.Current.Dispatcher.Invoke(() => OnCurrentTimeChanged(new TimeSpanEventArgs(Playlist.CurrentItem.Stream.CurrentTime)));
        }

        private void Playlist_CurrentItemChanged(object sender, ItemChangedEventArgs<Mp3PlaylistItem> e)
        {
            Load();

            var canPlayPrevious = Playlist.HasPrevious();

            if (canPlayPrevious != CanPlayPrevious)
            {
                CanPlayPrevious = canPlayPrevious;

                App.Current.Dispatcher.Invoke(new Action(() => OnCanPlayPreviousChanged(new CanPlayPreviousEventArgs(CanPlayPrevious))));
            }

            var canPlayNext = Playlist.HasNext();

            if (canPlayNext != CanPlayNext)
            {
                CanPlayNext = canPlayNext;

                App.Current.Dispatcher.Invoke(new Action(() => OnCanPlayNextChanged(new CanPlayNextEventArgs(CanPlayNext))));
            }
        }

        private void Playlist_ItemAdded(object sender, ItemAddedEventArgs<Mp3PlaylistItem> e)
        {
            var canPlayPrevious = Playlist.HasPrevious();

            if (canPlayPrevious != CanPlayPrevious)
            {
                CanPlayPrevious = canPlayPrevious;

                App.Current.Dispatcher.Invoke(new Action(() => OnCanPlayPreviousChanged(new CanPlayPreviousEventArgs(CanPlayPrevious))));
            }

            var canPlayNext = Playlist.HasNext();

            if (canPlayNext != CanPlayNext)
            {
                CanPlayNext = canPlayNext;

                App.Current.Dispatcher.Invoke(new Action(() => OnCanPlayNextChanged(new CanPlayNextEventArgs(CanPlayNext))));
            }
        }

        private void Playlist_ItemRemoved(object sender, ItemRemovedEventArgs<Mp3PlaylistItem> e)
        {
            var canPlayPrevious = Playlist.HasPrevious();

            if (canPlayPrevious != CanPlayPrevious)
            {
                CanPlayPrevious = canPlayPrevious;

                App.Current.Dispatcher.Invoke(new Action(() => OnCanPlayPreviousChanged(new CanPlayPreviousEventArgs(CanPlayPrevious))));
            }

            var canPlayNext = Playlist.HasNext();

            if (canPlayNext != CanPlayNext)
            {
                CanPlayNext = canPlayNext;

                App.Current.Dispatcher.Invoke(new Action(() => OnCanPlayNextChanged(new CanPlayNextEventArgs(CanPlayNext))));
            }
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
    }
}