using System;
using System.Collections.Generic;
using System.Linq;

namespace Snowflake.Support.Scraping.RecordTraversers.Extensions
{
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// Returns values distinct by a predicate.
        /// </summary>
        /// <returns>The distinct items.</returns>
        /// <param name="source">The enumerable to return.</param>
        /// <param name="keySelector">The predicate to select on.</param>
        /// <typeparam name="TSource">The source type</typeparam>
        /// <typeparam name="TKey">The predicate to return/typeparam>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
