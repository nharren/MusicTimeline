using NathanHarrenstein.MusicTimeline.Input;
using NathanHarrenstein.MusicTimeline.Models;
using NathanHarrenstein.MusicTimeline.Utilities;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace NathanHarrenstein.MusicTimeline.Providers
{
    public static class LinkProvider
    {
        public static Link GetLink(string url)
        {
            return new Link(url, GetTitle(url), GetIcon(url), new DelegateCommand(o => Process.Start(url)));
        }

        private static string GetIcon(string url)
        {
            var linkUri = new Uri(url);
            var localIconPath = Path.Combine(Environment.CurrentDirectory, "Resources/Favicons/" + linkUri.Host + ".ico");
            var localIconUri = new Uri(localIconPath);

            if (!Directory.Exists("Resources/Favicons"))
            {
                Directory.CreateDirectory("Resources/Favicons");
            }

            if (File.Exists(localIconPath))
            {
                return localIconPath;
            }
            else
            {
                var client = new WebClient();
                var webIconPath = "http://" + linkUri.Host + "/favicon.ico";

                var favicon = FileUtility.GetFile(webIconPath);

                if (favicon == null)
                {
                    return Path.Combine(Environment.CurrentDirectory, "Resources/Favicons/Default.ico");
                }

                File.WriteAllBytes(localIconPath, favicon);

                return localIconPath;
            }
        }

        private static string GetTitle(string link)
        {
            WebClient x = new WebClient();
            string source = x.DownloadString(link);

            return Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
        }
    }
}