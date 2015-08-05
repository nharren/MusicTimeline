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
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class ComposerPage : Page, IDisposable
    {
        private Composer _composer;
        private DataProvider _dataProvider;
        private bool _disposed;
        private FlacPlayer _flacPlayer;
        private Dictionary<ISampleProvider, Sample> _sampleDictionary;

        public ComposerPage()
        {
            InitializeComponent();

            _dataProvider = new DataProvider();
            _sampleDictionary = new Dictionary<ISampleProvider, Sample>();
            _flacPlayer = new FlacPlayer();
            _flacPlayer.CurrentTimeChanged += FlacPlayer_CurrentTimeChanged;
            _flacPlayer.TotalTimeChanged += FlacPlayer_TotalTimeChanged;
            _flacPlayer.TrackChanged += FlacPlayer_TrackChanged;
            _flacPlayer.PlaybackStateChanged += FlacPlayer_PlaybackStateChanged;
            _flacPlayer.VolumeChanged += FlacPlayer_VolumeChanged;
            _flacPlayer.MuteChanged += FlacPlayer_MuteChanged;

            var composerName = System.Windows.Application.Current.Properties["SelectedComposer"] as string;
            _composer = _dataProvider.Composers.AsNoTracking().FirstOrDefault(c => c.Name == composerName);

            if (_composer != null)
            {
                LoadComposer();
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
            _flacPlayer.Dispose();
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

            _composer = (Composer)button.DataContext;

            LoadComposer();
        }

        private void FlacPlayer_CurrentTimeChanged(object sender, TimeSpanEventArgs e)
        {
            ProgressSlider.Value = e.TimeSpan.Ticks;
        }

        private void FlacPlayer_MuteChanged(object sender, MuteEventArgs e)
        {
            MuteToggleButton.IsEnabled = true;
            MuteToggleButton.IsChecked = e.Mute;
        }

        private void FlacPlayer_PlaybackStateChanged(object sender, PlaybackStateEventArgs e)
        {
            PlayPauseToggleButton.IsChecked = e.PlaybackState == PlaybackState.Playing ? true : false;
        }

        private void FlacPlayer_TotalTimeChanged(object sender, TimeSpanEventArgs e)
        {
            ProgressSlider.Maximum = e.TimeSpan.Ticks;
        }

        private void FlacPlayer_TrackChanged(object sender, TrackEventArgs e)
        {
            NowPlayingTitleTextBlock.Text = _sampleDictionary[_flacPlayer.CurrentPlaylistItem.Value].Title;
            NowPlayingArtistTextBlock.Text = _sampleDictionary[_flacPlayer.CurrentPlaylistItem.Value].Artists;
        }

        private void FlacPlayer_VolumeChanged(object sender, VolumeEventArgs e)
        {
            VolumeSlider.IsEnabled = true;
            VolumeSlider.Value = e.Volume;
        }

        private void LoadComposer()
        {
            var influencedVisibility = _composer.Influenced.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            var influencesVisibility = _composer.Influences.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            var linksVisibility = _composer.ComposerLinks.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

            BuildBiographySection(_composer);
            BornTextBlock.Text = GetBorn(_composer);
            ComposerImagesListBox.ItemsSource = _composer.ComposerImages;
            ComposerNameTextBlock.Text = NameUtility.ToFirstLast(_composer.Name);
            DiedTextBlock.Text = GetDied(_composer);
            ComposerFlagsItemsControl.ItemsSource = _composer.Nationalities;
            InfluencedItemsControl.ItemsSource = _composer.Influenced;
            InfluencedItemsControl.Visibility = influencedVisibility;
            InfluencedTextBlock.Visibility = influencedVisibility;
            InfluencesItemsControl.ItemsSource = _composer.Influences;
            InfluencesItemsControl.Visibility = influencesVisibility;
            InfluencesTextBlock.Visibility = influencesVisibility;
            LinksItemControl.ItemsSource = _composer.ComposerLinks;
            LinksItemControl.Visibility = LinksTextBlock.Visibility = linksVisibility;

            var compositionTypes = _composer.CompositionCollections
                .SelectMany(cc => cc.Compositions)
                .Concat(_composer.Compositions)
                .GroupBy(c => c.CompositionType?.Name ?? "Unknown");

            TreeView.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(compositionTypes, null, "Key"));

            ComposerImagesListBox.SelectedIndex = 0;

            StartSampleLoadingThread();
        }

        private void LoadSamples()
        {
            foreach (var sample in _composer.Samples)
            {
                var flacReader = new FlacReader(new MemoryStream(sample.Audio));

                _sampleDictionary[flacReader] = sample;
                _flacPlayer.AddToPlaylist(flacReader);

                if (_sampleDictionary.Count == 1)
                {
                    _flacPlayer.Play();
                }
            }
        }

        private void MuteVolume_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _flacPlayer != null;
        }

        private void MuteVolume_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _flacPlayer.Mute = !_flacPlayer.Mute;
        }

        private void NextTrack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_flacPlayer != null)
            {
                e.CanExecute = _flacPlayer.CanSkipForward();
            }
        }

        private void NextTrack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_flacPlayer != null)
            {
                _flacPlayer.SkipForward();
                _flacPlayer.Play();

                PlayPauseToggleButton.IsChecked = true;
            }
        }

        private void PreviousTrack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_flacPlayer != null)
            {
                e.CanExecute = _flacPlayer.CanSkipBack();
            }
        }

        private void PreviousTrack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_flacPlayer != null)
            {
                _flacPlayer.SkipBack();
                _flacPlayer.Play();

                PlayPauseToggleButton.IsChecked = true;
            }
        }

        private void ProgressSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            _flacPlayer.CurrentTime = new TimeSpan((long)ProgressSlider.Value);
            _flacPlayer.Play();
        }

        private void ProgressSlider_DragStarted(object sender, RoutedEventArgs e)
        {
            _flacPlayer.Stop();
        }

        private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ProgressStatus.Text = TimeSpan.FromTicks((long)ProgressSlider.Value).ToString(@"hh\:mm\:ss");
        }

        private void StartSampleLoadingThread()
        {
            var sampleLoadingThread = new Thread(new ThreadStart(LoadSamples));
            sampleLoadingThread.Name = "Sample Loading Thread";
            sampleLoadingThread.IsBackground = true;
            sampleLoadingThread.Start();
        }

        private void TogglePlayPause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _flacPlayer != null;
        }

        private void TogglePlayPause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_flacPlayer.PlaybackState == PlaybackState.Playing)
            {
                _flacPlayer.Pause();
                PlayPauseToggleButton.IsChecked = false;
            }
            else
            {
                _flacPlayer.Play();
                PlayPauseToggleButton.IsChecked = true;
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_flacPlayer == null)
            {
                return;
            }

            _flacPlayer.Volume = (float)e.NewValue;
        }
    }
}