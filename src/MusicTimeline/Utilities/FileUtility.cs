using NathanHarrenstein.MusicTimeline.Logging;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Windows;

namespace NathanHarrenstein.MusicTimeline.Utilities
{
    internal static class FileUtility
    {
        internal static byte[] GetImage(string filePath)
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
                using (var webClient = new WebClient() { Proxy = null })
                {
                    webClient.Proxy = null;
                    webClient.Headers.Add("Content-Type", "image");

                    try
                    {
                        return webClient.DownloadData(filePath);
                    }
                    catch (WebException e)
                    {
                        MessageBox.Show(e.Message);
                    }
                    catch
                    {
                    }
                }
            }

            return null;
        }

        internal static bool WebsiteExists(string url)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((a, b, c, d) => true);
            WebRequest webRequest = WebRequest.Create(url);
            WebResponse webResponse;

            try
            {
                webResponse = webRequest.GetResponse();
            }
            catch (WebException e)
            {
                Logger.Log(e.ToString(), "MusicTimeline.log");

                return false;
            }

            return true;
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