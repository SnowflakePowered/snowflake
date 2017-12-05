using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake
{
    public sealed class EmptyLookup<T, TK> : ILookup<T, TK>
    {
        private static readonly EmptyLookup<T, TK> _instance
            = new EmptyLookup<T, TK>();

        public static EmptyLookup<T, TK> Instance
        {
            get { return _instance; }
        }

        private EmptyLookup() { }

        /// <inheritdoc/>
        public bool Contains(T key)
        {
            return false;
        }

        /// <inheritdoc/>
        public int Count
        {
            get { return 0; }
        }

        /// <inheritdoc/>
        public IEnumerable<TK> this[T key]
        {
            get { return Enumerable.Empty<TK>(); }
        }

        /// <inheritdoc/>
        public IEnumerator<IGrouping<T, TK>> GetEnumerator()
        {
            yield break;
        }

        /// <inheritdoc/>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            yield break;
        }
    }
}
