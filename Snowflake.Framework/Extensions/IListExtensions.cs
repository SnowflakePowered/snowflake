using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Snowflake.Extensions
{
    public static class IListExtensions
    {
        public static ReadOnlyCollection<TValue> AsReadOnly<TValue>(
    this IList<TValue> collection)
        {
            return new ReadOnlyCollection<TValue>(collection);
        }
    }
}
