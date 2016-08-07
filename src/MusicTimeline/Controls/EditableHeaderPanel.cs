using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class EditableHeaderPanel : Control
    {
        public static readonly DependencyProperty ButtonForegroundProperty = DependencyProperty.Register("ButtonForeground", typeof(Brush), typeof(EditableHeaderPanel), new PropertyMetadata(SystemColors.ControlTextBrush));
        public static readonly DependencyProperty ButtonSizeProperty = DependencyProperty.Register("ButtonSize", typeof(double), typeof(EditableHeaderPanel), new PropertyMetadata(SystemFonts.MessageFontSize));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(EditableHeaderPanel), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty UnderlineForegroundProperty = DependencyProperty.Register("UnderlineForeground", typeof(Brush), typeof(EditableHeaderPanel), new PropertyMetadata(SystemColors.ControlLightBrush));
        public static readonly DependencyProperty UnderlineOffsetProperty = DependencyProperty.Register("UnderlineOffset", typeof(GridLength), typeof(EditableHeaderPanel), new PropertyMetadata(new GridLength(2.0)));
        public static readonly DependencyProperty UnderlineThicknessProperty = DependencyProperty.Register("UnderlineThickness", typeof(double), typeof(EditableHeaderPanel), new PropertyMetadata(1.0));
        private Button editButton;

        static EditableHeaderPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EditableHeaderPanel), new FrameworkPropertyMetadata(typeof(EditableHeaderPanel)));
        }

        public event RoutedEventHandler ButtonClick;

        public Brush ButtonForeground
        {
            get
            {
                return (Brush)GetValue(ButtonForegroundProperty);
            }

            set
            {
                SetValue(ButtonForegroundProperty, value);
            }
        }

        public double ButtonSize
        {
            get
            {
                return (double)GetValue(ButtonSizeProperty);
            }

            set
            {
                SetValue(ButtonSizeProperty, value);
            }
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

        public Thickness ButtonMargin
        {
            get
            {
                return (Thickness)GetValue(ButtonMarginProperty);
            }

            set
            {
                SetValue(ButtonMarginProperty, value);
            }
        }

        public static readonly DependencyProperty ButtonMarginProperty = DependencyProperty.Register("ButtonMargin", typeof(Thickness), typeof(EditableHeaderPanel), new PropertyMetadata(new Thickness()));



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

        public override void OnApplyTemplate()
        {
            if (Template.VisualTree != null)
            {
                return;
            }

            editButton = (Button)Template.FindName("editButton", this);
            editButton.Click += editButton_Click;
        }

        protected virtual void OnButtonClick(RoutedEventArgs e)
        {
            ButtonClick?.Invoke(this, e);
        }


        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;

            OnButtonClick(e);
        }
    }
}