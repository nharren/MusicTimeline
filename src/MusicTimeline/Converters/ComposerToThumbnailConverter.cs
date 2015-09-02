using NathanHarrenstein.ClassicalMusicDb;
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
        private static Dictionary<int, BitmapImage> _thumbnailCache = new Dictionary<int, BitmapImage>();

        public static BitmapImage ComposerToThumbnail(Composer composer)
        {
            var thumbnail = (BitmapImage)null;

            if (_thumbnailCache.TryGetValue(composer.ComposerId, out thumbnail))
            {
                return _thumbnailCache[composer.ComposerId];
            }

            var directoryPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create)}\Music Timeline\Resources\Thumbnails\";

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var thumbnailPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create)}\Music Timeline\Resources\Thumbnails\{composer.ComposerId}.jpg";
            var thumbnailUri = new Uri(thumbnailPath, UriKind.Absolute);

            if (File.Exists(thumbnailPath))
            {
                thumbnail = new BitmapImage();
                thumbnail.BeginInit();
                thumbnail.DecodePixelHeight = 50;
                thumbnail.StreamSource = new MemoryStream(File.ReadAllBytes(thumbnailPath));
                thumbnail.EndInit();
                thumbnail.Freeze();

                _thumbnailCache[composer.ComposerId] = thumbnail;

                return thumbnail;
            }

            return CreateThumbnail(composer);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ComposerToThumbnail((Composer)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        internal static void ClearThumbnailCache()
        {
            _thumbnailCache.Clear();

            var thumbnailDirectory = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create)}\Music Timeline\Resources\Thumbnails\";
            var thumbnailPaths = Directory.EnumerateFiles(thumbnailDirectory).ToArray();

            foreach (var thumbnailPath in thumbnailPaths)
            {
                File.Delete(thumbnailPath);
            }
        }

        private static BitmapImage CreateThumbnail(Composer composer)
        {
            BitmapImage thumbnail = null;
            var thumbnailPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create)}\Music Timeline\Resources\Thumbnails\{composer.ComposerId}.jpg";
            var image = composer.ComposerImages.Select(ci => ci.Bytes).FirstOrDefault();

            if (image != null)
            {
                thumbnail = new BitmapImage();
                thumbnail.CacheOption = BitmapCacheOption.None;
                thumbnail.BeginInit();
                thumbnail.DecodePixelHeight = 40;
                thumbnail.StreamSource = new MemoryStream(image);
                thumbnail.EndInit();
                thumbnail.Freeze();

                var encoder = new JpegBitmapEncoder();
                encoder.QualityLevel = 60;
                encoder.Frames.Add(BitmapFrame.Create(thumbnail));

                using (var stream = new FileStream(thumbnailPath, FileMode.Create))
                {
                    encoder.Save(stream);
                }

                _thumbnailCache[composer.ComposerId] = thumbnail;

                return thumbnail;
            }
            else if (!_thumbnailCache.TryGetValue(0, out thumbnail))
            {
                thumbnailPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create)}\Music Timeline\Resources\Thumbnails\0.jpg";
                var thumbnailUri = new Uri(thumbnailPath, UriKind.Absolute);

                if (File.Exists(thumbnailPath))
                {
                    thumbnail = new BitmapImage();
                    thumbnail.BeginInit();
                    thumbnail.DecodePixelHeight = 50;
                    thumbnail.StreamSource = new MemoryStream(File.ReadAllBytes(thumbnailPath));
                    thumbnail.EndInit();
                    thumbnail.Freeze();

                    _thumbnailCache[0] = thumbnail;

                    return thumbnail;
                }
                else
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

                    _thumbnailCache[0] = thumbnail;

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