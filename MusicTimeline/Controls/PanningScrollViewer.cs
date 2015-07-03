using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NathanHarrenstein.MusicTimeline.Controls
{
    public class PanningScrollViewer : ScrollViewer
    {
        public static DependencyProperty CurrentHorizontalOffsetProperty = DependencyProperty.Register("CurrentHorizontalOffsetOffset", typeof(double), typeof(PanningScrollViewer), new PropertyMetadata(new PropertyChangedCallback(OnHorizontalChanged)));
        public static DependencyProperty CurrentVerticalOffsetProperty = DependencyProperty.Register("CurrentVerticalOffset", typeof(double), typeof(PanningScrollViewer), new PropertyMetadata(new PropertyChangedCallback(OnVerticalChanged)));

        private Point scrollOffset;
        private Point scrollOrigin;

        public PanningScrollViewer()
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
        }

        public double CurrentHorizontalOffset
        {
            get
            {
                return (double)GetValue(CurrentHorizontalOffsetProperty);
            }
            set
            {
                SetValue(CurrentHorizontalOffsetProperty, value);
            }
        }

        public double CurrentVerticalOffset
        {
            get
            {
                return (double)GetValue(CurrentVerticalOffsetProperty);
            }
            set
            {
                SetValue(CurrentVerticalOffsetProperty, value);
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (IsMouseOver)
            {
                scrollOrigin = e.GetPosition(this);
                scrollOffset = new Point(HorizontalOffset, VerticalOffset);

                if (ExtentWidth > ViewportWidth || ExtentHeight > ViewportHeight)
                {
                    Cursor = Cursors.ScrollAll;
                }
                else
                {
                    Cursor = Cursors.Arrow;
                }

                CaptureMouse();
            }

            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (IsMouseCaptured)
            {
                Cursor = Cursors.Arrow;

                ReleaseMouseCapture();
            }

            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (IsMouseCaptured)
            {
                var point = e.GetPosition(this);
                var delta = new Point(scrollOrigin.X - point.X, scrollOrigin.Y - point.Y);

                ScrollToHorizontalOffset(scrollOffset.X + delta.X);
                ScrollToVerticalOffset(scrollOffset.Y + delta.Y);
            }

            base.OnMouseMove(e);
        }

        private static void OnHorizontalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewer = (PanningScrollViewer)d;

            viewer.ScrollToHorizontalOffset((double)e.NewValue);
        }

        private static void OnVerticalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewer = (PanningScrollViewer)d;

            viewer.ScrollToVerticalOffset((double)e.NewValue);
        }
    }
}