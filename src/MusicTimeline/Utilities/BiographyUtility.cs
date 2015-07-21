using System.Text.RegularExpressions;

namespace NathanHarrenstein.MusicTimeline.Utilities
{
    public static class BiographyUtility
    {
        public static string RemoveEmptyParagraphs(string biography)
        {
            if (biography == null)
            {
                return biography;
            }

            var pattern = "<Paragraph />";
            var rgx = new Regex(pattern);
            return rgx.Replace(biography, "");
        }
    }
}