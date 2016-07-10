using System.Collections.Generic;

namespace NathanHarrenstein.MusicTimeline.Filters
{
    public interface IFilter<T>
    {
        IEnumerable<T> Apply(IEnumerable<T> inputItems, FilterMode filterMode);
    }
}