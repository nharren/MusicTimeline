using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class EditableHeaderPanel : Control
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(EditableHeaderPanel), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty UnderlineForegroundProperty = DependencyProperty.Register("UnderlineForeground", typeof(Brush), typeof(EditableHeaderPanel), new PropertyMetadata(SystemColors.ControlLightBrush));
        public static readonly DependencyProperty UnderlineOffsetProperty = DependencyProperty.Register("UnderlineOffset", typeof(GridLength), typeof(EditableHeaderPanel), new PropertyMetadata(new GridLength(2.0)));
        public static readonly DependencyProperty UnderlineThicknessProperty = DependencyProperty.Register("UnderlineThickness", typeof(double), typeof(EditableHeaderPanel), new PropertyMetadata(1.0));
    

        static EditableHeaderPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EditableHeaderPanel), new FrameworkPropertyMetadata(typeof(EditableHeaderPanel)));
        }

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }

            set
            {
                SetValue(TextProperty, value);
            }
        }

        public Brush UnderlineForeground
        {
            get
            {
                return (Brush)GetValue(UnderlineForegroundProperty);
            }

            set
            {
                SetValue(UnderlineForegroundProperty, value);
            }
        }

        public GridLength UnderlineOffset
        {
            get
            {
                return (GridLength)GetValue(UnderlineOffsetProperty);
            }

            set
            {
                SetValue(UnderlineOffsetProperty, value);
            }
        }

        public double UnderlineThickness
        {
            get
            {
                return (double)GetValue(UnderlineThicknessProperty);
            }

            set
            {
                SetValue(UnderlineThicknessProperty, value);
            }
        }
    }
}