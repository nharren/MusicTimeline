using NathanHarrenstein.MusicTimeline.Data;
using NathanHarrenstein.MusicTimeline.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.MusicTimeline.Converters
{
    internal class ComposerToBitmapImageConverter : IValueConverter
    {
        private static BitmapImage defaultImage;
        private static BitmapImage defaultImageThumbnail;
        private static Dictionary<int, BitmapImage> cache;
        private static Dictionary<int, BitmapImage> thumbnailCache;
        private static readonly string cacheDirectory;

        static ComposerToBitmapImageConverter()
        {
            cache = new Dictionary<int, BitmapImage>();
            thumbnailCache = new Dictionary<int, BitmapImage>();

            var applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create);
            cacheDirectory = $@"{applicationDataPath}\Music Timeline\Resources\ComposerImages";

            if (!Directory.Exists(cacheDirectory))
            {
                Directory.CreateDirectory(cacheDirectory);
            }
        }

        public BitmapImage Convert(Composer composer, ComposerToBitmapImageConverterSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (settings.ComposerImageId == -1)
            {
                settings.ComposerImageId = TryFindComposerImageId(composer);
            }

            return SearchMemoryCache(composer, settings) ?? SearchFileCache(composer, settings) ?? CreateBitmapImage(composer, settings);
        }

        private int TryFindComposerImageId(Composer composer)
        {
            var directory = $@"{cacheDirectory}\{composer.ComposerId}";

            if (Directory.Exists(directory))
            {
                var initialImagePath = Directory.EnumerateFiles(directory).FirstOrDefault();
                
                if (initialImagePath != null)
                {
                    initialImagePath = Path.GetFileNameWithoutExtension(initialImagePath);
                    initialImagePath = Path.GetFileNameWithoutExtension(initialImagePath);

                    return int.Parse(initialImagePath);
                }
            }
            else
            {
                App.ClassicalMusicContext.LoadProperty(composer, "ComposerImages");

                var initialImage = composer.ComposerImages.FirstOrDefault();

                if (initialImage != null)
                {
                    return initialImage.ComposerImageId;
                }
            }

            return -1;
        }

        private BitmapImage SearchFileCache(Composer composer, ComposerToBitmapImageConverterSettings settings)
        {
            BitmapImage bitmapImage;

            if (settings.IsThumbnail)
            {
                var thumbnailPath = $@"{cacheDirectory}\{composer.ComposerId}\{settings.ComposerImageId}.thumb.jpg";

                if (File.Exists(thumbnailPath))
                {
                    bitmapImage = ImageUtility.CreateBitmapImage(File.ReadAllBytes(thumbnailPath), height: 50);

                    thumbnailCache[settings.ComposerImageId] = bitmapImage;

                    return bitmapImage;
                }
            }
            else
            {
                var path = $@"{cacheDirectory}\{composer.ComposerId}\{settings.ComposerImageId}.jpg";

                if (File.Exists(path))
                {
                    bitmapImage = ImageUtility.CreateBitmapImage(File.ReadAllBytes(path));

                    cache[settings.ComposerImageId] = bitmapImage;

                    return bitmapImage;
                }
            }

            return null;
        }

        private BitmapImage SearchMemoryCache(Composer composer, ComposerToBitmapImageConverterSettings settings)
        {
            BitmapImage bitmapImage;

            if (settings.IsThumbnail)
            {
                if (thumbnailCache.TryGetValue(settings.ComposerImageId, out bitmapImage))
                {
                    return bitmapImage;
                }
            }
            else
            {
                if (cache.TryGetValue(settings.ComposerImageId, out bitmapImage))
                {
                    return bitmapImage;
                }
            }

            return null;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert((Composer)value, (ComposerToBitmapImageConverterSettings)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        internal void ClearCache()
        {
            if (Directory.Exists(cacheDirectory))
            {
                Directory.Delete(cacheDirectory, true);
            }            
        }

        private BitmapImage CreateBitmapImage(Composer composer, ComposerToBitmapImageConverterSettings settings)
        {
            App.ClassicalMusicContext.LoadProperty(composer, "ComposerImages");

            var composerImage = composer.ComposerImages.FirstOrDefault(ci => ci.ComposerImageId == settings.ComposerImageId);

            if (settings.IsThumbnail)
            {
                return CreateBitmapImageThumbnail(composerImage);
            }
            else
            {
                return CreateBitmapImage(composerImage);
            }
            
        }

        private BitmapImage CreateBitmapImage(ComposerImage composerImage)
        {
            if (composerImage == null)
            {
                return defaultImage ?? CreateDefaultBitmapImage();
            }

            var bytes = StreamUtility.ReadToEnd(App.ClassicalMusicContext.GetReadStream(composerImage).Stream);

            var bitmapImage = ImageUtility.CreateBitmapImage(bytes);

            SaveToFileCache(bitmapImage, composerImage);
            cache[composerImage.ComposerImageId] = bitmapImage;

            return bitmapImage;
        }

        private void SaveToFileCache(BitmapImage bitmapImage, ComposerImage composerImage)
        {
            var composerCacheDirectory = $@"{cacheDirectory}\{composerImage.ComposerId}";
            var path = $@"{composerCacheDirectory}\{composerImage.ComposerImageId}.jpg";

            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            Directory.CreateDirectory(composerCacheDirectory);

            using (var writeStream = new FileStream(path, FileMode.Create))
            {
                encoder.Save(writeStream);
            }
        }

        private BitmapImage CreateDefaultBitmapImage()
        {
            var thumbnailUri = new Uri($@"pack://application:,,,/Resources/Composers/Unknown.jpg", UriKind.Absolute);
            var bitmapImage = new BitmapImage(thumbnailUri);

            defaultImage = bitmapImage;

            return bitmapImage;
        }

        private BitmapImage CreateBitmapImageThumbnail(ComposerImage composerImage)
        {
            if (composerImage == null)
            {
                return defaultImageThumbnail ?? CreateDefaultBitmapImageThumbnail();
            }

            var bytes = StreamUtility.ReadToEnd(App.ClassicalMusicContext.GetReadStream(composerImage).Stream);

            var bitmapImage = ImageUtility.CreateBitmapImage(bytes, height: 50);

            SaveToThumbnailFileCache(bitmapImage, composerImage);
            thumbnailCache[composerImage.ComposerImageId] = bitmapImage;

            return bitmapImage;
        }

        private void SaveToThumbnailFileCache(BitmapImage bitmapImage, ComposerImage composerImage)
        {
            var composerCacheDirectory = $@"{cacheDirectory}\{composerImage.ComposerId}";
            var thumbnailPath = $@"{composerCacheDirectory}\{composerImage.ComposerImageId}.thumb.jpg";

            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            Directory.CreateDirectory(composerCacheDirectory);

            using (var writeStream = new FileStream(thumbnailPath, FileMode.Create))
            {
                encoder.Save(writeStream);
            }
        }

        private BitmapImage CreateDefaultBitmapImageThumbnail()
        {
            var thumbnailUri = new Uri($@"pack://application:,,,/Resources/Composers/Unknown.jpg", UriKind.Absolute);
            var bitmapImage = ImageUtility.CreateBitmapImage(thumbnailUri, height: 50);
            
            defaultImageThumbnail = bitmapImage;

            return bitmapImage;
        }
    }
}