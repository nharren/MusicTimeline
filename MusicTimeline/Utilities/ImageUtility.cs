using System.IO;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.MusicTimeline.Utilities
{
    internal static class ImageUtility
    {
        internal static BitmapImage FromBytes(byte[] bytes)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(bytes);
            bitmapImage.EndInit();

            return bitmapImage;
        }
    }
}