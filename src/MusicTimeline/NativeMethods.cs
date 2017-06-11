using System.Runtime.InteropServices;

namespace NathanHarrenstein.MusicTimeline
{
    internal static class NativeMethods
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int StrCmpLogicalW(string x, string y);
    }
}
