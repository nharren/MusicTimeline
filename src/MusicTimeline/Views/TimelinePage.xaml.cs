using NathanHarrenstein.MusicTimeline.ClassicalMusicDb;
using NathanHarrenstein.MusicTimeline.Builders;
using NathanHarrenstein.MusicTimeline.Converters;
using NathanHarrenstein.MusicTimeline.Input;
using NathanHarrenstein.Timeline;
using System;
using System.ComponentModel;
using System.Data.Entity;
using System.EDTF;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Data.Services.Client;
using System.Collections.Generic;
using NathanHarrenstein.MusicTimeline.ViewModels;
using NathanHarrenstein.MusicTimeline.Scrapers;
using HTMLConverter;
using NathanHarrenstein.MusicTimeline.Utilities;
using System.Windows.Markup;
using System.Windows.Documents;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Net;
using System.Security;
using System.Timers;
using System.Reflection;
using System.Diagnostics;

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class TimelinePage : Page, IDisposable
    {
        public static readonly DependencyProperty ChangeResolutionCommandProperty = DependencyProperty.Register("ChangeResolutionCommand", typeof(ICommand), typeof(TimelinePage));
        public static readonly DependencyProperty CloseCommandProperty = DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(TimelinePage));
        public static readonly DependencyProperty FullScreenCommandProperty = DependencyProperty.Register("FullScreenCommand", typeof(ICommand), typeof(TimelinePage));
        public static readonly DependencyProperty GoToCommandProperty = DependencyProperty.Register("GoToCommand", typeof(ICommand), typeof(TimelinePage));
        public static readonly DependencyProperty RebuildThumbnailCacheCommandProperty = DependencyProperty.Register("RebuildThumbnailCacheCommand", typeof(ICommand), typeof(TimelinePage));

        private ClassicalMusicContext classicalMusicContext;
        private bool isDisposed;

        private DelegateCommand GetCommand(int composerId)
        {
            Action<object> command = o =>
            {
                Application.Current.Properties["SelectedComposer"] = composerId;
                Application.Current.Properties["HorizontalOffset"] = timeline.HorizontalOffset;
                Application.Current.Properties["VerticalOffset"] = timeline.VerticalOffset;

                ((NavigationWindow)Application.Current.MainWindow).Navigate(new Uri("pack://application:,,,/Views/ComposerPage.xaml"));
            };

            return new DelegateCommand(command);
        }

        private static readonly Dictionary<string, SolidColorBrush> eraBrushes = new Dictionary<string, SolidColorBrush> {
            { "Medieval", new SolidColorBrush(Color.FromRgb(153, 153, 153)) },
            { "Renaissance", new SolidColorBrush(Color.FromRgb(155, 128, 181)) },
            { "Baroque", new SolidColorBrush(Color.FromRgb(204, 77, 77)) },
            { "Classical", new SolidColorBrush(Color.FromRgb(51, 151, 193)) },
            { "Romantic", new SolidColorBrush(Color.FromRgb(69, 168, 90)) },
            { "20th Century", new SolidColorBrush(Color.FromRgb(160, 118, 88)) },
            { "21st Century", new SolidColorBrush(Color.FromRgb(74, 142, 165)) }
        };

#if TRACE
        private Stopwatch performanceStopwatch = new Stopwatch();
#endif

        public TimelinePage()
        {

#if TRACE
            performanceStopwatch.Start();
#endif

            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                classicalMusicContext = new ClassicalMusicContext(new Uri("http://www.harrenstein.com/ClassicalMusic/ClassicalMusic.svc"));

                var composersUri = new Uri($"http://www.harrenstein.com/ClassicalMusic/ClassicalMusic.svc/Composers?$expand=Eras,Nationalities,BirthLocation,DeathLocation");
                var composers = classicalMusicContext.Execute<Composer>(composersUri, "GET", false);

#if TRACE
                Console.WriteLine($"Data Loaded: {performanceStopwatch.Elapsed.TotalSeconds}");
#endif

                var backgroundBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#44000000"));

                //timeline.Events = composers
                //    .Select(composer => new ComposerEventViewModel(
                //        NameUtility.ToFirstLast(composer.Name),
                //        ExtendedDateTimeInterval.Parse(composer.Dates),
                //        composer,
                //        backgroundBrush,
                //        Brushes.White,
                //        //composerEras,
                //        GetCommand(composer.ComposerId)))
                //   .OrderBy(model => model.Dates.Earliest())
                //   .ToList<ITimelineEvent>();

                foreach (var composer in composers)
                {
                    var composerEvent = new ComposerEventViewModel(
                        NameUtility.ToFirstLast(composer.Name),
                        ExtendedDateTimeInterval.Parse(composer.Dates),
                        composer,
                        backgroundBrush,
                        Brushes.White,
                        //composerEras,
                        GetCommand(composer.ComposerId));

                    timeline.Events.Add(composerEvent);
                }

                CloseCommand = new DelegateCommand(Exit);
                GoToCommand = new DelegateCommand(GoToEra);
                RebuildThumbnailCacheCommand = new DelegateCommand(RebuildThumbnailCache);
                FullScreenCommand = new DelegateCommand(ExpandToFullScreen);
                ChangeResolutionCommand = new DelegateCommand(ChangeResolution);

                timeline.Ruler = new TimeRuler();
                timeline.Ruler.TimeRulerUnit = TimeRulerUnit.Day;
                timeline.Ruler.TimeUnitWidth = 0.04109589041;
                timeline.Resolution = TimeResolution.Decade;
                timeline.Dates = new ExtendedDateTimeInterval(new ExtendedDateTime(476, 1, 1), ExtendedDateTime.Now);

                var gradientStops = new GradientStopCollection();
                var totalDays = timeline.Dates.Span().TotalDays;

                ComposerEraViewModel previousEraViewModel = null;

                foreach (var era in classicalMusicContext.Eras)
                {
                    var musicEra = new ComposerEraViewModel(era.Name, ExtendedDateTimeInterval.Parse(era.Dates), eraBrushes[era.Name], Brushes.White);

                    gradientStops.Add(new GradientStop(((SolidColorBrush)musicEra.Background).Color, ((musicEra.Dates.Earliest() - timeline.Dates.Earliest()).TotalDays + musicEra.Dates.Span().TotalDays * 0.5) / totalDays));

                    if (previousEraViewModel == null)
                    {
                        timeline.Eras.Add(musicEra);

                        previousEraViewModel = musicEra;

                        continue;
                    }

                    var era1StartDate = previousEraViewModel.Dates.Earliest();
                    var era1EndDate = previousEraViewModel.Dates.Latest();

                    var era2StartDate = musicEra.Dates.Earliest();
                    var era2EndDate = musicEra.Dates.Latest();

                    if (era2StartDate < era1EndDate)
                    {
                        var transitionBrush = new LinearGradientBrush();
                        transitionBrush.StartPoint = new Point(0, 0.5);
                        transitionBrush.EndPoint = new Point(1, 0.5);
                        transitionBrush.GradientStops.Add(new GradientStop(((SolidColorBrush)previousEraViewModel.Background).Color, 0));
                        transitionBrush.GradientStops.Add(new GradientStop(((SolidColorBrush)musicEra.Background).Color, 1));

                        var transitionEra = new ComposerEraViewModel(null, new ExtendedDateTimeInterval(era2StartDate, era1EndDate), transitionBrush, Brushes.White);

                        previousEraViewModel.Dates = new ExtendedDateTimeInterval(era1StartDate, era2StartDate);
                        musicEra.Dates = new ExtendedDateTimeInterval(era1EndDate, era2EndDate);

                        timeline.Eras.Add(transitionEra);
                    }

                    timeline.Eras.Add(musicEra);

                    previousEraViewModel = musicEra;
                }

                timeline.GradientStops = gradientStops;

                

                UpdateLayout();
            }

#if TRACE
            Loaded += TimelinePage_Loaded;
            
            Console.WriteLine($"TimelinePage Initialized: {performanceStopwatch.Elapsed.TotalSeconds}");
#endif

        }

#if TRACE
        private void TimelinePage_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"TimelinePage Loaded: {performanceStopwatch.Elapsed.TotalSeconds}");
        }
#endif

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

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(@"pack://application:,,,/Views/ComposersEditPage.xaml", UriKind.Absolute));
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
            var eraQuery = classicalMusicContext.Eras.First(e => e.Name == (string)obj);

            var composersSortedByDate = classicalMusicContext.Composers.ToArray()
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

        //private void TimelinePage_Loaded(object sender, RoutedEventArgs e)
        //{


        //    var eras = classicalMusicContext.Eras;
        //    timeline.Eras = ComposerEraViewModelBuilder.Build(eras);

        //    //try
        //    //{
        //    //    var d = eras.BeginExecute(OnErasQueryComplete, null);
        //    //}
        //    //catch (DataServiceQueryException ex)
        //    //{
        //    //    MessageBox.Show(ex.Message);

        //    //    App.Logger.Log(ex);
        //    //}

        //    var horizontalOffset = Application.Current.Properties["HorizontalOffset"] as double?;

        //    if (horizontalOffset != null)
        //    {
        //        timeline.HorizontalOffset = horizontalOffset.Value;
        //    }

        //    var verticalOffset = Application.Current.Properties["VerticalOffset"] as double?;

        //    if (verticalOffset != null)
        //    {
        //        timeline.VerticalOffset = verticalOffset.Value;
        //    }
        //}

        //private List<ComposerEraViewModel> composerEraViewModels;

        //private void OnErasQueryComplete(IAsyncResult result)
        //{ 
        //    var query = result.AsyncState as DataServiceQuery<Era>;

        //    var eraList = query.EndExecute(result).ToList();

        //    Dispatcher.Invoke(() =>
        //    {
        //        composerEraViewModels = ComposerEraViewModelBuilder.Build(eraList);

        //        timeline.Eras = composerEraViewModels;
        //    });

        //    var composersQuery = classicalMusicContext.Composers
        //        .Expand(c => c.Eras)
        //        .Expand(c => c.Nationalities);

        //    try
        //    {
        //        composersQuery.BeginExecute(OnComposersQueryComplete, composersQuery);
        //    }
        //    catch (DataServiceQueryException ex)
        //    {
        //        throw new ApplicationException("An error occurred during query execution.", ex);
        //    }
        //}

        //private void OnComposersQueryComplete(IAsyncResult result)
        //{
        //    var query = result.AsyncState as DataServiceQuery<Composer>;
        //    var composerList = query.EndExecute(result).ToList();

        //    Dispatcher.Invoke(() =>
        //   {                
        //        timeline.Events = ComposerEventViewModelBuilder.Build(composerList, composerEraViewModels, timeline);
        //   });
        //}
    }
}