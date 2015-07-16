using NathanHarrenstein.MusicDB;
using NathanHarrenstein.MusicTimeline.Input;
using NathanHarrenstein.MusicTimeline.Providers;
using NathanHarrenstein.MusicTimeline.ViewModels;
using NathanHarrenstein.Timeline;
using System;
using System.Collections.Generic;
using System.EDTF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace NathanHarrenstein.MusicTimeline.Views
{
    public partial class TimelinePage : Page
    {
        public static readonly DependencyProperty CloseCommandProperty = DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(TimelinePage));
        public static readonly DependencyProperty GoToCommandProperty = DependencyProperty.Register("GoToCommand", typeof(ICommand), typeof(TimelinePage));
        public static readonly DependencyProperty ManageDataCommandProperty = DependencyProperty.Register("ManageDataCommand", typeof(ICommand), typeof(TimelinePage));
        public static readonly DependencyProperty RebuildThumbnailCacheCommandProperty = DependencyProperty.Register("RebuildThumbnailCacheCommand", typeof(ICommand), typeof(TimelinePage));
        private DataProvider _dataProvider;

        public TimelinePage()
        {
            InitializeComponent();
            Initialize();
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

            ManageDataCommand = GetManageDataCommand();
            CloseCommand = GetCloseCommand();
            GoToCommand = GetGoToCommand();
            RebuildThumbnailCacheCommand = GetRebuildThumbnailCacheCommand();

            timeline.Dates = new ExtendedDateTimeInterval(new ExtendedDateTime(1000, 1, 1), ExtendedDateTime.Now);
            timeline.Eras = ComposerEraProvider.GetEras(_dataProvider);
            timeline.Ruler = new TimeRuler { TimeRulerUnit = TimeRulerUnit.Day, TimeUnitWidth = 0.04109589041 };
            timeline.Resolution = TimeResolution.Decade;
            timeline.EventHeight = 60;
            timeline.Events = ComposerEventProvider.GetComposerEvents(_dataProvider, (IList<ComposerEraViewModel>)timeline.Eras, timeline);
            timeline.Loaded += (o, e) =>
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
            };
        }

        private ICommand GetRebuildThumbnailCacheCommand()
        {
            return new DelegateCommand(o =>
            {
                foreach (var composer in _dataProvider.Composers)
                {
                    ComposerEventProvider.CreateThumbnail(composer);
                }

                Application.Current.Properties["HorizontalOffset"] = timeline.HorizontalOffset;
                Application.Current.Properties["VerticalOffset"] = timeline.VerticalOffset;

                NavigationService.Refresh();
            });
        }

        private ICommand GetCloseCommand()
        {
            return new DelegateCommand(o => Application.Current.Shutdown());
        }

        private ICommand GetGoToCommand()
        {
            return new DelegateCommand(o =>
            {
                timeline.HorizontalOffset = timeline.Ruler.ToPixels(timeline.Dates.Earliest(), new ExtendedDateTime(int.Parse((string)o)));
            });
        }

        private ICommand GetManageDataCommand()
        {
            return new DelegateCommand(o =>
            {
                NavigationService.Navigate(new Uri(@"pack://application:,,,/Views/InputPage.xaml", UriKind.Absolute));
            });
        }

        private void page_Unloaded(object sender, RoutedEventArgs e)
        {
            _dataProvider.Dispose();
        }
    }
}