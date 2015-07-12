using NathanHarrenstein.MusicDB;
using NathanHarrenstein.MusicTimeline.Input;
using NathanHarrenstein.MusicTimeline.Utilities;
using NathanHarrenstein.MusicTimeline.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.EDTF;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace NathanHarrenstein.MusicTimeline.Providers
{
    public static class ComposerEventProvider
    {
        private static Dictionary<int, BitmapImage> ThumbnailDictionary = new Dictionary<int, BitmapImage>();

        public static IList GetComposerEvents(DataProvider dataProvider, IList<ComposerEraViewModel> musicEras, Timeline.Timeline timeline)
        {
            var eventList = new List<ComposerEventViewModel>();
            dataProvider.Composers.Load();

            foreach (var composer in dataProvider.Composers.Local)
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

                var composerEvent = new ComposerEventViewModel(
                    NameUtility.ToFirstLast(composer.Name),
                    ExtendedDateTimeInterval.Parse(composer.Dates),
                    GetBorn(composer),
                    GetDied(composer),
                    composer,
                    background,
                    Brushes.White,
                    GetThumbnail(composer),
                    composerEras,
                    GetCommand(composer, timeline), null);

                eventList.Add(composerEvent);
            }

            return eventList.OrderBy(e => e.Dates.Earliest()).ToList();
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
                Application.Current.Properties["SelectedComposer"] = composer.Name;
                Application.Current.Properties["HorizontalOffset"] = timeline.HorizontalOffset;
                Application.Current.Properties["VerticalOffset"] = timeline.VerticalOffset;

                ((NavigationWindow)Application.Current.MainWindow).Navigate(new Uri("pack://application:,,,/Views/ComposerPage.xaml"));
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
                encoder.QualityLevel = 95;
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