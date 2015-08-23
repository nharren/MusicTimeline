using System;
using System.IO;

namespace NathanHarrenstein.MusicTimeline.Logging
{
    public static class Logger
    {
        public static void Log(string message, string filename)
        {
            using (StreamWriter streamWriter = File.AppendText($@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create)}\{filename}"))
            {
                streamWriter.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}\r\n\r\n{message}\r\n-------------------------------");
            }
        }

        public static void Reset(string filename)
        {
            using (var log = File.Create($@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create)}\{filename}"))
            {
            }
        }
    }
}