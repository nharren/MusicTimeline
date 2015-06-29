using System.IO;
using System.Net;

namespace NathanHarrenstein.ComposerTimeline
{
    public static class FileManager
    {
        public static byte[] GetFile(string path, FileLocation fileLocation)
        {
            if (fileLocation == FileLocation.Local || fileLocation == FileLocation.LocalOrWeb)
            {
                if (File.Exists(path))
                {
                    return File.ReadAllBytes(path);
                }
            }

            if (fileLocation == FileLocation.Web || fileLocation == FileLocation.LocalOrWeb)
            {
                if (FileExists(path))
                {
                    using (var webClient = new WebClient() { Proxy = null })
                    {
                        return webClient.DownloadData(path);
                    }
                }
            }

            return null;
        }

        public static bool FileExists(string url)
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