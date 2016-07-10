using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.MusicTimeline.Utilities
{
    public static class ImageUtility
    {
        public static BitmapImage CreateBitmapImage(byte[] bytes, int? width = null, int? height = null)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();

                if (width != null)
                {
                    bitmapImage.DecodePixelWidth = width.Value;
                }

                if (height != null)
                {
                    bitmapImage.DecodePixelHeight = height.Value;
                }

                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }            
        }
    }
}
