using System;
using System.IO;

namespace NathanHarrenstein.MusicTimeline
{
    public static class Logger
    {
        public static void Reset(string filename)
        {
            File.Create(filename).Dispose();
        }

        public static void Log(string message, string filename)
        {
            using (StreamWriter streamWriter = File.AppendText(filename))
            {
                streamWriter.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}\r\n\r\n{message}\r\n-------------------------------");
            }
        }
    }
}