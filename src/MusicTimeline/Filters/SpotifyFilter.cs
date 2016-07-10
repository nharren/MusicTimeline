using NathanHarrenstein.MusicTimeline.ClassicalMusicDb;
using System.Collections.Generic;

namespace NathanHarrenstein.MusicTimeline.Filters
{
    public class SpotifyFilter : IFilter<Link>
    {
        public IEnumerable<Link> Apply(IEnumerable<Link> inputItems, FilterMode filterMode)
        {
            foreach (var item in inputItems)
            {
                var lowerCaseUrl = item.Url.ToLower();

                if (lowerCaseUrl.StartsWith("spotify:") || lowerCaseUrl.Contains(".spotify.com/"))
                {
                    if (filterMode == FilterMode.Keep)
                    {
                        yield return item;
                    }
                }
                else
                {
                    if (filterMode == FilterMode.Remove)
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}