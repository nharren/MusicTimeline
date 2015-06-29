using Database;
using System.IO;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.ComposerTimeline
{
    internal static class ComposerImageUtility
    {
        internal static BitmapImage ToBitmapImage(ComposerImage composerImage)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(composerImage.Image);
            bitmapImage.EndInit();

            return bitmapImage;
        }
    }
}