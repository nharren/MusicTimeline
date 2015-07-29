using NathanHarrenstein.MusicDB;
using NathanHarrenstein.MusicTimeline.Builders;
using NathanHarrenstein.MusicTimeline.Converters;
using NathanHarrenstein.MusicTimeline.Input;
using NathanHarrenstein.MusicTimeline.ViewModels;
using NathanHarrenstein.Timeline;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.EDTF;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class TimelinePage : Page
    {
        public static readonly DependencyProperty CloseCommandProperty = DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(TimelinePage));
        public static readonly DependencyProperty FullScreenCommandProperty = DependencyProperty.Register("FullScreenCommand", typeof(ICommand), typeof(TimelinePage));
        public static readonly DependencyProperty GoToCommandProperty = DependencyProperty.Register("GoToCommand", typeof(ICommand), typeof(TimelinePage));
        public static readonly DependencyProperty ManageDataCommandProperty = DependencyProperty.Register("ManageDataCommand", typeof(ICommand), typeof(TimelinePage));
        public static readonly DependencyProperty RebuildThumbnailCacheCommandProperty = DependencyProperty.Register("RebuildThumbnailCacheCommand", typeof(ICommand), typeof(TimelinePage));
        private DataProvider _dataProvider;

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
            _dataProvider.Dispose();
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

        public void Initialize()
        {
            _dataProvider = new DataProvider();

            var eraList = _dataProvider.Eras.AsNoTracking().ToList();
            var composers = _dataProvider.Composers.AsNoTracking();
            var composerEraViewModels = ComposerEraViewModelBuilder.Build(eraList);

            ManageDataCommand = GetManageDataCommand();
            CloseCommand = GetCloseCommand();
            GoToCommand = GetGoToCommand();
            RebuildThumbnailCacheCommand = GetRebuildThumbnailCacheCommand();
            FullScreenCommand = GetFullScreenCommand();

            timeline.Dates = new ExtendedDateTimeInterval(new ExtendedDateTime(1000, 1, 1), ExtendedDateTime.Now);            
            timeline.Eras = composerEraViewModels;
            timeline.Ruler = new TimeRuler();
            timeline.Ruler.TimeRulerUnit = TimeRulerUnit.Day;
            timeline.Ruler.TimeUnitWidth = 0.04109589041;
            timeline.Resolution = TimeResolution.Decade;
            timeline.EventHeight = 60;
            timeline.Events = ComposerEventViewModelBuilder.Build(composers, composerEraViewModels, timeline);
            timeline.Loaded += Timeline_Loaded;
        }

        private void Timeline_Loaded(object sender, RoutedEventArgs e)
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

        private ICommand GetCloseCommand()
        {
            return new DelegateCommand(o => Application.Current.Shutdown());
        }

        private ICommand GetFullScreenCommand()
        {
            return new DelegateCommand(o =>
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
            });
        }

        private ICommand GetGoToCommand()
        {
            return new DelegateCommand(o =>
            {
                var eraQuery = _dataProvider.Eras.First(e => e.Name == (string)o);

                var composersSortedByDate = _dataProvider.Composers.ToArray()
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
            });
        }

        private ICommand GetManageDataCommand()
        {
            return new DelegateCommand(o =>
            {
                Application.Current.Properties["HorizontalOffset"] = timeline.HorizontalOffset;
                Application.Current.Properties["VerticalOffset"] = timeline.VerticalOffset;

                NavigationService.Navigate(new Uri(@"pack://application:,,,/Views/InputPage.xaml", UriKind.Absolute));
            });
        }

        private ICommand GetRebuildThumbnailCacheCommand()
        {
            return new DelegateCommand(o =>
            {
                ComposerToThumbnailConverter.ClearThumbnailCache();

                Application.Current.Properties["HorizontalOffset"] = timeline.HorizontalOffset;
                Application.Current.Properties["VerticalOffset"] = timeline.VerticalOffset;

                NavigationService.Refresh();
            });
        }
    }
}