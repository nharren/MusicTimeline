using NathanHarrenstein.MusicTimeline.ClassicalMusicDb;
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
using System.Collections.ObjectModel;
using NathanHarrenstein.MusicTimeline.Extensions;
using System.Collections;
using System.Data.Services.Client;

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
            _classicalMusicContext = new ClassicalMusicContext(new Uri("http://www.harrenstein.com/ClassicalMusic/ClassicalMusic.svc"));
            _classicalMusicContext.MergeOption = MergeOption.OverwriteChanges;
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

                _mp3Player.Dispose();

                _isDisposed = true;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();

            e.Handled = true;
        }

        private async void ComposerPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadComposerAsync();
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _sampleDictionary.Clear();

            _mp3Player.Dispose();

            //NavigationService.Navigate(new Uri(@"pack://application:,,,/Views/ComposerEditPage.xaml", UriKind.Absolute));
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

            var composerUri = new Uri($"http://www.harrenstein.com/ClassicalMusic/ClassicalMusic.svc/Composers?$filter=ComposerId eq {composerId}&$expand=Details,Details/BirthLocation,Details/DeathLocation,Nationalities,Influences,Influenced,Links");
            var composerQuery = await _classicalMusicContext.ExecuteAsync<Composer>(composerUri, null);

            _composer = composerQuery.First();

            ComposerNameTextBlock.Text = NameUtility.ToFirstLast(_composer.Name);

            if (_composer.Details != null)
            {
                if (_composer.Details.BirthLocation != null)
                {
                    BornTextBlock.Text = $"{ExtendedDateTimeInterval.Parse(_composer.Dates).Start}; {_composer.Details.BirthLocation.Name}";
                }
                else
                {
                    BornTextBlock.Text = ExtendedDateTimeInterval.Parse(_composer.Dates).Start.ToString();
                }

                var deathLocation = _composer.Details.DeathLocation;

                if (deathLocation != null)
                {
                    DiedTextBlock.Text = $"{ExtendedDateTimeInterval.Parse(_composer.Dates).End}; {deathLocation.Name}";
                }
                else
                {
                    DiedTextBlock.Text = ExtendedDateTimeInterval.Parse(_composer.Dates).End.ToString();
                }

                if (!string.IsNullOrEmpty(_composer.Details.Biography))
                {
                    var parserContext = new ParserContext();
                    parserContext.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
                    parserContext.XmlSpace = "preserve";

                    var flowDocument = (FlowDocument)XamlReader.Load(new MemoryStream(Encoding.UTF8.GetBytes(_composer.Details.Biography)), parserContext);

                    if (flowDocument != null)
                    {
                        flowDocument.FontFamily = new FontFamily("Cambria");
                        flowDocument.PagePadding = new Thickness(0, 5, 0, 0);
                        flowDocument.TextAlignment = TextAlignment.Left;

                        FlowDocumentScrollViewer.Document = flowDocument;
                    }
                }
            }

            ComposerFlagsItemsControl.ItemsSource = _composer.Nationalities;

            InfluencesItemsControl.ItemsSource = _composer.Influences;

            var influencesVisibility = _composer.Influences.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

            InfluencesItemsControl.Visibility = influencesVisibility;
            InfluencesTextBlock.Visibility = influencesVisibility;
            InfluencesUnderline.Visibility = influencesVisibility;

            InfluencedItemsControl.ItemsSource = _composer.Influenced;

            var influencedVisibility = _composer.Influenced.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

            InfluencedItemsControl.Visibility = influencedVisibility;
            InfluencedTextBlock.Visibility = influencedVisibility;
            InfluencedUnderline.Visibility = influencedVisibility;

            LinksItemControl.ItemsSource = _composer.Links;

            var linksVisibility = _composer.Links.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

            LinksItemControl.Visibility = linksVisibility;
            LinksTextBlock.Visibility = linksVisibility;
            LinksUnderline.Visibility = linksVisibility;

            var composerImagesQuery = await _classicalMusicContext.LoadPropertyAsync(_composer, "ComposerImages");
            var composerImages = composerImagesQuery.Cast<ComposerImage>().ToList();

            if (composerImages.Count == 0)
            {
                ComposerImagesListBox.ItemsSource = new ComposerImage[] { GetDefaultComposerImage() };
            }
            else
            {
                ComposerImagesListBox.ItemsSource = composerImages;
            }

            ComposerImagesListBox.SelectedIndex = 0;

            var compositionsUri = new Uri($"http://www.harrenstein.com/ClassicalMusic/ClassicalMusic.svc/Compositions?$filter=Composers/any(d:d/Name eq '{_composer.Name}')&$expand=Genre,Key,Movements");

            var genresQuery = await _classicalMusicContext.ExecuteAsync<Composition>(compositionsUri, null);
            var genres = genresQuery.GroupBy(c => c.Genre == null ? "Unknown" : c.Genre.Name)
                .OrderBy(s => s.Key);

            TreeView.SetBinding(ItemsControl.ItemsSourceProperty, BindingBuilder.Build(genres));

            var samplesQuery = await _classicalMusicContext.LoadPropertyAsync(_composer, "Samples");
            var samples = samplesQuery.Cast<Sample>();

            foreach (var sample in samples)
            {
                var mp3FileReader = new Mp3FileReader(new MemoryStream(sample.Bytes));

                _sampleDictionary[mp3FileReader] = sample;
                _mp3Player.AddToPlaylist(mp3FileReader);

                if (_sampleDictionary.Count == 1)
                {
                    _mp3Player.Play();
                }
            }

            ProgressBar.Visibility = Visibility.Collapsed;
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

            //NavigationService.Navigate(new Uri(@"pack://application:,,,/Views/CompositionEditPage.xaml", UriKind.Absolute));
        }
    }
}