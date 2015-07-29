using System;
using System.Text.RegularExpressions;

namespace NathanHarrenstein.MusicTimeline.Utilities
{
    public static class BiographyUtility
    {
        public static string CleanXaml(string xaml)
        {
            if (xaml == null)
            {
                return xaml;
            }

            xaml = xaml.Trim();

            RemoveEmptyParagraphs(ref xaml);
            RemoveNamespace(ref xaml);
            RemoveSection("Media", ref xaml);
            RemoveSection("References", ref xaml);
            RemoveSection("Bibliography", ref xaml);
            RemoveSection("See also", ref xaml);
            RemoveSection("External links", ref xaml);
            RemoveSection("Free scores", ref xaml);
            RemoveSection("Music scores", ref xaml);
            RemoveSection("Notes", ref xaml);
            RemoveSection("Further reading", ref xaml);
            RemoveSection("Cited sources", ref xaml);
            RemoveSection("Other sources", ref xaml);

            return xaml;
        }

        private static void RemoveNamespace(ref string xaml)
        {
            xaml = xaml.Replace(@" xml:space=""preserve"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""", "");
        }

        private static string RemoveEmptyParagraphs(ref string xaml)
        {
            return Regex.Replace(xaml, "<Paragraph />", "");
        }

        private static void RemoveSection(string title, ref string xaml)
        {
            xaml = Regex.Replace(xaml, $@"(<Paragraph Tag=""h\d""><Run>{title}<\/Run>.+?)(?=<Paragraph Tag=""h|<\/Section>)", "");
        }
    }
}