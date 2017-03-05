using NathanHarrenstein.MusicTimeline.Generic;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using NAudio.Wave;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace NathanHarrenstein.MusicTimeline.Audio
{
    public class Mp3StreamPlayer : IAudioSessionEventsHandler
    {
        private AudioSessionControl audioSessionControl;
        private BufferedWaveStream bufferedWaveStream;
        private TimeSpan currentTime;
        private volatile bool isDownloaded;
        private MMDevice playbackDevice;
        private volatile StreamingPlaybackState playbackState;
        private DispatcherTimer playbackTimer;
        private Playlist playlist;
        private HttpWebRequest request;
        private long totalBytes;
        private TimeSpan totalTime;
        private float volume;
        private IWavePlayer waveOut;

        public Mp3StreamPlayer()
        {
            Playlist = new Playlist();
            Playlist.CurrentItemChanged += Playlist_CurrentItemChanged;
            Playlist.ItemAdded += Playlist_ItemAdded;
            Playlist.ItemRemoved += Playlist_ItemRemoved;

            playbackTimer = new DispatcherTimer();
            playbackTimer.Interval = TimeSpan.FromMilliseconds(150);
            playbackTimer.Tick += PlaybackTimer_Tick;

            playbackDevice = GetMultimediaPlaybackDevice();
            playbackDevice.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
            playbackDevice.AudioSessionManager.OnSessionCreated += AudioSessionManager_OnSessionCreated;
        }

        public event EventHandler<CanPlayEventArgs> CanPlayChanged;
        public event EventHandler<CanPlayNextEventArgs> CanPlayNextChanged;
        public event EventHandler<CanPlayPreviousEventArgs> CanPlayPreviousChanged;
        public event EventHandler<TimeSpanEventArgs> CurrentTimeChanged;
        public event EventHandler<Exception> ExceptionOccurred;
        public event EventHandler<MuteEventArgs> IsMutedChanged;
        public event EventHandler<PlaybackStateEventArgs> PlaybackStateChanged;
        public event EventHandler<TimeSpanEventArgs> TotalTimeChanged;
        public event EventHandler TrackChanged;
        public event EventHandler<VolumeEventArgs> VolumeChanged;
        public bool CanPlay { get; private set; }
        public bool CanPlayNext { get; set; }
        public bool CanPlayPrevious { get; set; }

        public TimeSpan CurrentTime
        {
            get
            {
                return currentTime;
            }

            set
            {
                currentTime = value;
                bufferedWaveStream.Position = (int)currentTime.TotalSeconds * bufferedWaveStream.WaveFormat.AverageBytesPerSecond;
            }
        }

        public bool IsMuted
        {
            get
            {
                return audioSessionControl.SimpleAudioVolume.Mute;
            }
            set
            {
                audioSessionControl.SimpleAudioVolume.Mute = value;
            }
        }

        public StreamingPlaybackState PlaybackState
        {
            get
            {
                return playbackState;
            }
        }

        public Playlist Playlist
        {
            get
            {
                return playlist;
            }
            set
            {
                playlist = value;
            }
        }

        public TimeSpan TotalTime
        {
            get
            {
                return totalTime;
            }

            set
            {
                totalTime = value;
            }
        }

        public float Volume
        {
            get
            {
                return volume;
            }

            set
            {
                if (volume != value)
                {
                    volume = value;

                    if (audioSessionControl != null)
                    {
                        audioSessionControl.SimpleAudioVolume.Volume = volume;
                    }

                    Application.Current.Dispatcher.Invoke(() => OnVolumeChanged(new VolumeEventArgs(Volume)));
                }
            }
        }

        private bool IsBufferNearlyFull
        {
            get
            {
                return bufferedWaveStream != null && bufferedWaveStream.BufferLength - bufferedWaveStream.BufferedBytes < bufferedWaveStream.WaveFormat.AverageBytesPerSecond / 4;
            }
        }

        public void AddToPlaylist(PlaylistItem mp3PlaylistItem)
        {
            Playlist.Add(mp3PlaylistItem);
        }

        public void Buffer()
        {
            playbackState = StreamingPlaybackState.Buffering;

            waveOut.Pause();

            OnPlaybackStateChanged(new PlaybackStateEventArgs(playbackState));

            Debug.WriteLine(string.Format("Paused to buffer, waveOut.PlaybackState={0}", waveOut.PlaybackState));
        }

        public void Load()
        {
            if (playbackState == StreamingPlaybackState.Stopped)
            {
                playbackState = StreamingPlaybackState.Buffering;

                OnPlaybackStateChanged(new PlaybackStateEventArgs(playbackState));

                bufferedWaveStream = null;
                ThreadPool.QueueUserWorkItem(StartDownloading, null);
                playbackTimer.IsEnabled = true;

                OnTrackChanged(null);
            }
            else if (playbackState == StreamingPlaybackState.Paused)
            {
                playbackState = StreamingPlaybackState.Buffering;

                OnPlaybackStateChanged(new PlaybackStateEventArgs(playbackState));
            }
        }

        public bool Next()
        {
            return Playlist.Next();
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
            playbackState = StreamingPlaybackState.Paused;

            waveOut.Pause();

            OnPlaybackStateChanged(new PlaybackStateEventArgs(playbackState));

            Debug.WriteLine(string.Format("Paused, waveOut.PlaybackState={0}", waveOut.PlaybackState));
        }

        public void Play()
        {
            if (waveOut == null)
            {
                Load();

                return;
            }

            waveOut.Play();

            Debug.WriteLine(string.Format("Started playing, waveOut.PlaybackState={0}", waveOut.PlaybackState));

            playbackState = StreamingPlaybackState.Playing;

            OnPlaybackStateChanged(new PlaybackStateEventArgs(playbackState));
        }

        public bool Previous()
        {
            return Playlist.Previous();
        }

        public void Stop()
        {
            if (playbackState != StreamingPlaybackState.Stopped)
            {
                playbackTimer.Stop();

                if (!isDownloaded)
                {
                    request.Abort();
                }

                totalBytes = 0;

                playbackState = StreamingPlaybackState.Stopped;

                OnPlaybackStateChanged(new PlaybackStateEventArgs(playbackState));

                if (waveOut != null)
                {
                    waveOut.Stop();
                    waveOut.Dispose();
                    waveOut = null;
                }

                // n.b. streaming thread may not yet have exited
                Thread.Sleep(500);
            }
        }

        internal MMDevice GetMultimediaPlaybackDevice()
        {
            try
            {
                var multimediaDeviceEnumerator = new MMDeviceEnumerator();
                return multimediaDeviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            }
            catch (Exception exception)
            {
                OnExceptionOccurred(exception);

                throw exception;
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

        protected virtual void OnExceptionOccurred(Exception exception)
        {
            if (ExceptionOccurred != null)
            {
                ExceptionOccurred(this, exception);
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

        protected virtual void OnTrackChanged(EventArgs e)
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

        private static IMp3FrameDecompressor CreateFrameDecompressor(Mp3Frame frame)
        {
            WaveFormat waveFormat = new Mp3WaveFormat(frame.SampleRate, frame.ChannelMode == ChannelMode.Mono ? 1 : 2, frame.FrameLength, frame.BitRate);

            return new AcmMp3FrameDecompressor(waveFormat);
        }

        private void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data)
        {
            Application.Current.Dispatcher.Invoke(() => OnIsMutedChanged(new MuteEventArgs(data.Muted)));
            Application.Current.Dispatcher.Invoke(() => OnVolumeChanged(new VolumeEventArgs(data.MasterVolume)));
        }

        private void AudioSessionManager_OnSessionCreated(object sender, IAudioSessionControl newSession)
        {
            var newAudioSessionControl = new AudioSessionControl(newSession);

            if (newAudioSessionControl.GetProcessID == Process.GetCurrentProcess().Id)
            {
                audioSessionControl = newAudioSessionControl;
                audioSessionControl.RegisterEventClient(this);

                volume = newAudioSessionControl.SimpleAudioVolume.Volume;

                Application.Current.Dispatcher.Invoke(() => OnIsMutedChanged(new MuteEventArgs(IsMuted)));
                Application.Current.Dispatcher.Invoke(() => OnVolumeChanged(new VolumeEventArgs(Volume)));
            }
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            Debug.WriteLine("Playback Stopped");

            if (e.Exception != null)
            {
                OnExceptionOccurred(e.Exception);
            }
        }

        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            if (playbackState == StreamingPlaybackState.Stopped)
            {
                return;
            }

            if (waveOut == null && bufferedWaveStream != null)
            {
                Debug.WriteLine("Creating WaveOut Device");

                waveOut = new WaveOutEvent();
                waveOut.PlaybackStopped += OnPlaybackStopped;
                waveOut.Init(bufferedWaveStream);
            }
            else if (bufferedWaveStream != null)
            {
                var waveOutEvent = (WaveOutEvent)waveOut;

                currentTime = TimeSpan.FromSeconds(bufferedWaveStream.Position / (double)waveOutEvent.OutputWaveFormat.AverageBytesPerSecond);
                Application.Current.Dispatcher.Invoke(() => OnCurrentTimeChanged(new TimeSpanEventArgs(currentTime)));

                totalTime = TimeSpan.FromSeconds(totalBytes / (double)waveOutEvent.OutputWaveFormat.AverageBytesPerSecond);
                Application.Current.Dispatcher.Invoke(() => OnTotalTimeChanged(new TimeSpanEventArgs(totalTime)));

                var bufferedSeconds = bufferedWaveStream.BufferedDuration.TotalSeconds;

                if (bufferedSeconds < 0.5 && playbackState == StreamingPlaybackState.Playing && !isDownloaded)
                {
                    Buffer();
                }
                else if (playbackState == StreamingPlaybackState.Buffering)
                {
                    Play();
                }
                else if (isDownloaded && bufferedSeconds == 0)
                {
                    Debug.WriteLine("Reached end of stream");

                    if (!Next())
                    {
                        Stop();
                    }
                }
            }
        }

        private void Playlist_CurrentItemChanged(object sender, ItemChangedEventArgs<PlaylistItem> e)
        {
            var canPlayPrevious = false;
            var canPlayNext = false;

            if (e.NewValue != null)
            {
                Stop();
                Load();
                canPlayPrevious = Playlist.HasPrevious();
                canPlayNext = Playlist.HasNext();
            }

            if (canPlayPrevious != CanPlayPrevious)
            {
                CanPlayPrevious = canPlayPrevious;

                App.Current.Dispatcher.Invoke(new Action(() => OnCanPlayPreviousChanged(new CanPlayPreviousEventArgs(CanPlayPrevious))));
            }

            if (canPlayNext != CanPlayNext)
            {
                CanPlayNext = canPlayNext;

                App.Current.Dispatcher.Invoke(new Action(() => OnCanPlayNextChanged(new CanPlayNextEventArgs(CanPlayNext))));
            }
        }

        private void Playlist_ItemAdded(object sender, ItemAddedEventArgs<PlaylistItem> e)
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

        private void Playlist_ItemRemoved(object sender, ItemRemovedEventArgs<PlaylistItem> e)
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

        private void StartDownloading(object state)
        {
            isDownloaded = false;
            request = (HttpWebRequest)WebRequest.Create(playlist.CurrentItem.StreamUri);
            HttpWebResponse response;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Status != WebExceptionStatus.RequestCanceled)
                {
                    OnExceptionOccurred(e);
                }

                return;
            }

            var buffer = new byte[16384 * 4]; // needs to be big enough to hold a decompressed frame

            IMp3FrameDecompressor decompressor = null;

            try
            {
                using (var responseStream = response.GetResponseStream())
                {
                    var readFullyStream = new ReadFullyStream(responseStream);

                    do
                    {
                        if (IsBufferNearlyFull)
                        {
                            Debug.WriteLine("Buffer getting full, taking a break");
                            Thread.Sleep(500);
                        }
                        else
                        {
                            Mp3Frame frame;

                            try
                            {
                                frame = Mp3Frame.LoadFromStream(readFullyStream);
                            }
                            catch (EndOfStreamException)
                            {
                                Debug.WriteLine("Download complete.");

                                isDownloaded = true;

                                break;
                            }
                            catch (WebException)
                            {
                                // probably we have aborted download from the GUI thread
                                break;
                            }

                            if (frame == null)
                            {
                                Debug.WriteLine("Download complete.");

                                isDownloaded = true;

                                break;
                            }

                            if (decompressor == null)
                            {
                                decompressor = CreateFrameDecompressor(frame);
                                bufferedWaveStream = new BufferedWaveStream(decompressor.OutputFormat);
                                bufferedWaveStream.BufferDuration = TimeSpan.FromSeconds(60);
                            }

                            int decompressed = decompressor.DecompressFrame(frame, buffer, 0);
                            //Debug.WriteLine(String.Format("Decompressed a frame {0}", decompressed));
                            bufferedWaveStream.AddSamples(buffer, 0, decompressed);

                            totalBytes += decompressed;
                        }
                    } while (playbackState != StreamingPlaybackState.Stopped);

                    Debug.WriteLine("Exiting");
                    // was doing this in a finally block, but for some reason
                    // we are hanging on response stream .Dispose so never get there
                    decompressor.Dispose();
                }
            }
            finally
            {
                if (decompressor != null)
                {
                    decompressor.Dispose();
                }
            }
        }
    }
}