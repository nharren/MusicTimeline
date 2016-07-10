using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace NathanHarrenstein.MusicTimeline.Parsers
{
    public class YouTubeParser
    {
        private const string VideoRegexPattern = @"youtu(?:(?:\.be\/)|be\.com\/(?:(?:embed|v)\/)?(?:watch\?)?(?:feature=.*)?(?:(?:&|\?)?v=)?)([\w-]{11})";

        public bool IsValidVideoUrl(string url)
        {
            var regex = new Regex(VideoRegexPattern);

            return regex.IsMatch(url);
        }

        public string ParseVideoId(string url)
        {
            var regex = new Regex(VideoRegexPattern);
            var match = regex.Match(url);

            if (match.Groups.Count != 2)
            {
                throw new FormatException("The YouTube url was invalid.");
            }

            return match.Groups[1].Value;
        }
    }
}