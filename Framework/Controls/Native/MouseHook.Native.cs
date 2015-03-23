using System;
using System.Runtime.InteropServices;

namespace NathanHarrenstein.Controls
{
    partial class MouseHook
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(HookType hookType, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, WindowsMessages wParam, ref MSLLHOOKSTRUCT lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        internal static ushort HIWORD(IntPtr dwValue)
        {
            return (ushort)((((long)dwValue) >> 0x10) & 0xffff);
        }

        internal static ushort HIWORD(uint dwValue)
        {
            return (ushort)(dwValue >> 0x10);
        }

        internal static int GET_WHEEL_DELTA_WPARAM(IntPtr wParam)
        {
            return (short)HIWORD(wParam);
        }

        internal static int GET_WHEEL_DELTA_WPARAM(uint wParam)
        {
            return (short)HIWORD(wParam);
        }

        private delegate IntPtr LowLevelMouseProc(int code, WindowsMessages wParam, ref MSLLHOOKSTRUCT lParam);

        private enum HookType : int
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }
    }
}