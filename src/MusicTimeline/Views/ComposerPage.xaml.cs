using NathanHarrenstein.MusicDB;
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class ComposerPage : Page
    {
        private LinkedListNode<object> _currentItem;
        private DataProvider _dataProvider;
        private bool _isDraggingProgressSlider;
        private WaveOut _player;
        private FlacReader _stream;
        private DispatcherTimer _timer;
        private float _volume;

        public ComposerPage()
        {
            InitializeComponent();

            _dataProvider = new DataProvider();

            var composerName = Application.Current.Properties["SelectedComposer"] as string;
            var composer = _dataProvider.Composers.FirstOrDefault(c => c.Name == composerName);

            if (composer != null)
            {
                LoadComposer(composer);
            }

            PlaySamples(composer);
        }

        ~ComposerPage()
        {
            if (_stream != null)
            {
                _stream.Dispose();
            }

            if (_player != null)
            {
                _player.Stop();
                _player.Dispose();
            }

            _dataProvider.Dispose();
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

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (_isDraggingProgressSlider)
            {
                return;
            }

            if (_player.PlaybackState == PlaybackState.Stopped)
            {
                _timer.Stop();
                _stream.Seek(0, SeekOrigin.Begin);
                ProgressSlider.Value = _stream.CurrentTime.Ticks;
                PlayPauseToggleButton.IsChecked = false;

                return;
            }

            if (_player.PlaybackState == PlaybackState.Paused)
            {
                return;
            }

            ProgressSlider.Value = _stream.CurrentTime.Ticks;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();

            e.Handled = true;
        }

        private void ComposerButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var composer = (Composer)button.DataContext;

            LoadComposer(composer);
        }

        private string GetNowPlayingArtist(object item)
        {
            if (item is Sample)
            {
                return ((Sample)item).Artists;
            }
            else if (item is LibraryDB.Recording)
            {
                return string.Join(", ", _dataProvider.Recordings.Find(((LibraryDB.Recording)item).MDBID).Performers.Select(p => p.Name));
            }

            return null;
        }

        private string GetNowPlayingTitle(object item)
        {
            if (item is Sample)
            {
                return ((Sample)item).Title;
            }
            else if (item is LibraryDB.Recording)
            {
                var recording = _dataProvider.Recordings.Find(((LibraryDB.Recording)item).MDBID);

                if (recording.CompositionCollection != null)
                {
                    return recording.CompositionCollection.Name;
                }
                else if (recording.Composition != null)
                {
                    return recording.Composition.Name;
                }
                else
                {
                    return $"{recording.Composition.Name}: {recording.Movement.Number}. {recording.Movement.Name}";
                }
            }

            return null;
        }

        private void Load()
        {
            byte[] bytes = null;

            if (_currentItem.Value is Sample)
            {
                bytes = ((Sample)_currentItem.Value).Audio;
            }
            else if (_currentItem.Value is LibraryDB.Recording)
            {
                bytes = File.ReadAllBytes(((LibraryDB.Recording)_currentItem.Value).FilePath);
            }

            if (_stream != null)
            {
                _stream.Dispose();
            }

            _stream = new FlacReader(new MemoryStream(bytes));
            _player = new WaveOut { DesiredLatency = 200 };
            _player.Init(_stream);

            ProgressSlider.Maximum = _stream.TotalTime.Ticks;

            NowPlayingTitleTextBlock.Text = GetNowPlayingTitle(_currentItem.Value);
            NowPlayingArtistTextBlock.Text = GetNowPlayingArtist(_currentItem.Value);
        }

        private void LoadComposer(Composer composer)
        {
            var section = (Section)XamlReader.Parse(composer.Biography);

            var defaultFontSize = TextElement.GetFontSize(BiographyFlowDocumentScrollViewer);
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

            var flowdocument = new FlowDocument();
            flowdocument.FontFamily = new FontFamily("Cambria");
            flowdocument.TextAlignment = TextAlignment.Left;
            flowdocument.Blocks.Add(section);

            BiographyFlowDocumentScrollViewer.Document = flowdocument;

            BornTextBlock.Text = GetBorn(composer);
            ComposerImagesListBox.ItemsSource = composer.ComposerImages;
            ComposerNameTextBlock.Text = NameUtility.ToFirstLast(composer.Name);
            DiedTextBlock.Text = GetDied(composer);
            ComposerFlagsItemsControl.ItemsSource = composer.Nationalities;
            InfluencedItemsControl.ItemsSource = composer.Influenced;
            InfluencedItemsControl.Visibility = InfluencedTextBlock.Visibility = composer.Influenced.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            InfluencesItemsControl.ItemsSource = composer.Influences;
            InfluencesItemsControl.Visibility = InfluencesTextBlock.Visibility = composer.Influences.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            LinksItemControl.ItemsSource = composer.ComposerLinks;
            LinksItemControl.Visibility = LinksTextBlock.Visibility = composer.ComposerLinks.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            TreeView.Children = composer.CompositionCollections
                .Select<CompositionCollection, Controls.TreeViewItem>(cc => CompositionCollectionTreeViewItemProvider.GetCompositionCollectionTreeViewItem(cc, null))
                .Concat(composer.Compositions
                    .Select(c => CompositionTreeViewItemProvider.GetCompositionTreeViewItem(c, null)))
                .OrderBy(tvi => tvi.Header);
            ComposerImagesListBox.SelectedIndex = 0;
        }

        private void MuteVolume_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _player != null;
        }

        private void MuteVolume_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_player.Volume > 0)
            {
                _volume = _player.Volume;

                _player.Volume = 0;
            }
            else
            {
                _player.Volume = _volume;
            }
        }

        private void NextTrack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_currentItem != null)
            {
                e.CanExecute = _currentItem.Next != null;
            }
        }

        private void NextTrack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_currentItem.Next == null)
            {
                return;
            }

            if (_player != null)
            {
                _player.Dispose();
            }

            if (_stream != null)
            {
                _stream.Dispose();
            }

            _currentItem = _currentItem.Next;

            Load();
        }

        private void PlaySamples(Composer composer)
        {
            _currentItem = new LinkedList<object>(composer.Samples).First;

            if (_currentItem == null)
            {
                return;
            }

            Load();

            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(1);
            _timer.Tick += _timer_Tick;
            _timer.Start();

            _player.Play();
            PlayPauseToggleButton.IsChecked = true;
        }

        private void PreviousTrack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_currentItem != null)
            {
                e.CanExecute = _currentItem.Previous != null;
            }
        }

        private void PreviousTrack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_currentItem.Previous == null)
            {
                return;
            }

            if (_player != null)
            {
                _player.Dispose();
            }

            if (_stream != null)
            {
                _stream.Dispose();
            }

            _currentItem = _currentItem.Previous;

            Load();
        }

        private void ProgressSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            _isDraggingProgressSlider = false;
            _stream.CurrentTime = new TimeSpan((long)ProgressSlider.Value);
        }

        private void ProgressSlider_DragStarted(object sender, DragStartedEventArgs e)
        {
            _isDraggingProgressSlider = true;
        }

        private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ProgressStatus.Text = TimeSpan.FromTicks((long)ProgressSlider.Value).ToString(@"hh\:mm\:ss");
        }

        private void TogglePlayPause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _player != null;
        }

        private void TogglePlayPause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_player.PlaybackState == PlaybackState.Playing)
            {
                _player.Pause();
                PlayPauseToggleButton.IsChecked = false;

                return;
            }
            else if (_player.PlaybackState == PlaybackState.Stopped)
            {
                _timer = new DispatcherTimer();
                _timer.Interval = new TimeSpan(1);
                _timer.Tick += _timer_Tick;
                _timer.Start();
            }

            _player.Play();
            PlayPauseToggleButton.IsChecked = true;
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_player == null)
            {
                return;
            }

            _player.Volume = (float)e.NewValue;
        }
    }
}