using System.EDTF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NathanHarrenstein.Timeline
{
    public class LinearGradientPanel : Canvas, IPan
    {
        public static readonly DependencyProperty DatesProperty = DependencyProperty.Register("Dates", typeof(ExtendedDateTimeInterval), typeof(LinearGradientPanel), new PropertyMetadata(default(ExtendedDateTimeInterval), DatesChanged));
        public static readonly DependencyProperty GradientStopsProperty = DependencyProperty.Register("GradientStops", typeof(GradientStopCollection), typeof(LinearGradientPanel), new PropertyMetadata(default(GradientStopCollection), GradientStopsChanged));
        public static readonly DependencyProperty RulerProperty = DependencyProperty.Register("Ruler", typeof(TimeRuler), typeof(LinearGradientPanel), new PropertyMetadata(default(TimeRuler), RulerChanged));

        private LinearGradientBrush gradientBrush;
        private TranslateTransform transform;

        public LinearGradientPanel()
        {
            transform = new TranslateTransform();

            RenderTransform = transform;
            ClipToBounds = false;
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

        public GradientStopCollection GradientStops
        {
            get
            {
                return (GradientStopCollection)GetValue(GradientStopsProperty);
            }

            set
            {
                SetValue(GradientStopsProperty, value);
            }
        }

        public TimeRuler Ruler
        {
            get
            {
                return (TimeRuler)GetValue(RulerProperty);
            }

            set
            {
                SetValue(RulerProperty, value);
            }
        }

        public Vector CoercePan(Vector delta)
        {
            return delta;
        }

        public void CreateGradientBrush()
        {
            gradientBrush = new LinearGradientBrush();
            gradientBrush.StartPoint = new Point(0.0, 0.5);
            gradientBrush.EndPoint = new Point(1.0, 0.5);
            gradientBrush.GradientStops = GradientStops;

            Background = gradientBrush;
        }

        public void Pan(Vector delta)
        {
            transform.X -= delta.X;
        }

        private static void DatesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                return;
            }

            var gradientPanel = (LinearGradientPanel)d;
            gradientPanel.UpdateWidth();
        }

        private static void GradientStopsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var gradientPanel = (LinearGradientPanel)d;

            gradientPanel.CreateGradientBrush();
        }

        private static void RulerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                return;
            }

            var gradientPanel = (LinearGradientPanel)d;
            gradientPanel.UpdateWidth();
        }

        private void UpdateWidth()
        {
            if (Ruler == null || Dates == null)
            {
                return;
            }

            Width = Ruler.ToPixels(Dates);

            UpdateLayout();
        }
    }
}