using System;
using System.IO;
using System.Net;

namespace NathanHarrenstein.ComposerTimeline
{
    public static class FileManager
    {
        public static byte[] GetFile(string filePath)
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
                if (WebFileExists(filePath))
                {
                    using (var webClient = new WebClient() { Proxy = null })
                    {
                        return webClient.DownloadData(filePath);
                    }
                }
            }

            return null;
        }

        public static bool WebFileExists(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";

            HttpWebResponse response = null;

            var result = true;

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                result = response.StatusCode == HttpStatusCode.OK;
            }
            catch (WebException)
            {
                result = false;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return result;
        }
    }
}