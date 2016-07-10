using System;
using System.IO;

namespace NathanHarrenstein.MusicTimeline.Logging
{
    public class Logger
    {
        private readonly string directoryPath;

        public Logger()
        {
            directoryPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create)}\Logs";
        }

        internal void Log(Exception exception)
        {
            using (var streamWriter = File.AppendText($@"{directoryPath}\{DateTimeOffset.Now}.log"))
            {
                streamWriter.Write(exception);
            }
        }
    }
}