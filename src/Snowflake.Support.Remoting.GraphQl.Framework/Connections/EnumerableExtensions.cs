using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Relay.Utilities
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<T> Slice<T>(this IEnumerable<T> collection, int start, int end)
        {
            int index = 0;
            int count = 0;

            count = collection.Count();

            // Get start/end indexes, negative numbers start at the end of the collection.
            if (start < 0) start += count;
            if (end < 0) end += count;

            foreach (T item in collection)
            {
                if (index >= end) yield break;
                if (index >= start) yield return item;
                ++index;
            }
        }
    }
}