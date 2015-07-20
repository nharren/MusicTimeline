using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NathanHarrenstein.MusicTimeline.Utilities
{
    public static class WikiUtility
    {
        public static string GetHTML(string wikiUrl)
        {
            var urlParts = wikiUrl.Split(new string[] { "/wiki/" }, StringSplitOptions.None);

            if (urlParts.Length > 1)
            {
                var wikiId = urlParts[1];
                var requestUrl = $"http://en.wikipedia.org/w/api.php?action=query&prop=extracts&format=xml&exsectionformat=plain&titles={wikiId}&redirects=";
                string xml = null;

                using (WebClient client = new WebClient())
                {
                    client.Proxy = null;
                    client.Encoding = Encoding.UTF8;

                    try
                    {
                        xml = client.DownloadString(requestUrl);
                    }
                    catch (Exception e)
                    {
                        Logger.Log(e.ToString(), "MusicTimeline.log");
                    }               
                }

                if (xml != null)
                {
                    return ConvertExtendedASCII(XDocument.Parse(xml).Descendants("extract").First().Value); 
                }
            }

            return null;
        }

        public static string ConvertExtendedASCII(string HTML)
        {
            string retVal = "";
            char[] s = HTML.ToCharArray();

            foreach (char c in s)
            {
                if (Convert.ToInt32(c) > 127)
                    retVal += "&#" + Convert.ToInt32(c) + ";";
                else
                    retVal += c;
            }

            return retVal;
        }
    }
}
