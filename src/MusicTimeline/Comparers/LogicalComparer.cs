using System.Collections.Generic;

namespace NathanHarrenstein.MusicTimeline.Comparers
{
    public class LogicalComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return NativeMethods.StrCmpLogicalW(x, y);
        }
    }
}