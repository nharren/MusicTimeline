using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace NathanHarrenstein.MusicTimeline.Parsers
{
    internal class SpotifyParser
    {
        private const string TrackRegexPattern = @"spotify(?:\.com\/|:)track(?::|\/)([\w\d]{22})";

        public bool IsValidTrackUrl(string url)
        {
            var regex = new Regex(TrackRegexPattern);

            return regex.IsMatch(url);
        }

        public string ParseTrackId(string url)
        {
            var regex = new Regex(TrackRegexPattern);
            var match = regex.Match(url);
            var capture = match.Captures
                .OfType<Capture>()
                .FirstOrDefault();

            if (capture == null || capture.Value == null)
            {
                throw new FormatException("The Spotify url was invalid.");
            }

            return capture.Value;
        }
    }
}