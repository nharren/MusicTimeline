using Database;
using NathanHarrenstein.ComposerTimeline.Controls;
using NathanHarrenstein.ComposerTimeline.Data;
using NathanHarrenstein.ComposerTimeline.UI.Data.Providers;
using NathanHarrenstein.Converters;
using NathanHarrenstein.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.ComposerTimeline
{
    public static class ComposerControlProvider
    {
        private static Dictionary<int, BitmapImage> ThumbnailDictionary = new Dictionary<int, BitmapImage>();

        public static ComposerControl GetComposerControl(ComposerEvent composerEvent)
        {
            var composerControl = new ComposerControl();

            composerControl.ComposerEvent = composerEvent;
            composerControl.SortTitle = composerEvent.Composer.Name;
            composerControl.Eras = composerEvent.Composer.Eras.Select(e => e.Name);
            composerControl.Title = NameConverter.ToFirstLast(composerEvent.Composer.Name);
            composerControl.Image = GetThumbnail(composerEvent.Composer);
            composerControl.Born = GetBorn(composerEvent.Composer);
            composerControl.Died = GetDied(composerEvent.Composer);
            composerControl.Flags = GetFlags(composerEvent.Composer);
            composerControl.Click = GetClickCommand(composerEvent.Composer);
            // composerControl.PlayPopularCommand = GetPopularCommand();                       REQUIREMENTS: Setting the Audio File Directory, Reading IDs from Audio Files.

            return composerControl;
        }

        private static string GetBorn(Composer composer)
        {
            if (composer != null)
            {
                return string.Format("{0}; {1}", composer.Dates.Start, composer.BirthLocation.Name);
            }

            return composer.Dates.Start.ToString();
        }

        private static DelegateCommand GetClickCommand(Composer composer)
        {
            Action<object> click = o =>
            {
                App.Current.Properties.Add("SelectedComposer", composer);

                var frame = (Frame)App.Current.MainWindow.FindName("Frame");

                frame.Navigate(new Uri(@"\UI\ComposerPage.xaml"));
            };

            return new DelegateCommand(click);
        }

        private static string GetDied(Composer composer)
        {
            if (composer.DeathLocation != null)
            {
                return string.Format("{0}; {1}", composer.Dates.End, composer.DeathLocation.Name);
            }

            return composer.Dates.End.ToString();
        }

        private static IEnumerable<Flag> GetFlags(Composer composer)
        {
            return composer.Nationalities
                .Select(n => FlagProvider.GetFlag(n.Name, FlagSize.Small))
                .DefaultIfEmpty(FlagProvider.GetFlag(null, FlagSize.Small));
        }

        private static BitmapImage GetThumbnail(Composer composer)
        {
            if (ThumbnailDictionary[composer.ID] != null)                                                // Return thumbnail from cache.
            {
                return ThumbnailDictionary[composer.ID];
            }

            var thumbnailUriFormatString = @"\UI\Resources\Thumbnails\{0}.bin";
            var thumbnailUriString = string.Format(thumbnailUriFormatString, composer.ID);
            var thumbnailUri = new Uri(thumbnailUriString, UriKind.Relative);
            var thumbnailPath = thumbnailUri.AbsolutePath;

            if (File.Exists(thumbnailPath))                                                              // Cache and return thumbnail from file.
            {
                var thumbnail = new BitmapImage();
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
                var thumbnail = new BitmapImage();
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
            else if (ThumbnailDictionary[0] == null)
            {
                thumbnailUriString = string.Format(thumbnailUriFormatString, 0);
                thumbnailUri = new Uri(thumbnailUriString, UriKind.Relative);
                thumbnailPath = thumbnailUri.AbsolutePath;

                if (File.Exists(thumbnailPath))                                                                // Cache and return default thumbnail from file.
                {
                    var thumbnail = new BitmapImage();
                    thumbnail.CacheOption = BitmapCacheOption.None;
                    thumbnail.BeginInit();
                    thumbnail.DecodePixelHeight = 50;
                    thumbnail.UriSource = new Uri(string.Format(thumbnailUriFormatString, 0), UriKind.Relative);
                    thumbnail.EndInit();
                    thumbnail.Freeze();

                    ThumbnailDictionary[0] = thumbnail;

                    return thumbnail;
                }
                else                                                                                            // Create, cache and return default thumbnail from file.
                {
                    var thumbnail = new BitmapImage();
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

                    ThumbnailDictionary[0] = thumbnail;

                    return thumbnail;
                }
            }
            else                                                                                      // Return default thumbnail from cache.
            {
                return ThumbnailDictionary[0];
            }
        }

        /*
        private ICommand GetPopularCommand()
        {
            Func<object, Task> playPopular = async o =>
            {
                await Task.Run(() =>
                    {
                        var compositions = Composer.Compositions;
                        var compositionProperties = compositions.SelectMany(c => c.CompositionProperties);
                        var popularCompositionPopularProperties = compositionProperties.Where(cp => cp.Key == "Popular" && cp.Value == "Yes");
                        var popularCompositions = popularCompositionPopularProperties.Select(pc => pc.Composition);
                        var popularCompositionProperties = popularCompositions.SelectMany(pc => pc.CompositionProperties);
                        var popularCompositionAudioProperties = popularCompositionProperties.Where(pcp => pcp.Key == "Audio");
                        var popularCompositionAudioPaths = popularCompositionAudioProperties.Select(pcap => pcap.Value);

                        var movements = compositions.SelectMany(c => c.Movements);
                        var movementProperties = movements.SelectMany(m => m.MovementProperties);
                        var popularMovementPopularProperties = movementProperties.Where(mp => mp.Key == "Popular" && mp.Value == "Yes");
                        var popularMovements = popularMovementPopularProperties.Select(pmpp => pmpp.Movement);
                        var popularMovementProperties = popularMovements.SelectMany(pm => pm.MovementProperties);
                        var popularMovementAudioProperties = popularMovementProperties.Where(pmp => pmp.Key == "Audio");
                        var popularMovementAudioPaths = popularMovementAudioProperties.Select(pmap => pmap.Value);

                        var fileName = Path.GetTempFileName() + Guid.NewGuid().ToString() + ".m3u8";

                        File.WriteAllLines(fileName, popularCompositionAudioPaths);
                        File.AppendAllLines(fileName, popularMovementAudioPaths);

                        Process.Start(fileName);
                    });
            };

            Predicate<object> hasPopular = s =>
            {
                return Composer.Compositions.Any(composition =>
                {
                    if (composition.CompositionProperties.Any(cp => cp.Key == "Popular" && cp.Value == "Yes"))
                    {
                        return true;
                    }

                    if (composition.Movements.Any(m => m.MovementProperties.Any(mp => mp.Key == "Popular" && mp.Value == "Yes")))
                    {
                        return true;
                    }

                    return false;
                });
            };

            return new AsyncDelegateCommand(playPopular, hasPopular);
        }
        */
    }
}