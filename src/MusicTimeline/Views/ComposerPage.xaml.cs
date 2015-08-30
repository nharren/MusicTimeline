using NathanHarrenstein.ClassicalMusicDb;
using NathanHarrenstein.MusicTimeline.Audio;
using NathanHarrenstein.MusicTimeline.Builders;
using NathanHarrenstein.MusicTimeline.Comparers;
using NathanHarrenstein.MusicTimeline.Utilities;
using NAudio.Flac;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.EDTF;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class ComposerPage : Page, IDisposable
    {
        private static LogicalComparer _logicalComparer;
        private ClassicalMusicContext _classicalMusicContext;
        private Composer _composer;
        private FlacPlayer _flacPlayer;
        private bool _isDisposed;
        private Dictionary<ISampleProvider, Sample> _sampleDictionary;

        public ComposerPage()
        {
            InitializeComponent();

            _logicalComparer = new LogicalComparer();
            _classicalMusicContext = new ClassicalMusicContext();
            _sampleDictionary = new Dictionary<ISampleProvider, Sample>();
            _flacPlayer = new FlacPlayer();

            _flacPlayer.CurrentTimeChanged += FlacPlayer_CurrentTimeChanged;
            _flacPlayer.TotalTimeChanged += FlacPlayer_TotalTimeChanged;
            _flacPlayer.TrackChanged += FlacPlayer_TrackChanged;
            _flacPlayer.PlaybackStateChanged += FlacPlayer_PlaybackStateChanged;
            _flacPlayer.VolumeChanged += FlacPlayer_VolumeChanged;
            _flacPlayer.IsMutedChanged += FlacPlayer_IsMutedChanged;
            _flacPlayer.CanPlayChanged += FlacPlayer_CanPlayChanged;
            _flacPlayer.CanSkipBackChanged += FlacPlayer_CanSkipBackChanged;
            _flacPlayer.CanSkipForwardChanged += FlacPlayer_CanSkipForwardChanged;

            Loaded += ComposerPage_Loaded;
        }

        private async void ComposerPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadComposerAsync();
        }

        private async Task LoadComposerAsync()
        {
            if (ProgressBar.Visibility == Visibility.Collapsed)
            {
                ProgressBar.Visibility = Visibility.Visible;
            }

            var composerName = Application.Current.Properties["SelectedComposer"] as string;

            _composer = await _classicalMusicContext.Composers
                .Where(c => c.Name == composerName)
                .Include(c => c.CompositionCollections)
                .Include(c => c.Compositions)
                .FirstOrDefaultAsync();

            var influencedVisibility = _composer.Influenced.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            var influencesVisibility = _composer.Influences.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            var webpagesVisibility = _composer.Links.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            
            BornTextBlock.Text = GetBorn(_composer);
            ComposerNameTextBlock.Text = NameUtility.ToFirstLast(_composer.Name);
            DiedTextBlock.Text = GetDied(_composer);
            ComposerFlagsItemsControl.ItemsSource = _composer.Nationalities;
            InfluencedItemsControl.ItemsSource = _composer.Influenced;
            InfluencedItemsControl.Visibility = influencedVisibility;
            InfluencedTextBlock.Visibility = influencedVisibility;
            InfluencedUnderline.Visibility = influencedVisibility;
            InfluencesItemsControl.ItemsSource = _composer.Influences;
            InfluencesItemsControl.Visibility = influencesVisibility;
            InfluencesTextBlock.Visibility = influencesVisibility;
            InfluencesUnderline.Visibility = influencesVisibility;
            LinksItemControl.ItemsSource = _composer.Links;
            LinksItemControl.Visibility = webpagesVisibility;
            LinksTextBlock.Visibility = webpagesVisibility;
            LinksUnderline.Visibility = webpagesVisibility;

            BuildBiographySection(_composer);

            var compositionTypes = _composer.CompositionCollections
                .SelectMany(cc => cc.Compositions)
                .Concat(_composer.Compositions)
                .GroupBy(c => c.Genre?.Name ?? "Unknown")
                .OrderBy(s => s.Key);

            TreeView.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(compositionTypes));

            await _classicalMusicContext.Entry(_composer)
                .Collection("ComposerImages")
                .LoadAsync();

            ComposerImagesListBox.ItemsSource = _composer.ComposerImages.Count == 0 ? new List<ComposerImage> { GetDefaultComposerImage() } : _composer.ComposerImages;
            ComposerImagesListBox.SelectedIndex = 0;

            await _classicalMusicContext.Entry(_composer)
                .Collection("Samples")
                .LoadAsync();

            LoadSamples();

            ProgressBar.Visibility = Visibility.Collapsed;
        }

        ~ComposerPage()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _classicalMusicContext.Dispose();
                _flacPlayer.Dispose();

                _isDisposed = true;
            }
        }

        private static string GetBorn(Composer composer)
        {
            if (composer.Details.BirthLocation != null)
            {
                return $"{ExtendedDateTimeInterval.Parse(composer.Dates).Start}; {composer.Details.BirthLocation.Name}";
            }

            return ExtendedDateTimeInterval.Parse(composer.Dates).Start.ToString();
        }

        private static string GetDied(Composer composer)
        {
            if (composer.Details.DeathLocation != null)
            {
                return $"{ExtendedDateTimeInterval.Parse(composer.Dates).End}; {composer.Details.DeathLocation.Name}";
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
            if (composer.Details.Biography == null)
            {
                return;
            }

            var parserContext = new ParserContext();
            parserContext.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            parserContext.XmlSpace = "preserve";

            var section = (Section)XamlReader.Load(new MemoryStream(Encoding.UTF8.GetBytes(composer.Details.Biography)), parserContext);

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

            var flowDocument = new FlowDocument(section);
            flowDocument.FontFamily = new FontFamily("Cambria");
            flowDocument.PagePadding = new Thickness(0, 5, 0, 0);
            flowDocument.TextAlignment = TextAlignment.Left;

            FlowDocumentScrollViewer.Document = flowDocument;
        }

        private async void ComposerButton_Click(object sender, RoutedEventArgs e)
        {
            _sampleDictionary.Clear();

            _flacPlayer.Dispose();

            _flacPlayer = new FlacPlayer();

            _flacPlayer.CurrentTimeChanged += FlacPlayer_CurrentTimeChanged;
            _flacPlayer.TotalTimeChanged += FlacPlayer_TotalTimeChanged;
            _flacPlayer.TrackChanged += FlacPlayer_TrackChanged;
            _flacPlayer.PlaybackStateChanged += FlacPlayer_PlaybackStateChanged;
            _flacPlayer.VolumeChanged += FlacPlayer_VolumeChanged;
            _flacPlayer.IsMutedChanged += FlacPlayer_IsMutedChanged;
            _flacPlayer.CanPlayChanged += FlacPlayer_CanPlayChanged;
            _flacPlayer.CanSkipBackChanged += FlacPlayer_CanSkipBackChanged;
            _flacPlayer.CanSkipForwardChanged += FlacPlayer_CanSkipForwardChanged;

            NowPlayingTitleTextBlock.Text = null;
            NowPlayingArtistTextBlock.Text = null;

            PlayPauseToggleButton.IsEnabled = false;
            SkipBackButton.IsEnabled = false;
            SkipForwardButton.IsEnabled = false;
            ProgressSlider.IsEnabled = false;
            MuteToggleButton.IsEnabled = false;
            VolumeSlider.IsEnabled = false;

            var button = (Button)sender;

            Application.Current.Properties["SelectedComposer"] = ((Composer)button.DataContext).Name;

            await LoadComposerAsync();
        }

        private void FlacPlayer_CanPlayChanged(object sender, CanPlayEventArgs e)
        {
            PlayPauseToggleButton.IsEnabled = e.CanPlay;
        }

        private void FlacPlayer_CanSkipBackChanged(object sender, CanSkipBackEventArgs e)
        {
            SkipBackButton.IsEnabled = e.CanSkipBack;
        }

        private void FlacPlayer_CanSkipForwardChanged(object sender, CanSkipForwardEventArgs e)
        {
            SkipForwardButton.IsEnabled = e.CanSkipForward;
        }

        private void FlacPlayer_CurrentTimeChanged(object sender, TimeSpanEventArgs e)
        {
            ProgressSlider.IsEnabled = true;
            ProgressSlider.Value = e.TimeSpan.Ticks;
        }

        private void FlacPlayer_IsMutedChanged(object sender, MuteEventArgs e)
        {
            MuteToggleButton.IsEnabled = true;
            MuteToggleButton.IsChecked = e.Mute;
        }

        private void FlacPlayer_PlaybackStateChanged(object sender, PlaybackStateEventArgs e)
        {
            PlayPauseToggleButton.IsEnabled = true;
            PlayPauseToggleButton.IsChecked = e.PlaybackState == PlaybackState.Playing ? true : false;
        }

        private void FlacPlayer_TotalTimeChanged(object sender, TimeSpanEventArgs e)
        {
            ProgressSlider.IsEnabled = true;
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

        private ComposerImage GetDefaultComposerImage()
        {
            var defaultComposerImageUri = new Uri("pack://application:,,,/Resources/Composers/Unknown.jpg", UriKind.Absolute);
            var streamResourceInfo = Application.GetResourceStream(defaultComposerImageUri);

            var composerImage = new ComposerImage();
            composerImage.Bytes = StreamUtility.ReadToEnd(streamResourceInfo.Stream);

            return composerImage;
        }

        private void LoadSamples()
        {
            foreach (var sample in _composer.Samples)
            {
                var flacReader = new FlacReader(new MemoryStream(sample.Bytes));

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
            _flacPlayer.ToggleMute();
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