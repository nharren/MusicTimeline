using NathanHarrenstein.MusicDB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.MusicTimeline.Converters
{
    internal class ComposerToThumbnailConverter : IValueConverter
    {
        private static Dictionary<int, BitmapImage> Cache = new Dictionary<int, BitmapImage>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ComposerToThumbnail((Composer)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static BitmapImage ComposerToThumbnail(Composer composer)
        {
            var thumbnail = (BitmapImage)null;

            if (Cache.TryGetValue(composer.ID, out thumbnail))
            {
                return Cache[composer.ID];
            }

            var directoryPath = $@"{Environment.CurrentDirectory}\Resources\Thumbnails\";

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var thumbnailPath = $@"{Environment.CurrentDirectory}\Resources\Thumbnails\{composer.ID}.jpg";
            var thumbnailUri = new Uri(thumbnailPath, UriKind.Absolute);

            if (File.Exists(thumbnailPath))
            {
                thumbnail = new BitmapImage();
                thumbnail.BeginInit();
                thumbnail.DecodePixelHeight = 50;
                thumbnail.StreamSource = new MemoryStream(File.ReadAllBytes(thumbnailPath));
                thumbnail.EndInit();
                thumbnail.Freeze();

                Cache[composer.ID] = thumbnail;

                return thumbnail;
            }

            return CreateThumbnail(composer);
        }

        internal static void ClearThumbnailCache()
        {
            Cache.Clear();

            var thumbnailDirectory = $@"{Environment.CurrentDirectory}\Resources\Thumbnails\";
            var thumbnailPaths = Directory.EnumerateFiles(thumbnailDirectory).ToArray();

            foreach (var thumbnailPath in thumbnailPaths)
            {
                File.Delete(thumbnailPath);
            }
        }

        private static BitmapImage CreateThumbnail(Composer composer)
        {
            BitmapImage thumbnail = null;
            var thumbnailPath = $@"{Environment.CurrentDirectory}\Resources\Thumbnails\{composer.ID}.jpg";
            var image = composer.ComposerImages.Select(ci => ci.Image).FirstOrDefault();

            if (image != null)
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

                using (var stream = new FileStream(thumbnailPath, FileMode.Create))
                {
                    encoder.Save(stream);
                }

                Cache[composer.ID] = thumbnail;

                return thumbnail;
            }
            else if (!Cache.TryGetValue(0, out thumbnail))
            {
                thumbnailPath = $@"{Environment.CurrentDirectory}\Resources\Thumbnails\0.jpg";
                var thumbnailUri = new Uri(thumbnailPath, UriKind.Absolute);

                if (File.Exists(thumbnailPath))                                                                // Cache and return default thumbnail from file.
                {
                    thumbnail = new BitmapImage();
                    thumbnail.BeginInit();
                    thumbnail.DecodePixelHeight = 50;
                    thumbnail.StreamSource = new MemoryStream(File.ReadAllBytes(thumbnailPath));
                    thumbnail.EndInit();
                    thumbnail.Freeze();

                    Cache[0] = thumbnail;

                    return thumbnail;
                }
                else                                                                                            // Create, cache and return default thumbnail from file.
                {
                    thumbnail = new BitmapImage();
                    thumbnail.CacheOption = BitmapCacheOption.OnLoad;
                    thumbnail.BeginInit();
                    thumbnail.DecodePixelHeight = 50;
                    thumbnail.StreamSource = Application.GetResourceStream(new Uri("pack://application:,,,/Resources/Composers/Unknown.jpg", UriKind.Absolute)).Stream;
                    thumbnail.EndInit();
                    thumbnail.Freeze();

                    var encoder = new JpegBitmapEncoder();
                    encoder.QualityLevel = 80;
                    encoder.Frames.Add(BitmapFrame.Create(thumbnail));

                    using (var stream = new FileStream(thumbnailPath, FileMode.CreateNew))
                    {
                        encoder.Save(stream);
                    }

                    Cache[0] = thumbnail;

                    return thumbnail;
                }
            }
            else
            {
                return thumbnail;
            }
        }
    }
}