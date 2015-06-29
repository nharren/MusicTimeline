using Database;
using System.IO;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.ComposerTimeline
{
    internal static class ComposerImageExtensions
    {
        internal static BitmapImage ToBitmapImage(this ComposerImage composerImage)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(composerImage.Image);
            bitmapImage.EndInit();

            return bitmapImage;
        }
    }
}