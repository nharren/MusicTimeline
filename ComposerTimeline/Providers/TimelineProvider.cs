using Database;
using NathanHarrenstein.ComposerTimeline.Controls;
using NathanHarrenstein.ComposerTimeline.Data.Providers;
using NathanHarrenstein.Converters;
using NathanHarrenstein.Input;
using NathanHarrenstein.Services;
using NathanHarrenstein.Timeline;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ExtendedDateTimeFormat;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace NathanHarrenstein.ComposerTimeline.Initializers
{
    public static class TimelineProvider
    {
        private static Dictionary<int, BitmapImage> ThumbnailDictionary = new Dictionary<int, BitmapImage>();

        public async static Task<Timeline.Timeline> GetTimeline()
        {
            var timeline = new Timeline.Timeline();
            timeline.Dates = new ExtendedDateTimeInterval(new ExtendedDateTime(1000, 1, 1), ExtendedDateTime.Now);
            timeline.Eras = GetEras();
            timeline.Events = await GetEvents((List<EraControl>)timeline.Eras);
            timeline.Ruler = GetRuler();
            timeline.Resolution = TimeUnit.Decade;
            timeline.EventHeight = 60;

            return timeline;
        }

        private static TimeRuler GetRuler()
        {
            var timeRuler = new TimeRuler();
            timeRuler.TimeRulerUnit = TimeRulerUnit.Day;
            timeRuler.TimeUnitWidth = 0.04109589041;

            return timeRuler;
        }

        private static List<EraControl> GetEras()
        {
            var eras = new List<EraControl>();

            using (var dataProvider = new Database.DataProvider())
            {
                foreach (var era in dataProvider.Eras)
                {
                    var eraControl = new EraControl();
                    eraControl.Label = era.Name;
                    eraControl.Dates = ExtendedDateTimeInterval.Parse(era.Dates);
                    eraControl.Foreground = Brushes.White;

                    if (era.Name == "Medieval")
                    {
                        eraControl.Background = new SolidColorBrush(Color.FromRgb(153, 153, 153));            // #FF999999
                    }
                    else if (era.Name == "Renaissance")
                    {
                        eraControl.Background = new SolidColorBrush(Color.FromRgb(155, 128, 181));            // #FF9B80B5
                    }
                    else if (era.Name == "Baroque")
                    {
                        eraControl.Background = new SolidColorBrush(Color.FromRgb(204, 77, 77));              // #FFCC4D4D
                    }
                    else if (era.Name == "Classical")
                    {
                        eraControl.Background = new SolidColorBrush(Color.FromRgb(51, 151, 193));             // #FF3397C1
                    }
                    else if (era.Name == "Romantic")
                    {
                        eraControl.Background = new SolidColorBrush(Color.FromRgb(69, 168, 90));              // #FF45A85A
                    }
                    else if (era.Name == "20th Century")
                    {
                        eraControl.Background = new SolidColorBrush(Color.FromRgb(205, 173, 74));             // #FFCDAD4A
                    }
                    else if (era.Name == "21st Century")
                    {
                        eraControl.Background = new SolidColorBrush(Color.FromRgb(219, 109, 138));            // #FFDB6D8A
                    }

                    eras.Add(eraControl);
                }
            }

            return eras;
        }

        private async static Task<List<EventControl>> GetEvents(List<EraControl> eraControls)
        {
            var eventList = new List<EventControl>();

            using (var dataProvider = new Database.DataProvider())
            {
                foreach (var composer in dataProvider.Composers)
                {
                    var composerEvent = new ComposerEventControl();

                    composerEvent.Image = await GetThumbnail(composer);
                    composerEvent.Flags = await GetFlags(composer);
                    composerEvent.Tag = composer;
                    composerEvent.Label = NameConverter.ToFirstLast(composer.Name);
                    composerEvent.Dates = ExtendedDateTimeInterval.Parse(composer.Dates);
                    composerEvent.Born = GetBorn(composer);
                    composerEvent.Died = GetDied(composer);

                    composerEvent.Command = GetCommand(composer);

                    foreach (var era in composer.Eras)
                    {
                        foreach (var eraControl in eraControls)
                        {
                            if (era.Name == eraControl.Label)
                            {
                                composerEvent.Eras.Add(eraControl);
                                composerEvent.Background = eraControl.Background;
                                composerEvent.Foreground = eraControl.Foreground;
                            }
                        }
                    }

                    eventList.Add(composerEvent);
                }
            }

            return eventList.OrderBy(e => e.Dates.Earliest()).ToList();
        }

        private static string GetBorn(Composer composer)
        {
            if (composer.BirthLocation != null)
            {
                return string.Format("{0}; {1}", ExtendedDateTimeInterval.Parse(composer.Dates).Start, composer.BirthLocation.Name);
            }

            return ExtendedDateTimeInterval.Parse(composer.Dates).Start.ToString();
        }

        private static DelegateCommand GetCommand(Composer composer)
        {
            Action<object> command = o =>
            {
                App.Current.Properties.Add("SelectedComposer", composer);

                var frame = (Frame)App.Current.MainWindow.FindName("Frame");

                frame.Navigate(new Uri(@"pack://application:,,,/Pages/ComposerPage.xaml"));
            };

            return new DelegateCommand(command);
        }

        private static string GetDied(Composer composer)
        {
            if (composer.DeathLocation != null)
            {
                return string.Format("{0}; {1}", ExtendedDateTimeInterval.Parse(composer.Dates).End, composer.DeathLocation.Name);
            }

            return ExtendedDateTimeInterval.Parse(composer.Dates).End.ToString();
        }

        private async static Task<List<Flag>> GetFlags(Composer composer)
        {
            return await Task.Run<List<Flag>>(() => composer.Nationalities
                .Select(n => FlagProvider.GetFlag(n.Name, FlagSize.Small))
                .DefaultIfEmpty(FlagProvider.GetFlag(null, FlagSize.Small))
                .ToList());
        }

        private async static Task<BitmapImage> GetThumbnail(Composer composer)
        {
            return await Task.Run<BitmapImage>(() =>
            {
                var thumbnail = (BitmapImage)null;

                if (ThumbnailDictionary.TryGetValue(composer.ID, out thumbnail))                                                // Return thumbnail from cache.
                {
                    return ThumbnailDictionary[composer.ID];
                }

                var directoryPath = string.Format(@"{0}\Resources\Thumbnails\", Environment.CurrentDirectory);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var thumbnailPath = string.Format(@"{0}\Resources\Thumbnails\{1}.jpg", Environment.CurrentDirectory, composer.ID);
                var thumbnailUri = new Uri(thumbnailPath, UriKind.Absolute);

                if (File.Exists(thumbnailPath))                                                              // Cache and return thumbnail from file.
                {
                    thumbnail = new BitmapImage();
                    thumbnail.CacheOption = BitmapCacheOption.None;
                    thumbnail.BeginInit();
                    thumbnail.DecodePixelHeight = 50;
                    thumbnail.UriSource = thumbnailUri;
                    thumbnail.EndInit();
                    thumbnail.Freeze();

                    ThumbnailDictionary[composer.ID] = thumbnail;

                    return thumbnail;
                }

                var image = composer.ComposerImages.Select(ci => ci.Image).FirstOrDefault();

                if (image != null)                                                                          // Create, cache, and return a thumbnail from a composer image.
                {
                    thumbnail = new BitmapImage();
                    thumbnail.CacheOption = BitmapCacheOption.None;
                    thumbnail.BeginInit();
                    thumbnail.DecodePixelHeight = 50;
                    thumbnail.UriSource = thumbnailUri;
                    thumbnail.EndInit();
                    thumbnail.Freeze();

                    var encoder = new JpegBitmapEncoder();
                    encoder.QualityLevel = 80;
                    encoder.Frames.Add(BitmapFrame.Create(thumbnail));

                    using (var stream = new FileStream(thumbnailPath, FileMode.CreateNew))
                    {
                        encoder.Save(stream);
                    }

                    ThumbnailDictionary[composer.ID] = thumbnail;

                    return thumbnail;
                }
                else if (!ThumbnailDictionary.TryGetValue(0, out thumbnail))
                {
                    thumbnailPath = string.Format(@"{0}\Resources\Thumbnails\0.jpg", Environment.CurrentDirectory);
                    thumbnailUri = new Uri(thumbnailPath, UriKind.Absolute);

                    if (File.Exists(thumbnailPath))                                                                // Cache and return default thumbnail from file.
                    {
                        thumbnail = new BitmapImage();
                        thumbnail.CacheOption = BitmapCacheOption.None;
                        thumbnail.BeginInit();
                        thumbnail.DecodePixelHeight = 50;
                        thumbnail.UriSource = thumbnailUri;
                        thumbnail.EndInit();
                        thumbnail.Freeze();

                        ThumbnailDictionary[0] = thumbnail;

                        return thumbnail;
                    }
                    else                                                                                            // Create, cache and return default thumbnail from file.
                    {
                        var defaultUri = new Uri("pack://application:,,,/Resources/Composers/Unknown.jpg", UriKind.Absolute);

                        thumbnail = new BitmapImage();
                        thumbnail.CacheOption = BitmapCacheOption.None;
                        thumbnail.BeginInit();
                        thumbnail.DecodePixelHeight = 50;
                        thumbnail.UriSource = defaultUri;
                        thumbnail.EndInit();
                        thumbnail.Freeze();

                        var encoder = new JpegBitmapEncoder();
                        encoder.QualityLevel = 80;
                        encoder.Frames.Add(BitmapFrame.Create(thumbnail));

                        using (var stream = new FileStream(thumbnailPath, FileMode.CreateNew))
                        {
                            encoder.Save(stream);
                        }

                        ThumbnailDictionary[0] = thumbnail;

                        return thumbnail;
                    }
                }
                else                                                                                      // Return default thumbnail from cache.
                {
                    return thumbnail;
                }
            });
        }
    }
}