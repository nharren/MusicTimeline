using System;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace NathanHarrenstein.MusicTimeline.Utilities
{
    public static class WebUtility
    {
        public static string GetHtml(string url)
        {
            using (var webClient = new WebClient())
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((a, b, c, d) => true);
                webClient.Encoding = Encoding.UTF8;
                webClient.Proxy = null;

                try
                {
                    return webClient.DownloadString(url);
                }
                catch (Exception ex)
                {
                    App.Logger.Log(ex);

                    return string.Empty;
                }
            }
        }

        public static string GetTitle(string url)
        {
            var match = Regex.Match(GetHtml(url), @"<title>(.*?)</title>", RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}