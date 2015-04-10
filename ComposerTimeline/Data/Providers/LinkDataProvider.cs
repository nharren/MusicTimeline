using NathanHarrenstein.ComposerTimeline.Controls;
using NathanHarrenstein.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NathanHarrenstein.ComposerTimeline
{
    public static class LinkDataProvider
    {
        public static LinkData GetLinkData(string link)
        {
            var linkData = new LinkData();
            var linkUri = new Uri(link);

            linkData.Icon = GetIcon(linkUri);
            linkData.Click = new DelegateCommand(o => Process.Start(link));
            linkData.Label = linkUri.Host;

            return linkData;
        }

        private static Uri GetIcon(Uri linkUri)
        {
            var localIconPath = Path.Combine(Environment.CurrentDirectory, @"Files/Icons/" + linkUri.Host + ".ico");
            var localIconUri = new Uri(localIconPath);

            if (!Directory.Exists(@"Files/Icons"))
            {
                Directory.CreateDirectory(@"Files/Icons");
            }

            if (File.Exists(localIconPath))
            {
                return localIconUri;
            }
            else
            {
                var client = new WebClient();
                var webIconPath = "http://" + linkUri.Host + "/favicon.ico";

                try
                {
                    client.DownloadFile(webIconPath, localIconPath);

                    return localIconUri;
                }
                catch (Exception)
                {
                    return new Uri("pack://application:,,,/Pages/ComposerPage/Resources/Default.ico");
                }
            }
        }
    }
}