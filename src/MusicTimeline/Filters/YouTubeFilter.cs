using System.Collections.Generic;

namespace NathanHarrenstein.MusicTimeline.Filters
{
    public class YouTubeFilter : IFilter<string>
    {
        public IEnumerable<string> Apply(IEnumerable<string> inputItems, FilterMode filterMode)
        {
            foreach (var item in inputItems)
            {
                var lowerCaseUrl = item.ToLower();

                if (lowerCaseUrl.Contains(".youtube.com/") || lowerCaseUrl.Contains("://youtube.com/") || lowerCaseUrl.Contains("://youtu.be/"))
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