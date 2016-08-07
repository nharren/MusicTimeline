using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.MusicTimeline.Utilities
{
    public static class ImageUtility
    {
        //public static BitmapImage CreateBitmapImage(byte[] bytes, int? width = null, int? height = null)
        //{
        //    using (var memoryStream = new MemoryStream(bytes))
        //    {
        //        var bitmapImage = new BitmapImage();
        //        bitmapImage.BeginInit();
        //        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

        //        if (width != null)
        //        {
        //            bitmapImage.DecodePixelWidth = width.Value;
        //        }

        //        if (height != null)
        //        {
        //            bitmapImage.DecodePixelHeight = height.Value;
        //        }

        //        bitmapImage.StreamSource = memoryStream;
        //        bitmapImage.EndInit();
        //        bitmapImage.Freeze();

        //        return bitmapImage;
        //    }
        //}

        public static BitmapImage CreateBitmapImage(Uri uri, int? width = null, int? height = null)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

            if (width != null)
            {
                bitmapImage.DecodePixelWidth = width.Value;
            }

            if (height != null)
            {
                bitmapImage.DecodePixelHeight = height.Value;
            }

            bitmapImage.UriSource = uri;
            bitmapImage.EndInit();

            return bitmapImage;
        }

        //public static BitmapImage CreateBitmapImage(Stream stream, int? width = null, int? height = null)
        //{
        //    using (stream)
        //    {
        //        var bitmapImage = new BitmapImage();
        //        bitmapImage.BeginInit();
        //        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

        //        if (width != null)
        //        {
        //            bitmapImage.DecodePixelWidth = width.Value;
        //        }

        //        if (height != null)
        //        {
        //            bitmapImage.DecodePixelHeight = height.Value;
        //        }

        //        bitmapImage.StreamSource = stream;
        //        bitmapImage.EndInit();

        //        return bitmapImage;
        //    }
        //}
    }
}