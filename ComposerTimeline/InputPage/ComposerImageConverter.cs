using Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.ComposerTimeline
{
    internal class ComposerImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var composerImages = (HashSet<ComposerImage>)value;
            var images = new List<BitmapImage>();

            foreach (var composerImage in composerImages)
            {
                var memoryStream = new MemoryStream(composerImage.Image);
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = memoryStream;
                image.EndInit();

                images.Add(image);
            }

            return images;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}