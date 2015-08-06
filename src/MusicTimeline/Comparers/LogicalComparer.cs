using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NathanHarrenstein.MusicTimeline.Comparers
{
    public class LogicalComparer : IComparer<string>
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int StrCmpLogicalW(string x, string y);

        public int Compare(string x, string y)
        {
            return StrCmpLogicalW(x, y);
        }
    }
}