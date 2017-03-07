/*
 1. When the left mouse button is pressed, 

 */



using NathanHarrenstein.Timeline.Input;
using System;
using System.Threading;
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
        private bool isDisposed;
        private MouseHook mouseHook;
        private Point cursorPosition;
        private Vector velocity;
        private DispatcherTimer timer;

        private double flickDuration = 0.15;

        public double FlickDuration
        {
            get { return flickDuration; }
            set { flickDuration = value; }
        }


        static ScrollingPanel()
        {
            BackgroundProperty.AddOwner(typeof(ScrollingPanel), new FrameworkPropertyMetadata(Brushes.Transparent));
        }

        public ScrollingPanel()
        {
            mouseHook = new MouseHook();
            mouseHook.MouseHorizontalWheel += EventPanel_MouseHorizontalWheel;
            mouseHook.StartMouseHook();

            timer = new DispatcherTimer(DispatcherPriority.Background);
            timer.Interval = TimeSpan.FromMilliseconds(15);
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

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (IsMouseCaptured)
            {
                var newCursorPosition = InputManager.Current.PrimaryMouseDevice.GetPosition(this);
                var displacement = newCursorPosition - cursorPosition;
                cursorPosition = newCursorPosition;

                RequestScroll(-displacement);
                velocity = displacement / timer.Interval.TotalSeconds;

                return;
            }

            time += timer.Interval.TotalSeconds;

            if (time >= FlickDuration)
            {
                timer.Stop();

                return;
            }

            velocity = releaseVelocity - releaseVelocity * time * (1 / FlickDuration);

            RequestScroll(-velocity * timer.Interval.TotalSeconds);
        }

        private double time;
        private Vector releaseVelocity;

        ~ScrollingPanel()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void RequestScroll(Vector displacement)
        {
            if (scrollRequestEvent != null)
            {
                RaiseEvent(new ScrollingEventArgs(displacement, scrollRequestEvent, this));
            }
        }

        protected virtual void Dispose(bool disposeManagedObjects)
        {
            if (isDisposed)
            {
                return;
            }

            mouseHook.Dispose();

            isDisposed = true;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            cursorPosition = e.GetPosition(this);

            Cursor = Cursors.ScrollAll;

            timer.Start();

            CaptureMouse();
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            Cursor = Cursors.Arrow;

            releaseVelocity = velocity;
            time = 0;

            ReleaseMouseCapture();
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            cursorPosition = e.GetPosition(this);

            RequestScroll(new Vector(0, -e.Delta));

            base.OnMouseWheel(e);
        }

        private void EventPanel_MouseHorizontalWheel(object sender, MouseHorizontalWheelEventArgs e)
        {
            RequestScroll(new Vector(e.Delta, 0));
        }
    }
}