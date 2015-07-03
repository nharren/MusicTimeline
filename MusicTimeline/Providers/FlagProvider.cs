using NathanHarrenstein.MusicTimeline.Models;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.MusicTimeline.Providers
{
    public enum FlagSize { Small, Large }

    public static class FlagProvider
    {
        private static readonly Dictionary<string, BitmapImage> smallImageDictionary = new Dictionary<string, BitmapImage>();
        private static readonly Dictionary<string, BitmapImage> largeImageDictionary = new Dictionary<string, BitmapImage>();

        public static Flag GetFlag(string nationality, FlagSize flagSize)
        {
            var flag = new Flag();
            flag.Image = GetImage(nationality, flagSize);
            flag.Nationality = nationality;

            return flag;
        }

        private static BitmapImage GetImage(string nationality, FlagSize flagSize)
        {
            var imagePath = @"{0}\Resources\Flags\{1}\{2}.png";

            if (nationality == null)
            {
                nationality = "Unknown";
            }

            var image = (BitmapImage)null;
            var decodeHeight = 0;
            var decodeWidth = 0;
            var uri = (Uri)null;

            if (flagSize == FlagSize.Small)
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
    }
}