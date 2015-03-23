using NathanHarrenstein.ComposerDatabase;
using NathanHarrenstein.ComposerTimeline;
using NathanHarrenstein.ComposerTimeline.Controls;
using NathanHarrenstein.ComposerTimeline.Data;
using NathanHarrenstein.ComposerTimeline.UI.Data.Providers;
using NathanHarrenstein.ComposerTimeline.UI.Providers;
using NathanHarrenstein.Converters;
using NathanHarrenstein.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace NathanHarrenstein.ComposerTimeline
{
    public class ComposerControlProvider
    {
        private Composer Composer { get; set; }

        public ComposerControl GetComposerControl(ComposerEvent composerEvent)
        {
            Composer = composerEvent.Composer;

            var composerControl = new ComposerControl();

            composerControl.ComposerEvent = composerEvent;
            composerControl.SortTitle = composerEvent["Name"];
            composerControl.Era = composerEvent["Era"];
            composerControl.Title = GetTitle();
            composerControl.Image = GetImage();
            composerControl.Born = GetBorn();
            composerControl.Died = GetDied();
            composerControl.Flag = GetFlag();
            composerControl.Click = GetClickCommand();
            composerControl.PlayPopularCommand = GetPopularCommand();

            return composerControl;
        }

        private string GetBorn()
        {
            var composerProperties = Composer.ComposerProperties;

            var birthdayProperty = composerProperties.FirstOrDefault(cp => cp.Key == "Deathday");
            var birthplaceProperty = composerProperties.FirstOrDefault(cp => cp.Key == "Deathplace");

            var birthday = birthdayProperty.Value;
            var birthplace = birthplaceProperty.Value;

            if (!string.IsNullOrWhiteSpace(birthplace))
            {
                return birthday += "; " + birthplace;
            }

            return birthday;
        }

        private DelegateCommand GetClickCommand()
        {
            Action<object> click = o =>
            {
                var navigationWindow = (NavigationWindow)App.Current.MainWindow;

                App.Current.Properties.Add("SelectedComposer", Composer);

                navigationWindow.Navigate(new Uri(@"\UI\ComposerPage.xaml"));
            };

            return new DelegateCommand(click);
        }

        private string GetDied()
        {
            var composerProperties = Composer.ComposerProperties;

            var deathdayProperty = composerProperties.FirstOrDefault(cp => cp.Key == "Deathday");
            var deathplaceProperty = composerProperties.FirstOrDefault(cp => cp.Key == "Deathplace");

            var deathday = deathdayProperty.Value;
            var deathplace = deathplaceProperty.Value;

            if (!string.IsNullOrWhiteSpace(deathplace))
            {
                return deathday += "; " + deathplace;
            }

            return deathday;
        }

        private FlagData GetFlag()
        {
            var flagProvider = new FlagDataProvider();

            foreach (var composerProperty in Composer.ComposerProperties)
            {
                if (composerProperty.Key == "Nationality")
                {
                    return flagProvider.GetFlagData(composerProperty.Value);
                }
            }

            return flagProvider.GetFlagData(null);
        }

        private BitmapImage GetImage()
        {
            var imageQuery = from property in Composer.ComposerProperties
                             where property.Key == "Image" && File.Exists(property.Value)
                             select property;

            var imageProperty = imageQuery.FirstOrDefault();

            if (imageProperty != null)
            {
                FileStream stream;

                var thumbnailPath = @"F:\Old Timeline\Files\Images\Thumbnails\" + Path.GetFileName(imageProperty.Value);

                if (thumbnailPath.Length > 259)
                {
                    var filename = @"F:\Old Timeline\Files\Images\Thumbnails\" + Path.GetFileNameWithoutExtension(imageProperty.Value);
                    var extension = Path.GetExtension(imageProperty.Value);

                    thumbnailPath = filename.Substring(0, 259 - extension.Length) + extension;
                }

                try
                {
                    stream = new FileStream(thumbnailPath, FileMode.OpenOrCreate);
                }
                catch (DirectoryNotFoundException)
                {
                    Directory.CreateDirectory(@"F:\Old Timeline\Files\Images\Thumbnails\");

                    stream = new FileStream(thumbnailPath, FileMode.OpenOrCreate);
                }

                if (stream.Length == 0)
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.CacheOption = BitmapCacheOption.None;
                    bitmapImage.BeginInit();
                    bitmapImage.DecodePixelHeight = 50;
                    bitmapImage.UriSource = new Uri(imageProperty.Value);
                    bitmapImage.EndInit();

                    var encoder = new JpegBitmapEncoder();
                    encoder.QualityLevel = 100;
                    encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                    encoder.Save(stream);

                    stream.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                }

                var thumb = new BitmapImage();
                thumb.CacheOption = BitmapCacheOption.None;
                thumb.BeginInit();
                thumb.StreamSource = stream;
                thumb.EndInit();
                thumb.Freeze();

                return thumb;
            }
            else if (ComposerControl.DefaultImage == null)
            {
                ComposerControl.DefaultImage = new BitmapImage();
                ComposerControl.DefaultImage.CacheOption = BitmapCacheOption.None;
                ComposerControl.DefaultImage.BeginInit();
                ComposerControl.DefaultImage.UriSource = new Uri(@"\UI\Resources\NoImage.jpg", UriKind.Relative);
                ComposerControl.DefaultImage.EndInit();
                ComposerControl.DefaultImage.Freeze();

                return ComposerControl.DefaultImage;
            }
            else
            {
                return ComposerControl.DefaultImage;
            }
        }

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

        private int GetStartTime()
        {
            var composerProperties = Composer.ComposerProperties;

            var startTimeProperty = composerProperties.FirstOrDefault(cp => cp.Key == "Birthday");
            var startTimeString = startTimeProperty.Value;
            var startTime = 0;

            int.TryParse(startTimeString, out startTime);

            return startTime;
        }

        private string GetTitle()
        {
            var nameConverter = new NameConverter();

            foreach (var composerProperty in Composer.ComposerProperties)
            {
                var isNameProperty = composerProperty.Key == "Name";
                var notNullOrWhitespace = !string.IsNullOrWhiteSpace(composerProperty.Value);

                if (isNameProperty && notNullOrWhitespace)
                {
                    return nameConverter.ToFirstNameLastName(composerProperty.Value);
                }
            }

            throw new InvalidOperationException("There is no name property for the composer with the ID: " + Composer.ID);
        }
    }
}