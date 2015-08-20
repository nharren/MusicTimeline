using System.EDTF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NathanHarrenstein.Timeline
{
    public class BackgroundPanel : Grid, IPan
    {
        public static readonly DependencyProperty BackgroundImageProperty = DependencyProperty.Register("BackgroundImage", typeof(ImageSource), typeof(BackgroundPanel), new PropertyMetadata(new PropertyChangedCallback(BackgroundImage_PropertyChanged)));
        public static readonly DependencyProperty DatesProperty = DependencyProperty.Register("Dates", typeof(ExtendedDateTimeInterval), typeof(BackgroundPanel));

        private ImageBrush _backgroundImageBrush;
        private double _horizontalOffset;
        private double _verticalOffset;

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

        public ExtendedDateTimeInterval Dates
        {
            get
            {
                return (ExtendedDateTimeInterval)GetValue(DatesProperty);
            }

            set
            {
                SetValue(DatesProperty, value);
            }
        }

        public Vector CoercePan(Vector delta)
        {
            return delta;
        }

        public void Pan(Vector delta)
        {
            _horizontalOffset += delta.X;
            _verticalOffset += delta.Y;

            UpdateBackgroundImageBrush();
        }

        private static void BackgroundImage_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BackgroundPanel)d).CreateImageBrush();
        }

        private void CreateImageBrush()
        {
            _backgroundImageBrush = new ImageBrush();
            _backgroundImageBrush.ImageSource = BackgroundImage;
            _backgroundImageBrush.TileMode = TileMode.Tile;
            _backgroundImageBrush.ViewportUnits = BrushMappingMode.Absolute;

            UpdateBackgroundImageBrush();

            Background = _backgroundImageBrush;
        }

        private void UpdateBackgroundImageBrush()
        {
            var x = -(_horizontalOffset % BackgroundImage.Width);
            var y = -(_verticalOffset % BackgroundImage.Height);

            _backgroundImageBrush.Viewport = new Rect(x, y, BackgroundImage.Width, BackgroundImage.Height);
        }
    }
}