using NathanHarrenstein.MusicTimeline.Converters;
using NathanHarrenstein.MusicTimeline.Data;
using System.IO;

namespace NathanHarrenstein.MusicTimeline.ViewModels
{
    public class ComposerImageViewModel
    {
        private readonly ComposerImage composerImage;
        private readonly ComposerToBitmapImageConverterSettings settings;

        public ComposerImageViewModel(ComposerImage composerImage, bool isThumbnail)
        {
            this.composerImage = composerImage;
            this.settings = new ComposerToBitmapImageConverterSettings(composerImage.ComposerImageId, isThumbnail);
        }

        public ComposerImage ComposerImage
        {
            get
            {
                return composerImage;
            }
        }

        public Composer Composer
        {
            get
            {
                return composerImage.Composer;
            }
        }

        public ComposerToBitmapImageConverterSettings Settings
        {
            get
            {
                return settings;
            }
        }
    }
}