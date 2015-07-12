using System;
using System.IO;
using System.Net;

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
                if (IsWebImage(filePath))
                {
                    using (var webClient = new WebClient() { Proxy = null })
                    {
                        return webClient.DownloadData(filePath);
                    }
                }
            }

            return null;
        }

        internal static bool WebsiteExists(string url)
        {
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

        internal static bool IsWebImage(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";

            HttpWebResponse response = null;

            var result = true;
            string contentType = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                result = response.StatusCode == HttpStatusCode.OK;

                contentType = response.ContentType;
            }
            catch (WebException e)
            {
                Logger.Log(e.ToString(), "MusicTimeline.log");

                result = false;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return result && contentType.StartsWith("image");
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