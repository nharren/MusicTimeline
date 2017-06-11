using System.Collections.Generic;

namespace NathanHarrenstein.MusicTimeline.Filters
{
    public class SpotifyFilter : IFilter<string>
    {
        public IEnumerable<string> Apply(IEnumerable<string> inputItems, FilterMode filterMode)
        {
            foreach (var item in inputItems)
            {
                var lowerCaseUrl = item.ToLower();

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