using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.MusicTimeline.Converters
{
    public class NationalityToImageConverter : IValueConverter
    {
        private static readonly Dictionary<string, BitmapImage> smallImageDictionary = new Dictionary<string, BitmapImage>();
        private static readonly Dictionary<string, BitmapImage> largeImageDictionary = new Dictionary<string, BitmapImage>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nationality = (string)value;
            var flagSize = (string)parameter;
            var imagePath = @"{0}\Resources\Flags\{1}\{2}.png";

            if (nationality == null)
            {
                nationality = "Unknown";
            }

            var image = (BitmapImage)null;
            var decodeHeight = 0;
            var decodeWidth = 0;
            var uri = (Uri)null;

            if (flagSize == "Small")
            {
                if (smallImageDictionary.TryGetValue(nationality, out image))
                {
                    return image;
                }
                else
                {
                    uri = new Uri(string.Format(imagePath, Environment.CurrentDirectory, 16, nationality), UriKind.Absolute);
                    decodeHeight = 16;
                    decodeWidth = 16;
                }
            }
            else
            {
                if (largeImageDictionary.TryGetValue(nationality, out image))
                {
                    return image;
                }
                else
                {
                    uri = new Uri(string.Format(imagePath, Environment.CurrentDirectory, 32, nationality), UriKind.Absolute);
                    decodeHeight = 32;
                    decodeWidth = 32;
                }
            }

            image = new BitmapImage();
            image.BeginInit();
            image.DecodePixelHeight = decodeHeight;
            image.DecodePixelWidth = decodeWidth;
            image.UriSource = uri;
            image.EndInit();
            image.Freeze();

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}