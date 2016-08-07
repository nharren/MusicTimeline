using NathanHarrenstein.MusicTimeline.Utilities;
using System;
using System.Linq;
using System.Xml.Linq;

namespace NathanHarrenstein.MusicTimeline.Scrapers
{
    public static class WikipediaScraper
    {
        public static string ScrapeArticle(string url)
        {
            var urlParts = url.Split(new string[] { "/wiki/" }, StringSplitOptions.None);

            if (urlParts.Length > 1)
            {
                var wikiId = urlParts[1];
                var requestUrl = $"http://en.wikipedia.org/w/api.php?action=query&prop=extracts&format=xml&exsectionformat=plain&titles={wikiId}&redirects=";
                string xml = WebUtility.GetHtml(requestUrl);

                if (xml != null)
                {
                    return ConvertExtendedASCII(XDocument.Parse(xml).Descendants("extract").First().Value);
                }
            }

            return null;
        }

        public static string ScrapeIntro(string url)
        {
            var urlParts = url.Split(new string[] { "/wiki/" }, StringSplitOptions.None);

            if (urlParts.Length > 1)
            {
                var wikiId = urlParts[1];
                var requestUrl = $"http://en.wikipedia.org/w/api.php?action=query&prop=extracts&format=xml&explaintext=&exsectionformat=plain&titles={wikiId}&exintro=&redirects=";
                string xml = WebUtility.GetHtml(requestUrl);

                if (xml != null)
                {
                    return XDocument.Parse(xml).Descendants("extract").First().Value;
                }
            }

            return null;
        }

        private static string ConvertExtendedASCII(string HTML)
        {
            string retVal = "";
            char[] s = HTML.ToCharArray();

            foreach (char c in s)
            {
                if (Convert.ToInt32(c) > 127)
                {
                    retVal += "&#" + Convert.ToInt32(c) + ";";
                }
                else
                {
                    retVal += c;
                }
            }

            return retVal;
        }
    }
}