using System.EDTF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NathanHarrenstein.Timeline
{
    public class BackgroundPanel : Grid, IPan
    {
        public static readonly DependencyProperty BackgroundImageProperty = DependencyProperty.Register("BackgroundImage", typeof(ImageSource), typeof(BackgroundPanel), new PropertyMetadata(new PropertyChangedCallback(BackgroundImage_PropertyChanged)));

        private ImageBrush imageBrush;
        private double horizontalOffset;
        private double verticalOffset;

        public ImageSource BackgroundImage
        {
            get
            {
                return (ImageSource)GetValue(BackgroundImageProperty);
            }

            set
            {
                SetValue(BackgroundImageProperty, value);
            }
        }


        public Vector CoercePan(Vector delta)
        {
            return delta;
        }

        public void Pan(Vector delta)
        {
            horizontalOffset += delta.X;
            verticalOffset += delta.Y;

            UpdateBackgroundImageBrush();
        }

        private static void BackgroundImage_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BackgroundPanel)d).CreateImageBrush();
        }

        private void CreateImageBrush()
        {
            imageBrush = new ImageBrush();
            imageBrush.ImageSource = BackgroundImage;
            imageBrush.TileMode = TileMode.Tile;
            imageBrush.ViewportUnits = BrushMappingMode.Absolute;

            UpdateBackgroundImageBrush();

            Background = imageBrush;
        }

        private void UpdateBackgroundImageBrush()
        {
            var x = -(horizontalOffset % BackgroundImage.Width);
            var y = -(verticalOffset % BackgroundImage.Height);

            imageBrush.Viewport = new Rect(x, y, BackgroundImage.Width, BackgroundImage.Height);
        }
    }
}