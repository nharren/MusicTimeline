using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NathanHarrenstein
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TResult> Common<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            var collection = new Collection<TResult>();

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