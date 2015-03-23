using System;
using System.IO;

namespace NathanHarrenstein.Services
{
    public class Logger
    {
        private static bool HasLog { get; set; }

        public void Log(string message, string filename)
        {
            if (!HasLog)
            {
                var log = File.Create(filename); // Ensures a new log.

                log.Dispose();
            }

            using (StreamWriter streamWriter = File.AppendText(filename))
            {
                streamWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                streamWriter.WriteLine();
                streamWriter.WriteLine(message);
                streamWriter.WriteLine("-------------------------------");
            }
        }
    }
}