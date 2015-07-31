using NathanHarrenstein.MusicDB;
using NathanHarrenstein.MusicTimeline.Audio;
using NathanHarrenstein.MusicTimeline.Builders;
using NathanHarrenstein.MusicTimeline.Controls;
using NathanHarrenstein.MusicTimeline.Utilities;
using NAudio.Flac;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.EDTF;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class ComposerPage : Page, IDisposable
    {
        private AudioPlayer _audioPlayer;
        private DataProvider _dataProvider;
        private bool _disposed;
        private Dictionary<ISampleProvider, Sample> _sampleDictionary;

        public ComposerPage()
        {
            InitializeComponent();

            _dataProvider = new DataProvider();
            _sampleDictionary = new Dictionary<ISampleProvider, Sample>();
            _audioPlayer = new AudioPlayer();
            _audioPlayer.CurrentTimeChanged += AudioPlayer_CurrentTimeChanged;
            _audioPlayer.TotalTimeChanged += AudioPlayer_TotalTimeChanged;
            _audioPlayer.TrackChanged += AudioPlayer_TrackChanged;
            _audioPlayer.PlaybackStateChanged += AudioPlayer_PlaybackStateChanged;
            _audioPlayer.VolumeChanged += AudioPlayer_VolumeChanged;
            _audioPlayer.MuteChanged += AudioPlayer_MuteChanged;

            var composerName = System.Windows.Application.Current.Properties["SelectedComposer"] as string;
            var composer = _dataProvider.Composers.AsNoTracking().FirstOrDefault(c => c.Name == composerName);

            if (composer != null)
            {
                LoadComposer(composer);
            }
        }

        ~ComposerPage()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
                _disposed = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // clean up managed resources
            }

            _dataProvider.Dispose();
            _audioPlayer.Dispose();
        }

        private static string GetBorn(Composer composer)
        {
            if (composer.BirthLocation != null)
            {
                return $"{ExtendedDateTimeInterval.Parse(composer.Dates).Start}; {composer.BirthLocation.Name}";
            }

            return ExtendedDateTimeInterval.Parse(composer.Dates).Start.ToString();
        }

        private static string GetDied(Composer composer)
        {
            if (composer.DeathLocation != null)
            {
                return $"{ExtendedDateTimeInterval.Parse(composer.Dates).End}; {composer.DeathLocation.Name}";
            }

            return ExtendedDateTimeInterval.Parse(composer.Dates).End.ToString();
        }

        private void AudioPlayer_CurrentTimeChanged(object sender, TimeSpanEventArgs e)
        {
            ProgressSlider.Value = e.TimeSpan.Ticks;
        }

        private void AudioPlayer_MuteChanged(object sender, MuteEventArgs e)
        {
            MuteToggleButton.IsEnabled = true;
            MuteToggleButton.IsChecked = e.Mute;
        }

        private void AudioPlayer_PlaybackStateChanged(object sender, PlaybackStateEventArgs e)
        {
            PlayPauseToggleButton.IsChecked = e.PlaybackState == PlaybackState.Playing ? true : false;
        }

        private void AudioPlayer_TotalTimeChanged(object sender, TimeSpanEventArgs e)
        {
            ProgressSlider.Maximum = e.TimeSpan.Ticks;
        }

        private void AudioPlayer_TrackChanged(object sender, TrackEventArgs e)
        {
            NowPlayingTitleTextBlock.Text = _sampleDictionary[_audioPlayer.CurrentPlaylistItem.Value].Title;
            NowPlayingArtistTextBlock.Text = _sampleDictionary[_audioPlayer.CurrentPlaylistItem.Value].Artists;
        }

        private void AudioPlayer_VolumeChanged(object sender, VolumeEventArgs e)
        {
            VolumeSlider.IsEnabled = true;
            VolumeSlider.Value = e.Volume;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();

            e.Handled = true;
        }

        private void BuildBiographySection(Composer composer)
        {
            if (composer.Biography == null)
            {
                return;
            }

            var parserContext = new ParserContext();
            parserContext.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            parserContext.XmlSpace = "preserve";

            var section = (Section)XamlReader.Load(new MemoryStream(Encoding.UTF8.GetBytes(composer.Biography)), parserContext);

            if (section == null)
            {
                return;
            }

            var defaultFontSize = TextElement.GetFontSize(this);
            var headers = section.Blocks.Where(b => b.Tag != null);

            foreach (var header in headers)
            {
                header.FontWeight = FontWeights.Bold;

                switch ((string)header.Tag)
                {
                    case "h1":
                        header.FontSize = defaultFontSize * 2;
                        break;

                    case "h2":
                        header.FontSize = defaultFontSize * 1.5;
                        break;

                    case "h3":
                        header.FontSize = defaultFontSize * 1.17;
                        break;

                    case "h4":
                        break;

                    case "h5":
                        header.FontSize = defaultFontSize * 0.83;
                        break;

                    case "h6":
                        header.FontSize = defaultFontSize * 0.67;
                        break;

                    default:
                        break;
                }
            }

            BiographyFlowDocument.Blocks.Add(section);
        }

        private void ComposerButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var composer = (Composer)button.DataContext;

            LoadComposer(composer);
        }

        private void LoadComposer(Composer composer)
        {
            var influencedVisibility = composer.Influenced.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            var influencesVisibility = composer.Influences.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            var linksVisibility = composer.ComposerLinks.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

            BuildBiographySection(composer);
            BornTextBlock.Text = GetBorn(composer);
            ComposerImagesListBox.ItemsSource = composer.ComposerImages;
            ComposerNameTextBlock.Text = NameUtility.ToFirstLast(composer.Name);
            DiedTextBlock.Text = GetDied(composer);
            ComposerFlagsItemsControl.ItemsSource = composer.Nationalities;
            InfluencedItemsControl.ItemsSource = composer.Influenced;
            InfluencedItemsControl.Visibility = influencedVisibility;
            InfluencedTextBlock.Visibility = influencedVisibility;
            InfluencesItemsControl.ItemsSource = composer.Influences;
            InfluencesItemsControl.Visibility = influencesVisibility;
            InfluencesTextBlock.Visibility = influencesVisibility;
            LinksItemControl.ItemsSource = composer.ComposerLinks;
            LinksItemControl.Visibility = LinksTextBlock.Visibility = linksVisibility;
            TreeView.Children = composer.CompositionCollections
                .Select<CompositionCollection, Controls.TreeViewItem>(cc => CompositionCollectionTreeViewItemBuilder.Build(cc, null))
                .Concat(composer.Compositions
                    .Select(c => CompositionTreeViewItemBuilder.GetCompositionTreeViewItem(c, null)))
                .OrderBy(tvi => tvi.Header);

            ComposerImagesListBox.SelectedIndex = 0;

            foreach (var sample in composer.Samples)
            {
                var flacReader = new FlacReader(new MemoryStream(sample.Audio));

                _sampleDictionary[flacReader] = sample;

                _audioPlayer.AddToPlaylist(flacReader);
            }

            if (_audioPlayer.Playlist.Count > 0)
            {
                _audioPlayer.Play();
            }
        }

        private void MuteVolume_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _audioPlayer != null;
        }

        private void MuteVolume_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _audioPlayer.Mute = !_audioPlayer.Mute;
        }

        private void NextTrack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_audioPlayer != null)
            {
                e.CanExecute = _audioPlayer.CanSkipForward();
            }
        }

        private void NextTrack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_audioPlayer != null)
            {
                _audioPlayer.SkipForward();
                _audioPlayer.Play();

                PlayPauseToggleButton.IsChecked = true;
            }
        }

        private void PreviousTrack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_audioPlayer != null)
            {
                e.CanExecute = _audioPlayer.CanSkipBack();
            }
        }

        private void PreviousTrack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_audioPlayer != null)
            {
                _audioPlayer.SkipBack();
                _audioPlayer.Play();

                PlayPauseToggleButton.IsChecked = true;
            }
        }

        private void ProgressSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            _audioPlayer.CurrentTime = new TimeSpan((long)ProgressSlider.Value);
            _audioPlayer.Play();
        }

        private void ProgressSlider_DragStarted(object sender, RoutedEventArgs e)
        {
            _audioPlayer.Stop();
        }

        private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ProgressStatus.Text = TimeSpan.FromTicks((long)ProgressSlider.Value).ToString(@"hh\:mm\:ss");
        }

        private void TogglePlayPause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _audioPlayer != null;
        }

        private void TogglePlayPause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_audioPlayer.PlaybackState == PlaybackState.Playing)
            {
                _audioPlayer.Pause();
                PlayPauseToggleButton.IsChecked = false;
            }
            else
            {
                _audioPlayer.Play();
                PlayPauseToggleButton.IsChecked = true;
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_audioPlayer == null)
            {
                return;
            }

            _audioPlayer.Volume = (float)e.NewValue;
        }
    }
}