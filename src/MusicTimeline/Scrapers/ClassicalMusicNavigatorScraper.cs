using HtmlAgilityPack;
using NathanHarrenstein.ClassicalMusicDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace NathanHarrenstein.MusicTimeline.Scrapers
{
   public static class ClassicalMusicNavigatorScraper
    {
        public static void ScrapeComposer(string url, Composer composer, ClassicalMusicDbContext dbContext)
        {
            if (url == null)
            {
                return;
            }

            var urlParts = url.Split('#');

            if (urlParts.Length != 2)
            {
                return;
            }

            string source = null;

            using (var webClient = new WebClient())
            {
                webClient.Proxy = null;

                try
                {
                    source = webClient.DownloadString(url);
                }
                catch (WebException e)
                {
                    MessageBox.Show(e.Message);

                    return;
                }
            }

            var htmlSegments = source.Split(new string[] { "<p>" }, StringSplitOptions.RemoveEmptyEntries);

            var composerHtmlSegment = htmlSegments.FirstOrDefault(s => s.Contains($"name=\"{urlParts[1]}\""));

            if (composerHtmlSegment == null)
            {
                return;
            }

            var hasInfluencedSplit = composerHtmlSegment.Split(new string[] { "instruc1.htm#e9" }, StringSplitOptions.RemoveEmptyEntries);

            if (hasInfluencedSplit.Length > 1)
            {
                var influencedComposerUrls = Regex.Matches(hasInfluencedSplit[1], "(?<=href=\").*?(?=\")").OfType<Match>().Select(m => "http://people.wku.edu/charles.smith/music/" + m.Value);

                foreach (var influencedComposerUrl in influencedComposerUrls)
                {
                    var influencedComposerLink = dbContext.Links.Where(l => l.Url == influencedComposerUrl).FirstOrDefault();

                    if (influencedComposerLink == null)
                    {
                        continue;
                    }

                    var influencedComposer = influencedComposerLink.Composers.FirstOrDefault();

                    if (!composer.Influenced.Contains(influencedComposer))
                    {
                        composer.Influenced.Add(influencedComposer);
                        influencedComposer.Influences.Add(composer);
                    }
                }
            }

            var influencesSplit = hasInfluencedSplit[0].Split(new string[] { "instruc1.htm#e8" }, StringSplitOptions.RemoveEmptyEntries);

            if (influencesSplit.Length > 1)
            {
                var influenceComposerUrls = Regex.Matches(influencesSplit[1], "(?<=href=\").*?(?=\")").OfType<Match>().Select(m => "http://people.wku.edu/charles.smith/music/" + m.Value);

                foreach (var influenceComposerUrl in influenceComposerUrls)
                {
                    var influenceComposerLink = dbContext.Links.Where(l => l.Url == influenceComposerUrl).FirstOrDefault();

                    if (influenceComposerLink == null)
                    {
                        continue;
                    }

                    var influenceComposer = influenceComposerLink.Composers.FirstOrDefault();

                    if (!composer.Influences.Contains(influenceComposer))
                    {
                        composer.Influences.Add(influenceComposer);
                        influenceComposer.Influenced.Add(composer);
                    }
                }
            }
        }
    }
}
