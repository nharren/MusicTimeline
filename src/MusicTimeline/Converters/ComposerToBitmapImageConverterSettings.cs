using System.Windows;

namespace NathanHarrenstein.MusicTimeline.Converters
{
    public class ComposerToBitmapImageConverterSettings : DependencyObject
    {
        public ComposerToBitmapImageConverterSettings()
        {

        }

        public ComposerToBitmapImageConverterSettings(int composerImageId, bool isThumbnail)
        {
            ComposerImageId = composerImageId;
            IsThumbnail = isThumbnail;
        }

        public int ComposerImageId
        {
            get
            {
                return (int)GetValue(ComposerImageIdProperty);
            }

            set
            {
                SetValue(ComposerImageIdProperty, value);
            }
        }

        public static readonly DependencyProperty ComposerImageIdProperty = DependencyProperty.Register("ComposerImageId", typeof(int), typeof(ComposerToBitmapImageConverterSettings), new PropertyMetadata(-1));


        public bool IsThumbnail
        {
            get
            {
                return (bool)GetValue(IsThumbnailProperty);
            }

            set
            {
                SetValue(IsThumbnailProperty, value);
            }
        }

        public static readonly DependencyProperty IsThumbnailProperty = DependencyProperty.Register("IsThumbnail", typeof(bool), typeof(ComposerToBitmapImageConverterSettings), new PropertyMetadata(false));


    }
}