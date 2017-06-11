using MusicTimelineWebApi.Models;
using NathanHarrenstein.MusicTimeline.Controls;
using NathanHarrenstein.MusicTimeline.Converters;
using NathanHarrenstein.MusicTimeline.Input;
using NathanHarrenstein.MusicTimeline.Utilities;
using NathanHarrenstein.MusicTimeline.ViewModels;
using NathanHarrenstein.Timeline;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Services.Client;
using System.EDTF;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class TimelinePage : Page, IDisposable
    {
        public static readonly DependencyProperty ChangeResolutionCommandProperty = DependencyProperty.Register("ChangeResolutionCommand", typeof(ICommand), typeof(TimelinePage));
        public static readonly DependencyProperty CloseCommandProperty = DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(TimelinePage));
        public static readonly DependencyProperty FullScreenCommandProperty = DependencyProperty.Register("FullScreenCommand", typeof(ICommand), typeof(TimelinePage));
        public static readonly DependencyProperty GoToCommandProperty = DependencyProperty.Register("GoToCommand", typeof(ICommand), typeof(TimelinePage));
        public static readonly DependencyProperty RebuildThumbnailCacheCommandProperty = DependencyProperty.Register("RebuildThumbnailCacheCommand", typeof(ICommand), typeof(TimelinePage));

        private static readonly Dictionary<string, SolidColorBrush> eraBrushes = new Dictionary<string, SolidColorBrush> {
            { "Medieval", new SolidColorBrush(Color.FromRgb(155, 128, 181)) }, // 153, 153, 153
            { "Renaissance", new SolidColorBrush(Color.FromRgb(93, 138, 169)) }, // 155, 128, 181
            { "Baroque", new SolidColorBrush(Color.FromRgb(0, 128, 129)) }, // 204, 77, 77
            { "Classical", new SolidColorBrush(Color.FromRgb(69, 168, 90)) }, // 51, 151, 193
            { "Romantic", new SolidColorBrush(Color.FromRgb(204, 174, 87)) }, // 69, 168, 90
            { "20th Century", new SolidColorBrush(Color.FromRgb(249, 130, 40)) }, //160, 118, 88
            { "21st Century", new SolidColorBrush(Color.FromRgb(204, 77, 77)) } // 74, 142, 165
        };

        private bool isDisposed;
        private IEnumerable<Era> eras;
        private IEnumerable<Composer> composers;

        public TimelinePage()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                CloseCommand = new DelegateCommand(Exit);
                GoToCommand = new DelegateCommand(GoToEra);
                RebuildThumbnailCacheCommand = new DelegateCommand(RebuildThumbnailCache);
                FullScreenCommand = new DelegateCommand(ExpandToFullScreen);
                ChangeResolutionCommand = new DelegateCommand(ChangeResolution);

                var startDate = new ExtendedDateTime(476, 1, 1);
                var endDate = ExtendedDateTime.Now;

                timeline.Ruler = new TimeRuler();
                timeline.Ruler.TimeRulerUnit = TimeRulerUnit.Day;
                timeline.Ruler.TimeUnitWidth = 0.04109589041;
                timeline.Resolution = TimeResolution.Decade;               
                timeline.Dates = new ExtendedDateTimeInterval(startDate, endDate);

                Loaded += TimelinePage_Loaded;
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

        private async void TimelinePage_Loaded(object sender, RoutedEventArgs e)
        {
            eras = await WebApiInterface.GetErasAsync();
            composers = await WebApiInterface.GetComposersAsync();

            CreateComposerViewModels(composers);
            CreateEraViewModels(eras);
            ResetPosition();
            LoadSidebarItems();
        }

        private void LoadSidebarItems()
        {
            var composersItemsControl = (ItemsControl)timeline.Template.FindName("composersItemsControl", timeline);
            composersItemsControl.ItemsSource = composers.OrderBy(c => c.Name).ToList();
        }

        private void CreateComposerViewModels(IEnumerable<Composer> composers)
        {
            var backgroundColor = Color.FromArgb(0x44, 0x00, 0x00, 0x00);
            var background = new SolidColorBrush(backgroundColor);
            var foreground = Brushes.White;
            var orderedComposers = composers.OrderBy(c => ExtendedDateTimeInterval.Parse(c.Dates).Earliest());

            foreach (var composer in orderedComposers)
            {
                var name = NameUtility.ToFirstLast(composer.Name);
                var dates = ExtendedDateTimeInterval.Parse(composer.Dates);
                var clickCommand = GetClickCommand(composer.Id);
                var thumbnail = WebApiInterface.ApiRoot + composer.Thumbnail;
                var composerEventViewModel = new ComposerEventViewModel(name, dates, composer, background, foreground, clickCommand, thumbnail);

                timeline.Events.Add(composerEventViewModel);
            }
        }

        private DelegateCommand GetClickCommand(int composerId)
        {
            var navigateToComposerPage = new Action<object>(o =>
            {
                Application.Current.Properties["SelectedComposer"] = composerId;
                Application.Current.Properties["HorizontalOffset"] = timeline.HorizontalOffset;
                Application.Current.Properties["VerticalOffset"] = timeline.VerticalOffset;

                var composerPageUri = new Uri("pack://application:,,,/Views/ComposerPage.xaml");
                var navigationWindow = (NavigationWindow)Application.Current.MainWindow;
                navigationWindow.Navigate(composerPageUri);
            });

            return new DelegateCommand(navigateToComposerPage);
        }

        private void CreateEraViewModels(IEnumerable<Era> eras)
        {
            var gradientStops = new GradientStopCollection();
            var timelineTimeSpan = timeline.Dates.Span();
            var timelineTotalDays = timelineTimeSpan.TotalDays;

            ComposerEraViewModel previousEraViewModel = null;

            foreach (var era in eras)
            {
                var name = era.Name;
                var dates = ExtendedDateTimeInterval.Parse(era.Dates);
                var background = eraBrushes[era.Name];
                var foreground = Brushes.White;

                var composerEraViewModel = new ComposerEraViewModel(name, dates, background, foreground);

                var eraGradientStop = CreateEraGradientStop(timelineTotalDays, background, composerEraViewModel);
                gradientStops.Add(eraGradientStop);

                if (previousEraViewModel == null)
                {
                    timeline.Eras.Add(composerEraViewModel);
                    previousEraViewModel = composerEraViewModel;
                    continue;
                }

                var era1StartDate = previousEraViewModel.Dates.Earliest();
                var era1EndDate = previousEraViewModel.Dates.Latest();

                var era2StartDate = composerEraViewModel.Dates.Earliest();
                var era2EndDate = composerEraViewModel.Dates.Latest();

                // If there is an overlap between eras, create transition eras.
                if (era2StartDate < era1EndDate)
                {
                    var previousEraBrush = (SolidColorBrush)previousEraViewModel.Background;
                    var startGradientStop = new GradientStop(previousEraBrush.Color, 0);
                    var endGradientStop = new GradientStop(background.Color, 1);

                    var transitionBackground = new LinearGradientBrush();
                    transitionBackground.StartPoint = new Point(0, 0.5);
                    transitionBackground.EndPoint = new Point(1, 0.5);                   
                    transitionBackground.GradientStops.Add(startGradientStop);
                    transitionBackground.GradientStops.Add(endGradientStop);
                    var transitionForeground = Brushes.White;
                    var transitionDates = new ExtendedDateTimeInterval(era2StartDate, era1EndDate);

                    var transitionEra = new ComposerEraViewModel(null, transitionDates, transitionBackground, transitionForeground);

                    previousEraViewModel.Dates = new ExtendedDateTimeInterval(era1StartDate, era2StartDate);
                    composerEraViewModel.Dates = new ExtendedDateTimeInterval(era1EndDate, era2EndDate);

                    timeline.Eras.Add(transitionEra);
                }

                timeline.Eras.Add(composerEraViewModel);

                previousEraViewModel = composerEraViewModel;
            }

            timeline.GradientStops = gradientStops;
        }

        private GradientStop CreateEraGradientStop(double timelineTotalDays, SolidColorBrush background, ComposerEraViewModel composerEraViewModel)
        {
            var backgroundColor = background.Color;
            var eraTimeOffset = composerEraViewModel.Dates.Earliest() - timeline.Dates.Earliest();
            var eraDayOffset = eraTimeOffset.TotalDays;
            var eraTimeSpan = composerEraViewModel.Dates.Span();
            var eraDaySpan = eraTimeSpan.TotalDays;
            var halfEraDaySpan = eraDaySpan * 0.5;
            var eraGradientOffset = (eraDayOffset + halfEraDaySpan) / timelineTotalDays;

            return new GradientStop(backgroundColor, eraGradientOffset);   
        }

        private void ResetPosition()
        {
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

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
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

        private void composerButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var composer = button.DataContext as Composer;

            if (composer == null)
            {
                return;
            }

            var dates = ExtendedDateTimeInterval.Parse(composer.Dates);
            var startDate = dates.Earliest();
            var endDate = dates.Latest();

            var horizontalOffset = timeline.HorizontalOffset;
            var verticalOffset = timeline.VerticalOffset;

            var composerindex = 0;

            foreach (ComposerEventViewModel ev in timeline.Events)
            {
                if (ev.Composer == composer)
                {
                    break;
                }

                composerindex++;
            }

            timeline.HorizontalOffset = timeline.Ruler.ToPixels(timeline.Dates.Earliest(), startDate);
            timeline.VerticalOffset = composerindex * (timeline.EventHeight + timeline.EventSpacing);
        }

        private void composersHeader_ButtonClick(object sender, RoutedEventArgs e)
        {
            var composersEditPanel = timeline.Template.FindName("composersEditPanel", timeline) as FrameworkElement;
            var composersScrollViewer = timeline.Template.FindName("composersScrollViewer", timeline) as ScrollViewer;
            var composersListBox = timeline.Template.FindName("composersListBox", timeline) as ListBox;

            composersEditPanel.Visibility = Visibility.Visible;
            composersScrollViewer.Visibility = Visibility.Collapsed;

            composersListBox.ItemsSource = composers.OrderBy(c => c.Name).ToList();
            composersListBox.Focus();
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
            var eraQuery = eras.First(e => e.Name == (string)obj);

            var composersSortedByDate = composers
                .Select(c => Tuple.Create(ExtendedDateTimeInterval.Parse(c.Dates).Earliest(), c))
                .OrderBy(t => t.Item1)
                .ToArray();

            var index = 0;

            foreach (var composer in composersSortedByDate)
            {
                if (composer.Item2.Eras.Contains(eraQuery.Id))
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
            //var composerToBitmapImageConverter = new ComposerToBitmapImageConverter();
            //composerToBitmapImageConverter.ClearFileCache();

            //Application.Current.Properties["HorizontalOffset"] = timeline.HorizontalOffset;
            //Application.Current.Properties["VerticalOffset"] = timeline.VerticalOffset;

            //NavigationService.Refresh();
        }

        

        private void composerDatesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //var textBox = (TextBox)sender;
            //var composer = (Composer)textBox.DataContext;
            //var entityDescriptor = App.ClassicalMusicContext.GetEntityDescriptor(composer);

            //if (entityDescriptor.State != EntityStates.Added)
            //{
            //    App.ClassicalMusicContext.ChangeState(composer, EntityStates.Modified);
            //}
        }
    }
}