using System;
using System.Collections.Generic;

namespace NathanHarrenstein.MusicTimeline.Extensions
{
    internal static class EnumerableExtensions
    {
        internal static IEnumerable<TResult> Common<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            var collection = new List<TResult>();

            foreach (var element in source)
            {
                foreach (var subElement in selector(element))
                {
                    if (!collection.Contains(subElement))
                    {
                        collection.Add(subElement);

                        yield return subElement;
                    }
                }
            }
        }
    }
}