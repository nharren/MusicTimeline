using NathanHarrenstein.MusicDB;
using NathanHarrenstein.MusicTimeline.Input;
using NathanHarrenstein.MusicTimeline.ViewModels;
using NathanHarrenstein.MusicTimeline.Providers;
using NathanHarrenstein.MusicTimeline.Utilities;
using NathanHarrenstein.MusicTimeline.Views;
using NathanHarrenstein.Timeline;
using System;
using System.Collections;
using System.Collections.Generic;
using System.EDTF;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.MusicTimeline.Initializers
{
    public static class TimelineInitializer
    {
        private static Dictionary<int, BitmapImage> ThumbnailDictionary = new Dictionary<int, BitmapImage>();

        public static void Initialize(Timeline.Timeline timeline)
        {
            timeline.Dates = new ExtendedDateTimeInterval(new ExtendedDateTime(1000, 1, 1), ExtendedDateTime.Now);
            timeline.Eras = GetEras();
            timeline.Ruler = GetRuler();
            timeline.Resolution = TimeResolution.Decade;
            timeline.EventHeight = 60;
            timeline.Events = GetEvents((IList<ComposerEraViewModel>)timeline.Eras, timeline);
            timeline.Loaded += (o, e) =>
            {
                var horizontalOffset = Application.Current.Properties["HorizontalOffset"] as double?;

                if (horizontalOffset != null)
                {
                    timeline.HorizontalOffset = horizontalOffset.Value;
                }

                var verticalOffset = Application.Current.Properties["HorizontalOffset"] as double?;

                if (verticalOffset != null)
                {
                    timeline.VerticalOffset = verticalOffset.Value;
                }
            };
        }

        private static string GetBorn(Composer composer)
        {
            if (composer.BirthLocation != null)
            {
                return $"{ExtendedDateTimeInterval.Parse(composer.Dates).Start}; {composer.BirthLocation.Name}";
            }

            return ExtendedDateTimeInterval.Parse(composer.Dates).Start.ToString();
        }

        private static DelegateCommand GetCommand(Composer composer, Timeline.Timeline timeline)
        {
            Action<object> command = o =>
            {
                Application.Current.Properties["SelectedComposer"] = composer;
                Application.Current.Properties["HorizontalOffset"] = timeline.HorizontalOffset;
                Application.Current.Properties["VerticalOffset"] = timeline.VerticalOffset;

                ((Frame)System.Windows.Application.Current.MainWindow.FindName("Frame")).Navigate(new ComposerPage());
            };

            return new DelegateCommand(command);
        }

        private static string GetDied(Composer composer)
        {
            if (composer.DeathLocation != null)
            {
                return $"{ExtendedDateTimeInterval.Parse(composer.Dates).End}; {composer.DeathLocation.Name}";
            }

            return ExtendedDateTimeInterval.Parse(composer.Dates).End.ToString();
        }

        private static List<ComposerEraViewModel> GetEras()
        {
            var eras = new List<ComposerEraViewModel>();

            foreach (var era in App.DataProvider.Eras)
            {
                var background = (SolidColorBrush)null;

                if (era.Name == "Medieval")
                {
                    background = new SolidColorBrush(Color.FromRgb(153, 153, 153));            // #FF999999
                }
                else if (era.Name == "Renaissance")
                {
                    background = new SolidColorBrush(Color.FromRgb(155, 128, 181));            // #FF9B80B5
                }
                else if (era.Name == "Baroque")
                {
                    background = new SolidColorBrush(Color.FromRgb(204, 77, 77));              // #FFCC4D4D
                }
                else if (era.Name == "Classical")
                {
                    background = new SolidColorBrush(Color.FromRgb(51, 151, 193));             // #FF3397C1
                }
                else if (era.Name == "Romantic")
                {
                    background = new SolidColorBrush(Color.FromRgb(69, 168, 90));              // #FF45A85A
                }
                else if (era.Name == "20th Century")
                {
                    background = new SolidColorBrush(Color.FromRgb(205, 173, 74));             // #FFCDAD4A
                }
                else if (era.Name == "21st Century")
                {
                    background = new SolidColorBrush(Color.FromRgb(219, 109, 138));            // #FFDB6D8A
                }

                var musicEra = new ComposerEraViewModel(era.Name, ExtendedDateTimeInterval.Parse(era.Dates), background, Brushes.White);

                eras.Add(musicEra);
            }

            return eras;
        }

        private static IList GetEvents(IList<ComposerEraViewModel> musicEras, Timeline.Timeline timeline)
        {
            var eventList = new List<ComposerEventViewModel>();
            var composers = App.DataProvider.Composers.ToList();

            foreach (var composer in composers)
            {
                var background = (Brush)null;
                var composerEras = new List<ComposerEraViewModel>();
                var eras = composer.Eras.ToList();

                foreach (var era in eras)
                {
                    foreach (var musicEra in musicEras)
                    {
                        if (era.Name == musicEra.Label)
                        {
                            composerEras.Add(musicEra);
                        }
                    }
                }

                var composerEraCount = composerEras.Count;

                if (composerEraCount > 1)
                {
                    background = new LinearGradientBrush { StartPoint = new Point(0, 0.5), EndPoint = new Point(1, 0.5) };

                    for (int i = 0; i < composerEraCount; i++)
                    {
                        ((LinearGradientBrush)background).GradientStops.Add(new GradientStop(composerEras[i].Background.Color, i / (composerEraCount - 1)));
                    }
                }
                else if (composerEraCount == 1)
                {
                    background = composerEras[0].Background;
                }
                else
                {
                    background = Brushes.Black;
                }

                var composerEvent = new ComposerEventViewModel(NameUtility.ToFirstLast(composer.Name), ExtendedDateTimeInterval.Parse(composer.Dates), GetBorn(composer), GetDied(composer), composer, background, Brushes.White, GetThumbnail(composer), GetFlags(composer), composerEras, GetCommand(composer, timeline), null);

                eventList.Add(composerEvent);
            }

            return eventList.OrderBy(e => e.Dates.Earliest()).ToList();
        }

        private static List<FlagViewModel> GetFlags(Composer composer)
        {
            return composer.Nationalities
                .Select(n => FlagProvider.GetFlag(n.Name, FlagSize.Small))
                .DefaultIfEmpty(FlagProvider.GetFlag(null, FlagSize.Small))
                .ToList();
        }

        private static TimeRuler GetRuler()
        {
            var timeRuler = new TimeRuler();
            timeRuler.TimeRulerUnit = TimeRulerUnit.Day;
            timeRuler.TimeUnitWidth = 0.04109589041;

            return timeRuler;
        }

        private static BitmapImage GetThumbnail(Composer composer)
        {
            var thumbnail = (BitmapImage)null;

            if (ThumbnailDictionary.TryGetValue(composer.ID, out thumbnail))                                                // Return thumbnail from cache.
            {
                return ThumbnailDictionary[composer.ID];
            }

            var directoryPath = $@"{Environment.CurrentDirectory}\Resources\Thumbnails\";

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var thumbnailPath = $@"{Environment.CurrentDirectory}\Resources\Thumbnails\{composer.ID}.jpg";
            var thumbnailUri = new Uri(thumbnailPath, UriKind.Absolute);

            if (File.Exists(thumbnailPath))                                                              // Cache and return thumbnail from file.
            {
                thumbnail = new BitmapImage();
                thumbnail.CacheOption = BitmapCacheOption.OnLoad;
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
                thumbnail.CacheOption = BitmapCacheOption.OnLoad;
                thumbnail.BeginInit();
                thumbnail.DecodePixelHeight = 50;
                thumbnail.StreamSource = new MemoryStream(image);
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
                thumbnailPath = $@"{Environment.CurrentDirectory}\Resources\Thumbnails\0.jpg";
                thumbnailUri = new Uri(thumbnailPath, UriKind.Absolute);

                if (File.Exists(thumbnailPath))                                                                // Cache and return default thumbnail from file.
                {
                    thumbnail = new BitmapImage();
                    thumbnail.CacheOption = BitmapCacheOption.OnLoad;
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
                    thumbnail.CacheOption = BitmapCacheOption.OnLoad;
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
        }
    }
}