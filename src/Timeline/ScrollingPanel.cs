using NathanHarrenstein.Timeline.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace NathanHarrenstein.Timeline
{
    public class ScrollingPanel : Panel, IDisposable
    {
        private readonly RoutedEvent scrollRequestEvent;
        private double flickDuration = 0.5;
        private double fps = 60;
        private bool isDisposed;
        private Point lastCursorPosition;
        private DateTime lastMoveTime;
        private MouseHook mouseHook;
        private double sensitivity = 1.25;
        private double time;
        private DispatcherTimer timer;
        private Vector velocity;

        static ScrollingPanel()
        {
            BackgroundProperty.AddOwner(typeof(ScrollingPanel), new FrameworkPropertyMetadata(Brushes.Transparent));
        }

        public ScrollingPanel()
        {
            mouseHook = new MouseHook();
            mouseHook.MouseHorizontalWheel += EventPanel_MouseHorizontalWheel;
            mouseHook.StartMouseHook();

            timer = new DispatcherTimer(DispatcherPriority.Normal);
            timer.Interval = TimeSpan.FromSeconds(1 / fps);
            timer.Tick += Timer_Tick;

            var routedEvents = EventManager.GetRoutedEvents();

            foreach (var routedEvent in routedEvents)
            {
                if (routedEvent.Name == "ScrollRequest")
                {
                    scrollRequestEvent = routedEvent;
                }
            }
        }

        ~ScrollingPanel()
        {
            Dispose(false);
        }

        public double FlickDuration
        {
            get { return flickDuration; }
            set { flickDuration = value; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void RequestScroll(Vector displacement)
        {
            if (scrollRequestEvent != null && !double.IsNaN(displacement.X) && !double.IsNaN(displacement.Y))
            {
                RaiseEvent(new ScrollingEventArgs(displacement, scrollRequestEvent, this));
            }
        }

        protected virtual void Dispose(bool disposeManagedObjects)
        {
            if (mouseHook == null || isDisposed)
            {
                return;
            }

            mouseHook.Dispose();

            isDisposed = true;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            timer.Stop();
            time = 0;

            lastCursorPosition = e.GetPosition(this);
            lastMoveTime = DateTime.Now;
            velocity = new Vector();

            Cursor = Cursors.ScrollAll;

            CaptureMouse();
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            Cursor = Cursors.Arrow;

            var moveTime = DateTime.Now;
            var cursorPosition = e.GetPosition(this);

            var displacement = cursorPosition - lastCursorPosition;
            var duration = moveTime - lastMoveTime;

            if (displacement.Length == 0)
            {
                time += duration.TotalSeconds;
            }

            timer.Start();

            ReleaseMouseCapture();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!IsMouseCaptured)
            {
                return;
            }

            var moveTime = DateTime.Now;
            var cursorPosition = e.GetPosition(this);

            var displacement = (cursorPosition - lastCursorPosition) * sensitivity;
            var duration = moveTime - lastMoveTime;
            velocity = displacement / duration.TotalSeconds;

            RequestScroll(-displacement);

            lastMoveTime = DateTime.Now;
            lastCursorPosition = cursorPosition;
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            RequestScroll(new Vector(0, -e.Delta));

            base.OnMouseWheel(e);
        }

        private void EventPanel_MouseHorizontalWheel(object sender, MouseHorizontalWheelEventArgs e)
        {
            RequestScroll(new Vector(e.Delta, 0));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            time += timer.Interval.TotalSeconds;

            if (time >= FlickDuration)
            {
                timer.Stop();

                return;
            }

            velocity -= velocity * time / FlickDuration;

            RequestScroll(-velocity * timer.Interval.TotalSeconds);
        }
    }
}