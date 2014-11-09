using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
