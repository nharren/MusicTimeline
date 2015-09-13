using NathanHarrenstein.ClassicalMusicDb;
using NathanHarrenstein.MusicTimeline.Audio;
using NathanHarrenstein.MusicTimeline.Builders;
using NathanHarrenstein.MusicTimeline.Comparers;
using NathanHarrenstein.MusicTimeline.Utilities;
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
        private bool _isDisposed;
        private CancellationTokenSource _loadingCancellationTokenSource;
        private Mp3Player _mp3Player;
        private Dictionary<IWaveProvider, Sample> _sampleDictionary;

        public ComposerPage()
        {
            InitializeComponent();

            _logicalComparer = new LogicalComparer();
            _classicalMusicContext = new ClassicalMusicContext();
            _sampleDictionary = new Dictionary<IWaveProvider, Sample>();
            _mp3Player = new Mp3Player();

            _mp3Player.CurrentTimeChanged += Mp3Player_CurrentTimeChanged;
            _mp3Player.TotalTimeChanged += Mp3Player_TotalTimeChanged;
            _mp3Player.TrackChanged += Mp3Player_TrackChanged;
            _mp3Player.PlaybackStateChanged += Mp3Player_PlaybackStateChanged;
            _mp3Player.VolumeChanged += Mp3Player_VolumeChanged;
            _mp3Player.IsMutedChanged += Mp3Player_IsMutedChanged;
            _mp3Player.CanPlayChanged += Mp3Player_CanPlayChanged;
            _mp3Player.CanSkipBackChanged += Mp3Player_CanSkipBackChanged;
            _mp3Player.CanSkipForwardChanged += Mp3Player_CanSkipForwardChanged;

            Loaded += ComposerPage_Loaded;
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
                if (disposing)
                {
                    _loadingCancellationTokenSource.Cancel();
                }

                _classicalMusicContext.Dispose();
                _mp3Player.Dispose();

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

            var flowDocument = (FlowDocument)XamlReader.Load(new MemoryStream(Encoding.UTF8.GetBytes(composer.Details.Biography)), parserContext);

            if (flowDocument == null)
            {
                return;
            }

            flowDocument.FontFamily = new FontFamily("Cambria");
            flowDocument.PagePadding = new Thickness(0, 5, 0, 0);
            flowDocument.TextAlignment = TextAlignment.Left;

            FlowDocumentScrollViewer.Document = flowDocument;
        }

        private async void ComposerPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadComposerAsync();
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _sampleDictionary.Clear();

            _mp3Player.Dispose();

            NavigationService.Navigate(new Uri(@"pack://application:,,,/Views/ComposerEditPage.xaml", UriKind.Absolute));
        }

        private ComposerImage GetDefaultComposerImage()
        {
            var defaultComposerImageUri = new Uri("pack://application:,,,/Resources/Composers/Unknown.jpg", UriKind.Absolute);
            var streamResourceInfo = Application.GetResourceStream(defaultComposerImageUri);

            var composerImage = new ComposerImage();
            composerImage.Bytes = StreamUtility.ReadToEnd(streamResourceInfo.Stream);

            return composerImage;
        }

        private async void Influence_Click(object sender, RoutedEventArgs e)
        {
            _sampleDictionary.Clear();

            _mp3Player.Dispose();

            _mp3Player = new Mp3Player();

            _mp3Player.CurrentTimeChanged += Mp3Player_CurrentTimeChanged;
            _mp3Player.TotalTimeChanged += Mp3Player_TotalTimeChanged;
            _mp3Player.TrackChanged += Mp3Player_TrackChanged;
            _mp3Player.PlaybackStateChanged += Mp3Player_PlaybackStateChanged;
            _mp3Player.VolumeChanged += Mp3Player_VolumeChanged;
            _mp3Player.IsMutedChanged += Mp3Player_IsMutedChanged;
            _mp3Player.CanPlayChanged += Mp3Player_CanPlayChanged;
            _mp3Player.CanSkipBackChanged += Mp3Player_CanSkipBackChanged;
            _mp3Player.CanSkipForwardChanged += Mp3Player_CanSkipForwardChanged;

            NowPlayingTitleTextBlock.Text = null;
            NowPlayingArtistTextBlock.Text = null;

            PlayPauseToggleButton.IsEnabled = false;
            SkipBackButton.IsEnabled = false;
            SkipForwardButton.IsEnabled = false;
            ProgressSlider.IsEnabled = false;
            MuteToggleButton.IsEnabled = false;
            VolumeSlider.IsEnabled = false;

            var button = (Button)sender;

            Application.Current.Properties["SelectedComposer"] = ((Composer)button.DataContext).ComposerId;

            await LoadComposerAsync();
        }

        private async Task LoadComposerAsync()
        {
            _loadingCancellationTokenSource = new CancellationTokenSource();

            if (ProgressBar.Visibility == Visibility.Collapsed)
            {
                ProgressBar.Visibility = Visibility.Visible;
            }

            var composerId = (int)Application.Current.Properties["SelectedComposer"];

            _composer = await _classicalMusicContext.Composers
                .Where(c => c.ComposerId == composerId)
                .Include(c => c.CompositionCollections)
                .Include(c => c.Compositions)
                .FirstOrDefaultAsync(_loadingCancellationTokenSource.Token);

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
                .LoadAsync(_loadingCancellationTokenSource.Token);

            ComposerImagesListBox.ItemsSource = _composer.ComposerImages.Count == 0 ? new List<ComposerImage> { GetDefaultComposerImage() } : _composer.ComposerImages;
            ComposerImagesListBox.SelectedIndex = 0;

            await _classicalMusicContext.Entry(_composer)
                .Collection("Samples")
                .LoadAsync(_loadingCancellationTokenSource.Token);

            LoadSamples();

            ProgressBar.Visibility = Visibility.Collapsed;
        }

        private void LoadSamples()
        {
            foreach (var sample in _composer.Samples)
            {
                var mp3FileReader = new Mp3FileReader(new MemoryStream(sample.Bytes));

                _sampleDictionary[mp3FileReader] = sample;
                _mp3Player.AddToPlaylist(mp3FileReader);

                if (_sampleDictionary.Count == 1)
                {
                    _mp3Player.Play();
                }
            }
        }

        private void Mp3Player_CanPlayChanged(object sender, CanPlayEventArgs e)
        {
            PlayPauseToggleButton.IsEnabled = e.CanPlay;
        }

        private void Mp3Player_CanSkipBackChanged(object sender, CanSkipBackEventArgs e)
        {
            SkipBackButton.IsEnabled = e.CanSkipBack;
        }

        private void Mp3Player_CanSkipForwardChanged(object sender, CanSkipForwardEventArgs e)
        {
            SkipForwardButton.IsEnabled = e.CanSkipForward;
        }

        private void Mp3Player_CurrentTimeChanged(object sender, TimeSpanEventArgs e)
        {
            ProgressSlider.IsEnabled = true;
            ProgressSlider.Value = e.TimeSpan.Ticks;
        }

        private void Mp3Player_IsMutedChanged(object sender, MuteEventArgs e)
        {
            MuteToggleButton.IsEnabled = true;
            MuteToggleButton.IsChecked = e.Mute;
        }

        private void Mp3Player_PlaybackStateChanged(object sender, PlaybackStateEventArgs e)
        {
            PlayPauseToggleButton.IsEnabled = true;
            PlayPauseToggleButton.IsChecked = e.PlaybackState == PlaybackState.Playing ? true : false;
        }

        private void Mp3Player_TotalTimeChanged(object sender, TimeSpanEventArgs e)
        {
            ProgressSlider.IsEnabled = true;
            ProgressSlider.Maximum = e.TimeSpan.Ticks;
        }

        private void Mp3Player_TrackChanged(object sender, TrackEventArgs e)
        {
            NowPlayingTitleTextBlock.Text = _sampleDictionary[_mp3Player.CurrentPlaylistItem.Value].Title;
            NowPlayingArtistTextBlock.Text = _sampleDictionary[_mp3Player.CurrentPlaylistItem.Value].Artists;
        }

        private void Mp3Player_VolumeChanged(object sender, VolumeEventArgs e)
        {
            VolumeSlider.IsEnabled = true;
            VolumeSlider.Value = e.Volume;
        }

        private void MuteVolume_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _mp3Player != null;
        }

        private void MuteVolume_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _mp3Player.ToggleMute();
        }

        private void NextTrack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_mp3Player != null)
            {
                _mp3Player.SkipForward();
                _mp3Player.Play();

                PlayPauseToggleButton.IsChecked = true;
            }
        }

        private void PreviousTrack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_mp3Player != null)
            {
                _mp3Player.SkipBack();
                _mp3Player.Play();

                PlayPauseToggleButton.IsChecked = true;
            }
        }

        private void ProgressSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            _mp3Player.CurrentTime = new TimeSpan((long)ProgressSlider.Value);
            _mp3Player.Play();
        }

        private void ProgressSlider_DragStarted(object sender, RoutedEventArgs e)
        {
            _mp3Player.Stop();
        }

        private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ProgressStatus.Text = TimeSpan.FromTicks((long)ProgressSlider.Value).ToString(@"hh\:mm\:ss");
        }

        private void TogglePlayPause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _mp3Player != null;
        }

        private void TogglePlayPause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_mp3Player.PlaybackState == PlaybackState.Playing)
            {
                _mp3Player.Pause();
                PlayPauseToggleButton.IsChecked = false;
            }
            else
            {
                _mp3Player.Play();
                PlayPauseToggleButton.IsChecked = true;
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_mp3Player == null)
            {
                return;
            }

            _mp3Player.Volume = (float)e.NewValue;
        }

        private void CompositionEditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            Application.Current.Properties["SelectedComposition"] = ((Composition)menuItem.DataContext).CompositionId;

            NavigationService.Navigate(new Uri(@"pack://application:,,,/Views/CompositionEditPage.xaml", UriKind.Absolute));
        }
    }
}