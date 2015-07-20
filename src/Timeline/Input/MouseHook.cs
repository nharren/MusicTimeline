using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace NathanHarrenstein.Timeline.Input
{
    internal partial class MouseHook : IDisposable
    {
        public const int WH_MOUSE = 7;
        public const uint WM_MOUSEHWHEEL = 526;

        private bool disposed = false;
        private IntPtr hHook;
        private MouseProc mouseProcDelegate;

        ~MouseHook()
        {
            Dispose(false);
        }

        private delegate IntPtr MouseProc(int nCode, uint wParam, ref MOUSEHOOKSTRUCTEX lParam);

        public event MouseHorizontalWheelEventHandler MouseHorizontalWheel;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public void StartMouseHook()
        {
            mouseProcDelegate = new MouseProc(mouseProc);

            hHook = SetWindowsHookEx(WH_MOUSE, mouseProcDelegate, IntPtr.Zero, GetCurrentThreadId());

            if (hHook == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public void StopMouseHook()
        {
            StopMouseHook(true);
        }

        internal static int GET_WHEEL_DELTA_WPARAM(int wParam)
        {
            return HIWORD(wParam);
        }

        internal static int HIWORD(int dwValue)
        {
            return dwValue >> 16;
        }

        protected virtual void OnMouseHorizontalWheel(MouseHorizontalWheelEventArgs e)
        {
            if (MouseHorizontalWheel != null)
            {
                MouseHorizontalWheel(this, e);
            }
        }

        [DllImport("User32.dll", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, uint wParam, ref MOUSEHOOKSTRUCTEX lParam);

        [DllImport("Kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        [DllImport("User32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, MouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("User32.dll", SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;

                StopMouseHook(false);
            }
        }

        private IntPtr mouseProc(int code, uint wParam, ref MOUSEHOOKSTRUCTEX lParam)
        {
            if (code >= 0 && wParam == WM_MOUSEHWHEEL)
            {
                OnMouseHorizontalWheel(new MouseHorizontalWheelEventArgs(lParam.MOUSEHOOKSTRUCT.pt.x, lParam.MOUSEHOOKSTRUCT.pt.y, GET_WHEEL_DELTA_WPARAM(lParam.mouseData)));
            }

            return CallNextHookEx(IntPtr.Zero, code, wParam, ref lParam);
        }

        private void StopMouseHook(bool throwExceptionOnError)
        {
            if (hHook != IntPtr.Zero)
            {
                var successful = UnhookWindowsHookEx(hHook);

                if (!successful && throwExceptionOnError)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEHOOKSTRUCT
        {
            public POINT pt;
            public IntPtr hwnd;
            public uint wHitTestCode;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEHOOKSTRUCTEX
        {
            public MOUSEHOOKSTRUCT MOUSEHOOKSTRUCT;
            public int mouseData;         
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }
    }
}