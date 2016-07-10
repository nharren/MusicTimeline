using NathanHarrenstein.MusicTimeline.ClassicalMusicDb;
using NathanHarrenstein.MusicTimeline.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
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
        private static Dictionary<int, BitmapImage> cache = new Dictionary<int, BitmapImage>();

        public static BitmapImage ComposerToThumbnail(Composer composer)
        {
            BitmapImage thumbnail;

            if (cache.TryGetValue(composer.ComposerId, out thumbnail))
            {
                return cache[composer.ComposerId];
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
                thumbnail = ImageUtility.CreateBitmapImage(File.ReadAllBytes(thumbnailPath), height: 50);

                cache[composer.ComposerId] = thumbnail;

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
            cache.Clear();

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
            var context = new ClassicalMusicContext(new Uri("http://www.harrenstein.com/ClassicalMusic/ClassicalMusic.svc"));
            var image = context.Execute<ComposerImage>(new Uri($"http://www.harrenstein.com/ClassicalMusic/ClassicalMusic.svc/Composers({composer.ComposerId})/ComposerImages")).Select(c => c.Bytes).FirstOrDefault();

            if (image != null)
            {
                thumbnail = ImageUtility.CreateBitmapImage(image, height: 40);

                var encoder = new JpegBitmapEncoder();
                encoder.QualityLevel = 60;
                encoder.Frames.Add(BitmapFrame.Create(thumbnail));

                using (var stream = new FileStream(thumbnailPath, FileMode.Create))
                {
                    encoder.Save(stream);
                }

                cache[composer.ComposerId] = thumbnail;

                return thumbnail;
            }
            else if (!cache.TryGetValue(0, out thumbnail))
            {
                thumbnailPath = $@"pack://application:,,,/Resources/Composers/Unknown.jpg";
                var thumbnailUri = new Uri(thumbnailPath, UriKind.Absolute);

                thumbnail = new BitmapImage(thumbnailUri);

                cache[0] = thumbnail;

                return thumbnail;
            }
            else
            {
                return thumbnail;
            }
        }
    }
}