using NathanHarrenstein.MusicTimeline.ClassicalMusicDb;
using NathanHarrenstein.MusicTimeline.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.MusicTimeline.Converters
{
    internal class ComposerImageToBitmapConverter : IValueConverter
    {
        private Dictionary<int, BitmapImage> cache = new Dictionary<int, BitmapImage>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ComposerImageToBitmap((ComposerImage)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private BitmapImage ComposerImageToBitmap(ComposerImage composerImage)
        {
            if (composerImage == null)
            {
                return null;
            }

            BitmapImage bitmapImage;

            if (!cache.TryGetValue(composerImage.GetHashCode(), out bitmapImage))
            {
                bitmapImage = ImageUtility.CreateBitmapImage(composerImage.Bytes);

                cache[composerImage.GetHashCode()] = bitmapImage;
            }

            return bitmapImage;
        }
    }
}