using System.IO;

namespace NathanHarrenstein.ComposerTimeline
{
    public static class StreamReaderExtensions
    {
        public static bool HasLine(this StreamReader reader, string line)
        {
            while (!reader.EndOfStream)
            {
                if (reader.ReadLine() == line)
                {
                    return true;
                }
            }

            return false;
        }
    }
}