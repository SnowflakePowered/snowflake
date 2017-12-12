using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Utility
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Does a list contain all values of another list?
        /// </summary>
        /// <remarks>Needs .NET 3.5 or greater.  Source:  https://stackoverflow.com/a/1520664/1037948 </remarks>
        /// <typeparam name="T">list value type</typeparam>
        /// <param name="containingList">the larger list we're checking in</param>
        /// <param name="lookupList">the list to look for in the containing list</param>
        /// <returns>true if it has everything</returns>
        public static bool ContainsAll<T>(this IEnumerable<T> containingList, IEnumerable<T> lookupList)
        {
            return !lookupList.Except(containingList).Any();
        }

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
