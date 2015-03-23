using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace NathanHarrenstein.Timeline
{
    public delegate void MouseHorizontalWheelEventHandler(object sender, MouseHorizontalWheelEventArgs e);

    public class MouseHorizontalWheelEventArgs : EventArgs
    {
        public int X { get; private set; }

        public int Y { get; private set; }

        public int Delta { get; private set; }

        public MouseHorizontalWheelEventArgs(int X, int Y, int Delta)
        {
            this.X = X;
            this.Y = Y;
            this.Delta = Delta;
        }
    }

    public partial class MouseHook : IDisposable
    {
        private bool disposed = false;
        private IntPtr hHook;

        public event MouseHorizontalWheelEventHandler MouseHorizontalWheel;

        ~MouseHook()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
                StopMouseHook(false);
            }
        }

        protected virtual void OnMouseHorizontalWheel(MouseHorizontalWheelEventArgs e)
        {
            if (MouseHorizontalWheel != null)
                MouseHorizontalWheel(this, e);
        }

        private IntPtr mouseProc(int code, WindowsMessages wParam, ref MSLLHOOKSTRUCT lParam)
        {
            // MSDN says only process msg if code >= 0, and filter for movement events
            if (code >= 0 && wParam == WindowsMessages.MOUSEHWHEEL)
            {
                // Raise our mousemovement event
                OnMouseHorizontalWheel(new MouseHorizontalWheelEventArgs(lParam.pt.x, lParam.pt.y, GET_WHEEL_DELTA_WPARAM(lParam.mouseData)));
            }

            return CallNextHookEx(IntPtr.Zero, code, wParam, ref lParam);
        }

        private LowLevelMouseProc mouseProcDelegate;

        public void StartMouseHook()
        {
            mouseProcDelegate = new LowLevelMouseProc(mouseProc);
            // Insert global low-level mouse hook, tell Windows to call our mouseProc method
            hHook = SetWindowsHookEx(HookType.WH_MOUSE_LL, mouseProcDelegate, IntPtr.Zero, 0);
            if (hHook == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        public void StopMouseHook()
        {
            StopMouseHook(true);
        }

        private void StopMouseHook(bool ThrowExceptionOnError)
        {
            if (hHook != IntPtr.Zero)
            {
                bool success = UnhookWindowsHookEx(hHook);
                if (!success && ThrowExceptionOnError)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }
    }
}