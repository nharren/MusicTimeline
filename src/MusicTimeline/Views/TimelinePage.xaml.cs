using NathanHarrenstein.ClassicalMusicDb;
using NathanHarrenstein.MusicTimeline.Builders;
using NathanHarrenstein.MusicTimeline.Converters;
using NathanHarrenstein.MusicTimeline.Input;
using NathanHarrenstein.Timeline;
using System;
using System.ComponentModel;
using System.EDTF;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class TimelinePage : Page, IDisposable
    {
        public static readonly DependencyProperty ChangeResolutionCommandProperty = DependencyProperty.Register("ChangeResolutionCommand", typeof(ICommand), typeof(TimelinePage));
        public static readonly DependencyProperty CloseCommandProperty = DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(TimelinePage));
        public static readonly DependencyProperty FullScreenCommandProperty = DependencyProperty.Register("FullScreenCommand", typeof(ICommand), typeof(TimelinePage));
        public static readonly DependencyProperty GoToCommandProperty = DependencyProperty.Register("GoToCommand", typeof(ICommand), typeof(TimelinePage));
        public static readonly DependencyProperty ManageDataCommandProperty = DependencyProperty.Register("ManageDataCommand", typeof(ICommand), typeof(TimelinePage));
        public static readonly DependencyProperty RebuildThumbnailCacheCommandProperty = DependencyProperty.Register("RebuildThumbnailCacheCommand", typeof(ICommand), typeof(TimelinePage));

        private ClassicalMusicDbContext _classicalMusicDbContext;
        private bool _isDisposed = false;

        public TimelinePage()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                Initialize();
            }
        }

        ~TimelinePage()
        {
            Dispose(false);
        }

        public ICommand ChangeResolutionCommand
        {
            get
            {
                return (ICommand)GetValue(ChangeResolutionCommandProperty);
            }

            set
            {
                SetValue(ChangeResolutionCommandProperty, value);
            }
        }

        public ICommand CloseCommand
        {
            get
            {
                return (ICommand)GetValue(CloseCommandProperty);
            }

            set
            {
                SetValue(CloseCommandProperty, value);
            }
        }

        public ICommand FullScreenCommand
        {
            get
            {
                return (ICommand)GetValue(FullScreenCommandProperty);
            }

            set
            {
                SetValue(FullScreenCommandProperty, value);
            }
        }

        public ICommand GoToCommand
        {
            get
            {
                return (ICommand)GetValue(GoToCommandProperty);
            }

            set
            {
                SetValue(GoToCommandProperty, value);
            }
        }

        public ICommand ManageDataCommand
        {
            get
            {
                return (ICommand)GetValue(ManageDataCommandProperty);
            }

            set
            {
                SetValue(ManageDataCommandProperty, value);
            }
        }

        public ICommand RebuildThumbnailCacheCommand
        {
            get
            {
                return (ICommand)GetValue(RebuildThumbnailCacheCommandProperty);
            }

            set
            {
                SetValue(RebuildThumbnailCacheCommandProperty, value);
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public void Initialize()
        {
            ManageDataCommand = new DelegateCommand(NavigateToInputPage);
            CloseCommand = new DelegateCommand(Exit);
            GoToCommand = new DelegateCommand(GoToEra);
            RebuildThumbnailCacheCommand = new DelegateCommand(RebuildThumbnailCache);
            FullScreenCommand = new DelegateCommand(ExpandToFullScreen);
            ChangeResolutionCommand = new DelegateCommand(ChangeResolution);

            timeline.Dates = new ExtendedDateTimeInterval(new ExtendedDateTime(476, 1, 1), ExtendedDateTime.Now);
            timeline.Ruler = new TimeRuler();
            timeline.Ruler.TimeRulerUnit = TimeRulerUnit.Day;
            timeline.Ruler.TimeUnitWidth = 0.04109589041;
            timeline.Resolution = TimeResolution.Decade;
            timeline.Loaded += Timeline_Loaded;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                _classicalMusicDbContext.Dispose();

                _isDisposed = true;
            }
        }

        private void ChangeResolution(object obj)
        {
            var targetResolution = (TimeResolution)int.Parse((string)obj);

            switch (targetResolution)
            {
                case TimeResolution.Century:
                    timeline.Ruler.TimeUnitWidth = 0.01;
                    timeline.Ruler.TimeRulerUnit = TimeRulerUnit.Day;
                    break;

                case TimeResolution.Decade:
                    timeline.Ruler.TimeUnitWidth = 0.04109589041;
                    timeline.Ruler.TimeRulerUnit = TimeRulerUnit.Day;
                    break;

                case TimeResolution.Year:
                    timeline.Ruler.TimeUnitWidth = 0.41095890411;
                    timeline.Ruler.TimeRulerUnit = TimeRulerUnit.Day;
                    break;

                case TimeResolution.Month:
                    timeline.Ruler.TimeUnitWidth = 5;
                    timeline.Ruler.TimeRulerUnit = TimeRulerUnit.Day;
                    break;

                case TimeResolution.Day:
                    timeline.Ruler.TimeUnitWidth = 150;
                    timeline.Ruler.TimeRulerUnit = TimeRulerUnit.Day;
                    break;
            }

            timeline.Resolution = targetResolution;
            timeline.VerticalOffset = timeline.VerticalOffset;
        }

        private void Exit(object obj)
        {
            Application.Current.Shutdown();
        }

        private void ExpandToFullScreen(object obj)
        {
            if (Application.Current.MainWindow.WindowStyle == WindowStyle.None)
            {
                Application.Current.MainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                Application.Current.MainWindow.WindowStyle = WindowStyle.None;
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
        }

        private void GoToEra(object obj)
        {
            var eraQuery = _classicalMusicDbContext.Eras.First(e => e.Name == (string)obj);

            var composersSortedByDate = _classicalMusicDbContext.Composers.ToArray()
                .Select(c => Tuple.Create(ExtendedDateTimeInterval.Parse(c.Dates).Earliest(), c))
                .OrderBy(t => t.Item1);

            var index = 0;

            foreach (var composer in composersSortedByDate)
            {
                if (composer.Item2.Eras.Contains(eraQuery))
                {
                    timeline.HorizontalOffset = timeline.Ruler.ToPixels(timeline.Dates.Earliest(), composer.Item1);
                    timeline.VerticalOffset = index * (timeline.EventHeight + timeline.EventSpacing);

                    return;
                }

                index++;
            }

            timeline.HorizontalOffset = timeline.Ruler.ToPixels(timeline.Dates.Earliest(), ExtendedDateTimeInterval.Parse(eraQuery.Dates).Earliest());
        }

        private void NavigateToInputPage(object obj)
        {
            Application.Current.Properties["HorizontalOffset"] = timeline.HorizontalOffset;
            Application.Current.Properties["VerticalOffset"] = timeline.VerticalOffset;

            NavigationService.Navigate(new Uri(@"pack://application:,,,/Views/InputPage.xaml", UriKind.Absolute));
        }

        private void RebuildThumbnailCache(object obj)
        {
            ComposerToThumbnailConverter.ClearThumbnailCache();

            Application.Current.Properties["HorizontalOffset"] = timeline.HorizontalOffset;
            Application.Current.Properties["VerticalOffset"] = timeline.VerticalOffset;

            NavigationService.Refresh();
        }

        private async void Timeline_Loaded(object sender, RoutedEventArgs e)
        {
            _classicalMusicDbContext = new ClassicalMusicDbContext();

            var composerEraViewModels = await ComposerEraViewModelBuilder.BuildAsync(_classicalMusicDbContext);
            var composerEventViewModels = await ComposerEventViewModelBuilder.BuildAsync(composerEraViewModels, timeline, _classicalMusicDbContext);

            timeline.Eras = composerEraViewModels;
            timeline.Events = composerEventViewModels;

            var horizontalOffset = Application.Current.Properties["HorizontalOffset"] as double?;

            if (horizontalOffset != null)
            {
                timeline.HorizontalOffset = horizontalOffset.Value;
            }

            var verticalOffset = Application.Current.Properties["VerticalOffset"] as double?;

            if (verticalOffset != null)
            {
                timeline.VerticalOffset = verticalOffset.Value;
            }
        }
    }
}