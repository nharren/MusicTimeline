using NathanHarrenstein.MusicDB;
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
        private Dictionary<short, BitmapImage> _bitmapCache = new Dictionary<short, BitmapImage>();

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

            var bitmap = (BitmapImage)null;

            if (!_bitmapCache.TryGetValue(composerImage.ID, out bitmap))
            {
                bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(composerImage.Image);
                bitmap.EndInit();
                bitmap.Freeze();

                _bitmapCache[composerImage.ID] = bitmap;

                return bitmap;
            }

            return bitmap;
        }
    }
}