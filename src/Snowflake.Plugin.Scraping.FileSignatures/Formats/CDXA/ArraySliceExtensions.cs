using System;
using System.Collections.Generic;
using System.Linq;

namespace Snowflake.Plugin.Scraping.FileSignatures.Formats.CDXA
{
    internal static class ArraySliceExtensions
    {
        public static int[] FindAllIndexof<T>(this IEnumerable<T> values, T val)
        {
            return values.Select((b, i) => object.Equals(b, val) ? i : -1).Where(i => i != -1).ToArray();
        }

        public static T[] Slice<T>(this T[] arr, int indexFrom, int indexTo)
        {
            if (indexFrom > indexTo)
            {
                throw new ArgumentOutOfRangeException("indexFrom is bigger than indexTo!");
            }

            int length = indexTo - indexFrom;
            T[] result = new T[length];
            Array.Copy(arr, indexFrom, result, 0, length);
            return result;
        }
    }
}
