using NathanHarrenstein.Input;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace NathanHarrenstein.ComposerTimeline.Providers
{
    public static class LinkProvider
    {
        public static Link GetLink(string url)
        {
            return new Link(url, GetTitle(url), GetIcon(url), new DelegateCommand(o => Process.Start(url)));
        }

        private static string GetTitle(string link)
        {
            WebClient x = new WebClient();
            string source = x.DownloadString(link);

            return Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
        }

        private static string GetIcon(string url)
        {
            var linkUri = new Uri(url);
            var localIconPath = Path.Combine(Environment.CurrentDirectory, @"Files/Icons/" + linkUri.Host + ".ico");
            var localIconUri = new Uri(localIconPath);

            if (!Directory.Exists(@"Files/Icons"))
            {
                Directory.CreateDirectory(@"Files/Icons");
            }

            if (File.Exists(localIconPath))
            {
                return localIconPath;
            }
            else
            {
                var client = new WebClient();
                var webIconPath = "http://" + linkUri.Host + "/favicon.ico";

                try
                {
                    client.DownloadFile(webIconPath, localIconPath);

                    return localIconPath;
                }
                catch
                {
                    return "pack://application:,,,/Pages/ComposerPage/Resources/Default.ico";
                }
            }
        }
    }
}