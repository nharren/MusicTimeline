using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.ComposerTimeline.UI.Data.Providers
{
    public enum FlagSize { Small, Large }

    public static class FlagDataProvider
    {
        private static readonly Dictionary<string, BitmapImage> smallFlagImageDictionary = new Dictionary<string, BitmapImage>();
        private static readonly Dictionary<string, BitmapImage> largeFlagImageDictionary = new Dictionary<string, BitmapImage>();

        private static readonly Dictionary<string, string> flagPathDictionary = new Dictionary<string, string>
        {
            { "German", @"UI\Resources\Flags\{0}\Germany.png" },
            { "Italian", @"UI\Resources\Flags\{0}\Italy.png" },
            { "French", @"UI\Resources\Flags\{0}\France.png" },
            { "English", @"UI\Resources\Flags\{0}\United Kingdom (Great Britain).png" },
            { "Polish", @"UI\Resources\Flags\{0}\Poland.png" },
            { "Irish", @"UI\Resources\Flags\{0}\Ireland.png" },
            { "Russian", @"UI\Resources\Flags\{0}\Russian Federation.png" },
            { "American", @"UI\Resources\Flags\{0}\United States of America (USA).png" },
            { "Spanish", @"UI\Resources\Flags\{0}\Spain.png" },
            { "Czech", @"UI\Resources\Flags\{0}\Czech Republic.png" },
            { "Belgian", @"UI\Resources\Flags\{0}\Belgium.png" },
            { "Austrian", @"UI\Resources\Flags\{0}\Austria.png" },
            { "Flemish", @"UI\Resources\Flags\{0}\Flanders (Belgium).png" },
            { "Occitan", @"UI\Resources\Flags\{0}\Occitania.png" },
            { "Portuguese", @"UI\Resources\Flags\{0}\Portugal.png" },
            { "Dutch", @"UI\Resources\Flags\{0}\Netherlands.png" }
        };

        private static BitmapImage LargeDefaultFlagImage;
        private static BitmapImage SmallDefaultFlagImage;

        public static FlagData GetFlagData(string nationality, FlagSize flagSize)
        {
            var flagData = new FlagData();
            flagData.Image = GetFlagImage(nationality, flagSize);
            flagData.Nationality = nationality;

            return flagData;
        }

        private static BitmapImage GetFlagImage(string nationality, FlagSize flagSize)
        {
            var source = (string)null;

            if (nationality == null || !flagPathDictionary.TryGetValue(nationality, out source))
            {
                if (flagSize == FlagSize.Small)
                {
                    if (SmallDefaultFlagImage == null)
                    {
                        SmallDefaultFlagImage = new BitmapImage();
                        SmallDefaultFlagImage.BeginInit();
                        SmallDefaultFlagImage.DecodePixelHeight = 16;
                        SmallDefaultFlagImage.DecodePixelWidth = 16;
                        SmallDefaultFlagImage.UriSource = new Uri(@"UI\Resources\Flags\16\Unknown.png", UriKind.Relative);
                        SmallDefaultFlagImage.EndInit();
                        SmallDefaultFlagImage.Freeze();
                    }

                    return SmallDefaultFlagImage;
                }
                else
                {
                    if (LargeDefaultFlagImage == null)
                    {
                        LargeDefaultFlagImage = new BitmapImage();
                        LargeDefaultFlagImage.BeginInit();
                        LargeDefaultFlagImage.DecodePixelHeight = 32;
                        LargeDefaultFlagImage.DecodePixelWidth = 32;
                        LargeDefaultFlagImage.UriSource = new Uri(@"UI\Resources\Flags\{0}\Unknown.png", UriKind.Relative);
                        LargeDefaultFlagImage.EndInit();
                        LargeDefaultFlagImage.Freeze();
                    }

                    return LargeDefaultFlagImage;
                }
            }
            else
            {
                var flagImage = (BitmapImage)null;
                int decodeHeight;
                int decodeWidth;
                var uri = (Uri)null;

                if (flagSize == FlagSize.Small)
                {
                    if (smallFlagImageDictionary.TryGetValue(nationality, out flagImage))
                    {
                        return flagImage;
                    }
                    else
                    {
                        uri = new Uri(string.Format(source, 16), UriKind.Relative);
                        decodeHeight = 16;
                        decodeWidth = 16;
                    }
                }
                else
                {
                    if (largeFlagImageDictionary.TryGetValue(nationality, out flagImage))
                    {
                        return flagImage;
                    }
                    else
                    {
                        uri = new Uri(string.Format(source, 32), UriKind.Relative);
                        decodeHeight = 32;
                        decodeWidth = 32;
                    }
                }

                flagImage = new BitmapImage();
                flagImage.BeginInit();
                flagImage.DecodePixelHeight = decodeHeight;
                flagImage.DecodePixelWidth = decodeWidth;
                flagImage.UriSource = uri;
                flagImage.EndInit();
                flagImage.Freeze();

                return flagImage;
            }
        }
    }
}