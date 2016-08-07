using NathanHarrenstein.MusicTimeline.Controls;
using NathanHarrenstein.MusicTimeline.Converters;
using NathanHarrenstein.MusicTimeline.Data;
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

        public TimelinePage()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                var eras = new DataServiceCollection<Era>(App.ClassicalMusicContext, App.ClassicalMusicContext.Eras, TrackingMode.AutoChangeTracking, "Eras", null, null);

                var composerQuery = App.ClassicalMusicContext.Composers
                    .Expand(c => c.BirthLocation)
                    .Expand(c => c.DeathLocation)
                    .Expand(c => c.Nationalities)
                    .Expand(c => c.Images)
                    .Expand(c => c.Eras);

                var composers = new DataServiceCollection<Composer>(App.ClassicalMusicContext, composerQuery, TrackingMode.AutoChangeTracking, "Composers", null, null);
                var backgroundBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#44000000"));

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

                foreach (var era in eras)
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

            composersListBox.ItemsSource = App.ClassicalMusicContext.Composers.OrderBy(c => c.Name).ToList();

            composersListBox.Focus();
        }

        private void composersItemsControl_Loaded(object sender, RoutedEventArgs e)
        {
            var composersItemsControl = (ItemsControl)sender;
            composersItemsControl.ItemsSource = App.ClassicalMusicContext.Composers.OrderBy(c => c.Name).ToList();
        }

        private void composersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var composersListBox = (ListBox)sender;
            composersListBox.Items.Refresh();
        }

        private void composersToolbar_Adding(object sender, EventArgs e)
        {
            var composersListBox = timeline.Template.FindName("composersListBox", timeline) as ListBox;

            var composer = new Composer();
            composer.Name = "New Composer";

            App.ClassicalMusicContext.AddToComposers(composer);

            var composerList = (ICollection<Composer>)composersListBox.ItemsSource;

            composerList.Add(composer);
            composersListBox.Items.Refresh();

            composersListBox.SelectedItem = composer;

            composersListBox.ScrollIntoView(composer);
        }

        private void composersToolbar_Cancelling(object sender, EventArgs e)
        {
            var composersHeader = timeline.Template.FindName("composersHeader", timeline) as EditableHeaderPanel;
            var composersEditPanel = timeline.Template.FindName("composersEditPanel", timeline) as FrameworkElement;
            var composersScrollViewer = timeline.Template.FindName("composersScrollViewer", timeline) as ScrollViewer;
            var composersListBox = timeline.Template.FindName("composersListBox", timeline) as ListBox;

            composersListBox.ItemsSource = null;

            composersScrollViewer.Visibility = Visibility.Visible;
            composersEditPanel.Visibility = Visibility.Collapsed;

            composersHeader.IsEnabled = true;
        }

        private void composersToolbar_Removing(object sender, EventArgs e)
        {
            var composersListBox = timeline.Template.FindName("composersListBox", timeline) as ListBox;
            var selectedComposer = (Composer)composersListBox.SelectedItem;

            App.ClassicalMusicContext.DeleteObject(selectedComposer);

            var composersList = (ICollection<Composer>)composersListBox.ItemsSource;

            composersList.Remove(selectedComposer);
            composersListBox.Items.Refresh();
        }

        private void composersToolbar_Saving(object sender, EventArgs e)
        {
            var composersHeader = (EditableHeaderPanel)timeline.Template.FindName("composersHeader", timeline);
            var composersEditPanel = (FrameworkElement)timeline.Template.FindName("composersEditPanel", timeline);
            var composersScrollViewer = (ScrollViewer)timeline.Template.FindName("composersScrollViewer", timeline);
            var composersListBox = (ListBox)timeline.Template.FindName("composersListBox", timeline);
            var composersItemsControl = (ItemsControl)timeline.Template.FindName("composersItemsControl", timeline);

            foreach (var composer in App.ClassicalMusicContext.Composers)
            {
                if (string.IsNullOrWhiteSpace(composer.Name))
                {
                    MessageBox.Show("Could not save because a composer has an empty Name.");

                    return;
                }

                try
                {
                    ExtendedDateTimeInterval.Parse(composer.Dates);
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not save because a composer has invalid dates.");

                    return;
                }

                if (ExtendedDateTimeFormatParser.Parse(composer.Dates) == null)
                {
                    MessageBox.Show("Could not save because a composer has invalid dates.");

                    return;
                }
            }

            App.ClassicalMusicContext.SaveChanges();

            composersItemsControl.ItemsSource = App.ClassicalMusicContext.Composers.OrderBy(c => c.Name);

            composersListBox.ItemsSource = null;

            composersScrollViewer.Visibility = Visibility.Visible;
            composersEditPanel.Visibility = Visibility.Collapsed;

            composersHeader.IsEnabled = true;
        }

        private void composerTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var composerTextBox = (TextBox)sender;
            Keyboard.Focus(composerTextBox);
        }

        private void composerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var composer = (Composer)textBox.DataContext;
            var entityDescriptor = App.ClassicalMusicContext.GetEntityDescriptor(composer);

            if (entityDescriptor.State != EntityStates.Added)
            {
                App.ClassicalMusicContext.ChangeState(composer, EntityStates.Modified);
            }
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

        private void GoToEra(object obj)
        {
            var eraQuery = App.ClassicalMusicContext.Eras.First(e => e.Name == (string)obj);

            var composersSortedByDate = App.ClassicalMusicContext.Composers.ToArray()
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
            var composerToBitmapImageConverter = new ComposerToBitmapImageConverter();
            composerToBitmapImageConverter.ClearFileCache();

            Application.Current.Properties["HorizontalOffset"] = timeline.HorizontalOffset;
            Application.Current.Properties["VerticalOffset"] = timeline.VerticalOffset;

            NavigationService.Refresh();
        }

        private void TimelinePage_Loaded(object sender, RoutedEventArgs e)
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

        private void composerDatesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var composer = (Composer)textBox.DataContext;
            var entityDescriptor = App.ClassicalMusicContext.GetEntityDescriptor(composer);

            if (entityDescriptor.State != EntityStates.Added)
            {
                App.ClassicalMusicContext.ChangeState(composer, EntityStates.Modified);
            }
        }
    }
}