using NathanHarrenstein.Timeline.Input;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NathanHarrenstein.Timeline
{
    public class ScrollingPanel : Panel, IDisposable
    {
        private readonly RoutedEvent scrollRequestEvent;
        private CancellationTokenSource decelerationCancellationTokenSource;
        private TimeSpan decelerationDuration = TimeSpan.FromMilliseconds(500);
        private TimeSpan decelerationInterval = TimeSpan.FromTicks(200);
        private bool isDisposed;
        private MouseHook mouseHook;
        private Point previousCursorPosition;
        private DateTime previousTime;
        private Point originCursorPosition;
        private DateTime originTime;

        static ScrollingPanel()
        {
            BackgroundProperty.AddOwner(typeof(ScrollingPanel), new FrameworkPropertyMetadata(Brushes.Transparent));
        }

        public ScrollingPanel()
        {
            decelerationCancellationTokenSource = new CancellationTokenSource();

            mouseHook = new MouseHook();
            mouseHook.MouseHorizontalWheel += EventPanel_MouseHorizontalWheel;
            mouseHook.StartMouseHook();

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
            decelerationCancellationTokenSource.Cancel();

            originCursorPosition = previousCursorPosition = e.GetPosition(this);
            originTime = previousTime = DateTime.Now;

            Cursor = Cursors.ScrollAll;

            CaptureMouse();
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (!IsMouseCaptured)
            {
                return;
            }

            var displacement = originCursorPosition - e.GetPosition(this);
            var duration = DateTime.Now - originTime;

            // Prevents infinite velocity (dividing by zero).
            if (duration.Ticks == 0)
            {
                return;
            }

            var averageHorizontalVelocity = displacement.X / duration.TotalMilliseconds;
            var averageVerticalVelocity = displacement.Y / duration.TotalMilliseconds;

            decelerationCancellationTokenSource.Cancel();
            decelerationCancellationTokenSource = new CancellationTokenSource();

            Decelerate(averageHorizontalVelocity, averageVerticalVelocity, decelerationCancellationTokenSource.Token);

            Cursor = Cursors.Arrow;

            ReleaseMouseCapture();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!IsMouseCaptured)
            {
                return;
            }

            var cursorPosition = e.GetPosition(this);

            var displacement = previousCursorPosition - cursorPosition;

            RequestScroll(displacement);

            previousCursorPosition = cursorPosition;
            previousTime = DateTime.Now;
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            previousCursorPosition = e.GetPosition(this);

            RequestScroll(new Vector(0, -e.Delta));

            base.OnMouseWheel(e);
        }

        private void Decelerate(double horizontalVelocity, double verticalVelocity, CancellationToken cancellationToken)
        {
            var horizontalDeceleration = -horizontalVelocity / decelerationDuration.TotalMilliseconds;
            var verticalDeceleration = -verticalVelocity / decelerationDuration.TotalMilliseconds;

            var startTime = DateTime.Now;

            var decelerationTimer = new System.Timers.Timer(decelerationInterval.TotalMilliseconds);
            decelerationTimer.Elapsed += (o, e) =>
            {
                var time = DateTime.Now;

                if (cancellationToken.IsCancellationRequested || (time - startTime) >= decelerationDuration)
                {
                    Dispatcher.BeginInvoke(new Action(() => decelerationTimer.Enabled = false));
                }

                var timeDifference = time - previousTime;

                // This is a guard against lag spikes, which will inflate the velocity.
                if (timeDifference.Milliseconds > 20)
                {
                    previousTime = time;

                    return;
                }

                horizontalVelocity += horizontalDeceleration * timeDifference.TotalMilliseconds;
                verticalVelocity += verticalDeceleration * timeDifference.TotalMilliseconds;

                var displacement = new Vector(timeDifference.TotalMilliseconds * horizontalVelocity, timeDifference.TotalMilliseconds * verticalVelocity);

                if (!double.IsNaN(displacement.X) && !double.IsNaN(displacement.Y))
                {
                    Dispatcher.BeginInvoke(new Action(() => RequestScroll(displacement)));

                    previousTime = time;
                }
            };

            decelerationTimer.Enabled = true;
        }

        private void EventPanel_MouseHorizontalWheel(object sender, MouseHorizontalWheelEventArgs e)
        {
            RequestScroll(new Vector(e.Delta, 0));
        }
    }
}