using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake
{
    public sealed class EmptyLookup<T, K> : ILookup<T, K>
    {
        private static readonly EmptyLookup<T, K> _instance
            = new EmptyLookup<T, K>();

        public static EmptyLookup<T, K> Instance
        {
            get { return _instance; }
        }

        private EmptyLookup() { }

        public bool Contains(T key)
        {
            return false;
        }

        public int Count
        {
            get { return 0; }
        }

        public IEnumerable<K> this[T key]
        {
            get { return Enumerable.Empty<K>(); }
        }

        public IEnumerator<IGrouping<T, K>> GetEnumerator()
        {
            yield break;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            yield break;
        }
    }
    
}
