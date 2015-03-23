using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace NathanHarrenstein.Controls
{
    public class PanGrid : Grid
    {
        private enum Speed { Slow, Fast };

        private Point origin;
        private MouseHook mouseHook;
        private Timer lTimer;
        private Timer rTimer;
        private Timer tTimer;
        private Timer bTimer;
        private Timer tlTimer;
        private Timer blTimer;
        private Timer trTimer;
        private Timer brTimer;
        private Speed currentSpeed;
        private Speed newSpeed;
        private Timer currentTimer;
        private const int SlowPanInterval = 15;
        private const int SideZoneSize = 75;
        private const int CornerZoneSize = 150;

        public PanGrid()
        {
            // Necessary for keyboard events.
            Focusable = true;

            mouseHook = new MouseHook();
            mouseHook.StartMouseHook();
            mouseHook.MouseHorizontalWheel += mouseHook_MouseHorizontalWheel;
        }

        private Point Origin
        {
            get
            {
                return origin;
            }
            set
            {
                origin = value;
            }
        }

        public void Pan(Vector delta)
        {
            foreach (var child in Children)
            {
                var panningChild = child as IPan;

                if (panningChild != null)
                {
                    var childCanPan = panningChild.CanPan(delta);

                    if (childCanPan != delta)
                    {
                        delta = childCanPan;
                    }
                }
            }

            foreach (var child in Children)
            {
                var panningChild = child as IPan;

                if (panningChild != null)
                {
                    panningChild.Pan(delta);
                }
            }
        }

        private void HoverPan(ref Timer timer, Point origin, Vector vector)
        {
            if (timer == null)
            {
                if (currentTimer != null)
                {
                    currentSpeed = Speed.Slow;
                }

                Action<object> action = o =>
                {
                    Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(() =>
                    {
                        Origin = origin;

                        if (newSpeed != currentSpeed)
                        {
                            if (newSpeed == Speed.Fast)
                            {
                                vector = new Vector(vector.X * 2, vector.Y * 2);
                                Console.WriteLine(vector.ToString());

                                currentSpeed = Speed.Fast;
                            }
                            else
                            {
                                vector = new Vector(vector.X * 0.5, vector.Y * 0.5);
                                Console.WriteLine(vector.ToString());

                                currentSpeed = Speed.Slow;
                            }
                        }

                        Pan(vector);
                    }));
                };

                timer = new Timer(new TimerCallback(action), null, 0, 25);

                currentTimer = timer;
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            Origin = e.GetPosition(this);

            Pan(new Vector(0, -e.Delta));

            base.OnMouseWheel(e);
        }

        private void mouseHook_MouseHorizontalWheel(object sender, MouseHorizontalWheelEventArgs e)
        {
            Origin = new Point(e.X, e.Y);

            Pan(new Vector(e.Delta, 0));
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            newSpeed = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift ? Speed.Fast : Speed.Slow;

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            newSpeed = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift ? Speed.Fast : Speed.Slow;

            base.OnKeyUp(e);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (IsMouseOver)
            {
                origin = e.GetPosition(this);

                Cursor = Cursors.ScrollAll;

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

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (lTimer != null)
            {
                lTimer.Dispose();
                lTimer = null;
            }
            else if (rTimer != null)
            {
                rTimer.Dispose();
                rTimer = null;
            }
            else if (bTimer != null)
            {
                bTimer.Dispose();
                bTimer = null;
            }
            else if (tTimer != null)
            {
                tTimer.Dispose();
                tTimer = null;
            }
            else if (tlTimer != null)
            {
                tlTimer.Dispose();
                tlTimer = null;
            }
            else if (blTimer != null)
            {
                blTimer.Dispose();
                blTimer = null;
            }
            else if (trTimer != null)
            {
                trTimer.Dispose();
                trTimer = null;
            }
            else if (brTimer != null)
            {
                brTimer.Dispose();
                brTimer = null;
            }

            Cursor = Cursors.Arrow;

            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Necessary for keyboard events.
            if (!HasEffectiveKeyboardFocus)
            {
                Focus();
            }

            var position = e.GetPosition(this);

            // Click-And-Drag Pan
            if (IsMouseCaptured)
            {
                var vector = Origin - position;

                Pan(vector);

                origin = position;

                return;
            }

            // Determine Hover Zone
            var inTopLeftZone = position.Y > 50 && position.Y <= 50 + CornerZoneSize && position.X <= CornerZoneSize;
            var inTopRightZone = position.Y > 50 && position.Y <= 50 + CornerZoneSize && position.X >= ActualWidth - CornerZoneSize;
            var inBottomLeftZone = position.Y >= ActualHeight - CornerZoneSize && position.X <= CornerZoneSize;
            var inBottomRightZone = position.Y >= ActualHeight - CornerZoneSize && position.X >= ActualWidth - CornerZoneSize;
            var inLeftZone = position.X <= SideZoneSize && !inTopLeftZone && !inBottomLeftZone;
            var inRightZone = position.X >= ActualWidth - SideZoneSize && !inTopRightZone && !inBottomRightZone;
            var inBottomZone = position.Y >= ActualHeight - SideZoneSize && !inBottomRightZone && !inBottomLeftZone;
            var inTopZone = position.Y > 50 && position.Y <= 50 + SideZoneSize && !inTopLeftZone && !inTopRightZone;

            // Left Hover Pan
            if (inLeftZone && !inBottomLeftZone && !inTopLeftZone)
            {
                HoverPan(ref lTimer, position, new Vector(-SlowPanInterval, 0));
                Cursor = Cursors.ScrollW;
            }
            else if (lTimer != null)
            {
                lTimer.Dispose();
                lTimer = null;
                Cursor = Cursors.Arrow;
            }

            // Right Hover Pan
            if (inRightZone && !inBottomRightZone && !inTopRightZone)
            {
                HoverPan(ref rTimer, position, new Vector(SlowPanInterval, 0));
                Cursor = Cursors.ScrollE;
            }
            else if (rTimer != null)
            {
                rTimer.Dispose();
                rTimer = null;
                Cursor = Cursors.Arrow;
            }

            // Bottom Hover Pan
            if (inBottomZone && !inBottomLeftZone && !inBottomRightZone)
            {
                HoverPan(ref bTimer, position, new Vector(0, SlowPanInterval));
                Cursor = Cursors.ScrollS;
            }
            else if (bTimer != null)
            {
                bTimer.Dispose();
                bTimer = null;
                Cursor = Cursors.Arrow;
            }

            // Top Hover Pan
            if (inTopZone && !inTopLeftZone && !inTopRightZone)
            {
                HoverPan(ref tTimer, position, new Vector(0, -SlowPanInterval));
                Cursor = Cursors.ScrollN;
            }
            else if (tTimer != null)
            {
                tTimer.Dispose();
                tTimer = null;
                Cursor = Cursors.Arrow;
            }

            // Top-Left Hover Pan
            if (inTopLeftZone)
            {
                HoverPan(ref tlTimer, position, new Vector(-SlowPanInterval + 5, -SlowPanInterval + 5));
                Cursor = Cursors.ScrollNW;
            }
            else if (tlTimer != null)
            {
                tlTimer.Dispose();
                tlTimer = null;
                Cursor = Cursors.Arrow;
            }

            // Bottom-Left Hover Pan
            if (inBottomLeftZone)
            {
                HoverPan(ref blTimer, position, new Vector(-SlowPanInterval + 5, SlowPanInterval - 5));
                Cursor = Cursors.ScrollSW;
            }
            else if (blTimer != null)
            {
                blTimer.Dispose();
                blTimer = null;
                Cursor = Cursors.Arrow;
            }

            // Top-Right Hover Pan
            if (inTopRightZone)
            {
                HoverPan(ref trTimer, position, new Vector(SlowPanInterval - 5, -SlowPanInterval + 5));
                Cursor = Cursors.ScrollNE;
            }
            else if (trTimer != null)
            {
                trTimer.Dispose();
                trTimer = null;
                Cursor = Cursors.Arrow;
            }

            // Bottom-Right Hover Pan
            if (inBottomRightZone)
            {
                HoverPan(ref brTimer, position, new Vector(SlowPanInterval - 5, SlowPanInterval - 5));
                Cursor = Cursors.ScrollSE;
            }
            else if (brTimer != null)
            {
                brTimer.Dispose();
                brTimer = null;
                Cursor = Cursors.Arrow;
            }

            base.OnMouseMove(e);
        }
    }
}