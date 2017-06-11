using NathanHarrenstein.MusicTimeline.Audio;
using NAudio.Wave;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class Mp3PlayerControl : Control, IDisposable
    {
        private Mp3StreamPlayer mp3Player;
        private ToggleButton muteToggleButton;
        private TextBlock nowPlayingArtistTextBlock;
        private Marquee nowPlayingTitleMarquee;
        private Button playNextButton;
        private ToggleButton playPauseToggleButton;
        private Button playPreviousButton;
        private Slider progressSlider;
        private TextBlock progressStatus;
        private Slider volumeSlider;

        static Mp3PlayerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Mp3PlayerControl), new FrameworkPropertyMetadata(typeof(Mp3PlayerControl)));
        }

        public Mp3PlayerControl()
        {
            mp3Player = new Mp3StreamPlayer();
        }

        public Playlist Playlist
        {
            get
            {
                return mp3Player.Playlist;
            }
        }

        public override void OnApplyTemplate()
        {
            if (isTemplateApplied)
            {
                return;
            }

            playPauseToggleButton = (ToggleButton)Template.FindName("playPauseToggleButton", this);
            playPreviousButton = (Button)Template.FindName("playPreviousButton", this);
            playNextButton = (Button)Template.FindName("playNextButton", this);
            progressSlider = (Slider)Template.FindName("progressSlider", this);
            progressStatus = (TextBlock)Template.FindName("progressStatus", this);
            muteToggleButton = (ToggleButton)Template.FindName("muteToggleButton", this);
            nowPlayingTitleMarquee = (Marquee)Template.FindName("nowPlayingTitleMarquee", this);
            nowPlayingArtistTextBlock = (TextBlock)Template.FindName("nowPlayingArtistTextBlock", this);
            volumeSlider = (Slider)Template.FindName("volumeSlider", this);

            mp3Player.CurrentTimeChanged += mp3Player_CurrentTimeChanged;
            mp3Player.TotalTimeChanged += mp3Player_TotalTimeChanged;
            mp3Player.TrackChanged += mp3Player_TrackChanged;
            mp3Player.PlaybackStateChanged += mp3Player_PlaybackStateChanged;
            mp3Player.VolumeChanged += mp3Player_VolumeChanged;
            mp3Player.IsMutedChanged += mp3Player_IsMutedChanged;
            mp3Player.CanPlayChanged += mp3Player_CanPlayChanged;
            mp3Player.CanPlayPreviousChanged += mp3Player_CanPlayPreviousChanged;
            mp3Player.CanPlayNextChanged += mp3Player_CanPlayNextChanged;
            progressSlider.AddHandler(Thumb.DragStartedEvent, new RoutedEventHandler(progressSlider_DragStarted));
            progressSlider.AddHandler(Thumb.DragCompletedEvent, new RoutedEventHandler(progressSlider_DragCompleted));
            progressSlider.ValueChanged += progressSlider_ValueChanged;
            volumeSlider.ValueChanged += volumeSlider_ValueChanged;

            CommandManager.AddExecutedHandler(playPauseToggleButton, new ExecutedRoutedEventHandler(playPauseToggleButton_Executed));
            CommandManager.AddCanExecuteHandler(playPauseToggleButton, new CanExecuteRoutedEventHandler(playPauseToggleButton_CanExecute));
            CommandManager.AddExecutedHandler(playPreviousButton, new ExecutedRoutedEventHandler(playPreviousButton_Executed));
            CommandManager.AddCanExecuteHandler(playPreviousButton, new CanExecuteRoutedEventHandler(playPreviousButton_CanExecute));
            CommandManager.AddExecutedHandler(playNextButton, new ExecutedRoutedEventHandler(playNextButton_Executed));
            CommandManager.AddCanExecuteHandler(playNextButton, new CanExecuteRoutedEventHandler(playNextButton_CanExecute));
            CommandManager.AddExecutedHandler(muteToggleButton, new ExecutedRoutedEventHandler(muteToggleButton_Executed));
            CommandManager.AddCanExecuteHandler(muteToggleButton, new CanExecuteRoutedEventHandler(muteToggleButton_CanExecute));

            isTemplateApplied = true;
        }

        public void Play()
        {
            mp3Player.Play();
        }

        private void mp3Player_CanPlayChanged(object sender, CanPlayEventArgs e)
        {
            playPauseToggleButton.IsEnabled = e.CanPlay;
        }

        private void mp3Player_CanPlayNextChanged(object sender, CanPlayNextEventArgs e)
        {
            playNextButton.IsEnabled = e.CanPlayNext;
        }

        private void mp3Player_CanPlayPreviousChanged(object sender, CanPlayPreviousEventArgs e)
        {
            playPreviousButton.IsEnabled = e.CanPlayPrevious;
        }

        private void mp3Player_CurrentTimeChanged(object sender, TimeSpanEventArgs e)
        {
            progressSlider.IsEnabled = true;
            progressSlider.Value = e.TimeSpan.Ticks;
        }

        private void mp3Player_IsMutedChanged(object sender, MuteEventArgs e)
        {
            muteToggleButton.IsChecked = e.Mute;
        }

        private void mp3Player_PlaybackStateChanged(object sender, PlaybackStateEventArgs e)
        {
            playPauseToggleButton.IsChecked = e.PlaybackState == StreamingPlaybackState.Playing;

            CommandManager.InvalidateRequerySuggested();
        }

        private void mp3Player_TotalTimeChanged(object sender, TimeSpanEventArgs e)
        {
            progressSlider.IsEnabled = true;
            progressSlider.Maximum = e.TimeSpan.Ticks;
        }

        private void mp3Player_TrackChanged(object sender, EventArgs e)
        {
            string title;
            string artist;

            if (Playlist.CurrentItem.Metadata.TryGetValue("Title", out title))
            {
                nowPlayingTitleMarquee.Content = title;
            }

            if (Playlist.CurrentItem.Metadata.TryGetValue("Artist", out artist))
            {
                nowPlayingArtistTextBlock.Text = artist;
            }
        }

        private void mp3Player_VolumeChanged(object sender, VolumeEventArgs e)
        {
            volumeSlider.IsEnabled = true;
            volumeSlider.Value = e.Volume;
        }

        private void muteToggleButton_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mp3Player != null;
        }

        private void muteToggleButton_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mp3Player.IsMuted = !mp3Player.IsMuted;
        }

        private void playNextButton_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mp3Player != null && mp3Player.CanPlayNext;
        }

        private void playNextButton_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mp3Player.Next();
            mp3Player.Play();

            playPauseToggleButton.IsChecked = true;
        }

        private void playPauseToggleButton_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mp3Player != null;
        }

        private void playPauseToggleButton_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (mp3Player.PlaybackState == StreamingPlaybackState.Playing)
            {
                mp3Player.Pause();

                playPauseToggleButton.IsChecked = false;
            }
            else
            {
                mp3Player.Play();

                playPauseToggleButton.IsChecked = true;
            }
        }

        private void playPreviousButton_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mp3Player != null && mp3Player.CanPlayPrevious;
        }

        private void playPreviousButton_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mp3Player.Previous();
            mp3Player.Play();

            playPauseToggleButton.IsChecked = true;
        }

        private void progressSlider_DragCompleted(object sender, RoutedEventArgs e)
        {
            mp3Player.CurrentTime = new TimeSpan((long)progressSlider.Value);

            mp3Player.CurrentTimeChanged += mp3Player_CurrentTimeChanged;
            mp3Player.TotalTimeChanged += mp3Player_TotalTimeChanged;
        }

        private void progressSlider_DragStarted(object sender, RoutedEventArgs e)
        {
            mp3Player.CurrentTimeChanged -= mp3Player_CurrentTimeChanged;
            mp3Player.TotalTimeChanged -= mp3Player_TotalTimeChanged;
        }

        private void progressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            progressStatus.Text = $"{TimeSpan.FromTicks((long)progressSlider.Value).ToString(@"mm\:ss")} / {TimeSpan.FromTicks((long)progressSlider.Maximum).ToString(@"mm\:ss")}";
        }

        private void volumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mp3Player == null)
            {
                return;
            }

            mp3Player.Volume = (float)e.NewValue;
        }
       
        private bool isDisposed = false;
        private bool isTemplateApplied;

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            if (mp3Player != null)
            {
                mp3Player.PlaybackStateChanged -= mp3Player_PlaybackStateChanged;
                mp3Player.Stop();
                mp3Player.Dispose();
            }

            isDisposed = true;
        }

        ~Mp3PlayerControl()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public void Stop()
        {
            mp3Player.Stop();
        }
    }
}