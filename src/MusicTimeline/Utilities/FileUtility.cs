using NathanHarrenstein.MusicTimeline.Logging;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Threading.Tasks;
using System.Windows;

namespace NathanHarrenstein.MusicTimeline.Utilities
{
    internal static class FileUtility
    {
        internal static bool TryGetImage(string path, out byte[] imageBytes)
        {
            Uri fileUri = null;

            if (!Uri.TryCreate(path, UriKind.Absolute, out fileUri))
            {
                imageBytes = null;

                return false;
            }

            if (fileUri.IsFile)
            {
                if (File.Exists(path))
                {
                    imageBytes = File.ReadAllBytes(path);

                    return true;
                }
            }
            else
            {
                using (var webClient = new WebClient())
                {
                    webClient.Proxy = null;
                    webClient.Headers.Add("Content-Type", "image");

                    try
                    {
                        imageBytes = webClient.DownloadData(path);

                        return true;
                    }
                    catch
                    {
                    }
                }
            }

            imageBytes = null;

            return false;
        }


        internal async static Task<byte[]> GetImageAsync(string filePath)
        {
            Uri fileUri = null;

            if (!Uri.TryCreate(filePath, UriKind.Absolute, out fileUri))
            {
                return null;
            }

            if (fileUri.IsFile)
            {
                if (File.Exists(filePath))
                {
                    return File.ReadAllBytes(filePath);
                }
            }
            else
            {
                using (var webClient = new WebClient())
                {
                    webClient.Proxy = null;
                    webClient.Headers.Add("Content-Type", "image");

                    try
                    {
                        return await webClient.DownloadDataTaskAsync(filePath);
                    }
                    catch
                    {
                    }
                }
            }

            return null;
        }

        internal static bool HasLine(Stream file, string line)
        {
            var reader = new StreamReader(file);

            while (!reader.EndOfStream)
            {
                if (reader.ReadLine() == line)
                {
                    return true;
                }
            }

            return false;
        }

        internal static void WriteLine(Stream file, string line)
        {
            new StreamWriter(file).WriteLine(line);
        }
    }
}