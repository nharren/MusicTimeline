using System.Runtime.InteropServices;

namespace NathanHarrenstein.MusicTimeline.Utilities
{
    public static class AccessibilityUtility
    {
        private const uint FKF_CONFIRMHOTKEY = 0x00000008;
        private const uint FKF_FILTERKEYSON = 0x00000001;
        private const uint FKF_HOTKEYACTIVE = 0x00000004;
        private const uint SKF_CONFIRMHOTKEY = 0x00000008;
        private const uint SKF_HOTKEYACTIVE = 0x00000004;
        private const uint SKF_STICKYKEYSON = 0x00000001;
        private const uint SPI_GETFILTERKEYS = 0x0032;
        private const uint SPI_GETSTICKYKEYS = 0x003A;
        private const uint SPI_GETTOGGLEKEYS = 0x0034;
        private const uint SPI_SETFILTERKEYS = 0x0033;
        private const uint SPI_SETSTICKYKEYS = 0x003B;
        private const uint SPI_SETTOGGLEKEYS = 0x0035;
        private const uint TKF_CONFIRMHOTKEY = 0x00000008;
        private const uint TKF_HOTKEYACTIVE = 0x00000004;
        private const uint TKF_TOGGLEKEYSON = 0x00000001;
        private static uint FKEYSize = sizeof(uint) * 6;
        private static uint SKEYSize = sizeof(uint) * 2;
        private static bool StartupAccessibilitySet = false;
        private static FILTERKEY StartupFilterKeys;
        private static SKEY StartupStickyKeys;
        private static SKEY StartupToggleKeys;

        public static void AllowAccessibilityShortcutKeys(bool allowKeys)
        {
            if (!StartupAccessibilitySet)
            {
                StartupStickyKeys.cbSize = SKEYSize;
                StartupToggleKeys.cbSize = SKEYSize;
                StartupFilterKeys.cbSize = FKEYSize;
                SystemParametersInfo(SPI_GETSTICKYKEYS, SKEYSize, ref StartupStickyKeys, 0);
                SystemParametersInfo(SPI_GETTOGGLEKEYS, SKEYSize, ref StartupToggleKeys, 0);
                SystemParametersInfo(SPI_GETFILTERKEYS, FKEYSize, ref StartupFilterKeys, 0);
                StartupAccessibilitySet = true;
            }

            if (allowKeys)
            {
                SystemParametersInfo(SPI_SETSTICKYKEYS, SKEYSize, ref StartupStickyKeys, 0);
                SystemParametersInfo(SPI_SETTOGGLEKEYS, SKEYSize, ref StartupToggleKeys, 0);
                SystemParametersInfo(SPI_SETFILTERKEYS, FKEYSize, ref StartupFilterKeys, 0);
            }
            else
            {
                SKEY skOff = StartupStickyKeys;

                if ((skOff.dwFlags & SKF_STICKYKEYSON) == 0)
                {
                    skOff.dwFlags &= ~SKF_HOTKEYACTIVE;
                    skOff.dwFlags &= ~SKF_CONFIRMHOTKEY;
                    SystemParametersInfo(SPI_SETSTICKYKEYS, SKEYSize, ref skOff, 0);
                }

                SKEY tkOff = StartupToggleKeys;

                if ((tkOff.dwFlags & TKF_TOGGLEKEYSON) == 0)
                {
                    tkOff.dwFlags &= ~TKF_HOTKEYACTIVE;
                    tkOff.dwFlags &= ~TKF_CONFIRMHOTKEY;
                    SystemParametersInfo(SPI_SETTOGGLEKEYS, SKEYSize, ref tkOff, 0);
                }

                FILTERKEY fkOff = StartupFilterKeys;

                if ((fkOff.dwFlags & FKF_FILTERKEYSON) == 0)
                {
                    fkOff.dwFlags &= ~FKF_HOTKEYACTIVE;
                    fkOff.dwFlags &= ~FKF_CONFIRMHOTKEY;
                    SystemParametersInfo(SPI_SETFILTERKEYS, FKEYSize, ref fkOff, 0);
                }
            }
        }

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo", SetLastError = false)]
        private static extern bool SystemParametersInfo(uint action, uint param, ref SKEY vparam, uint init);

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo", SetLastError = false)]
        private static extern bool SystemParametersInfo(uint action, uint param, ref FILTERKEY vparam, uint init);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct FILTERKEY
        {
            public uint cbSize;
            public uint dwFlags;
            public uint iWaitMSec;
            public uint iDelayMSec;
            public uint iRepeatMSec;
            public uint iBounceMSec;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SKEY
        {
            public uint cbSize;
            public uint dwFlags;
        }
    }
}