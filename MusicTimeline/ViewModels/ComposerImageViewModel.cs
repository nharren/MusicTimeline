using NathanHarrenstein.MusicDB;
using NathanHarrenstein.MusicTimeline.Utilities;
using System.Windows.Media.Imaging;

namespace NathanHarrenstein.MusicTimeline.ViewModels
{
    public class ComposerImageViewModel
    {
        private readonly BitmapImage _bitmapImage;
        private readonly ComposerImage _composerImage;

        public ComposerImageViewModel(ComposerImage composerImage)
        {
            _bitmapImage = ImageUtility.FromBytes(composerImage.Image);
            _composerImage = composerImage;
        }

        public BitmapImage BitmapImage
        {
            get
            {
                return _bitmapImage;
            }
        }

        public ComposerImage ComposerImage
        {
            get
            {
                return _composerImage;
            }
        }
    }
}