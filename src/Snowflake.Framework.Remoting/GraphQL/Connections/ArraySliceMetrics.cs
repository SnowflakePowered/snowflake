using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Builders;
using GraphQL.Relay.Utilities;

namespace GraphQL.Relay.Types
{
    internal ref struct ArraySliceMetrics<TSource, TParent>
    {
        private IList<TSource> _items;

        /// <summary>
        /// Gets or sets the Total number of items in outer list. May be >= the SliceSize
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// Gets or sets the local total of the list slice.
        /// </summary>
        public int SliceSize { get; }

        /// <summary>
        /// Gets or sets the start index of the slice within the larger List
        /// </summary>
        /// <returns></returns>
        public int StartIndex { get; } 

        /// <summary>
        /// Gets the end index of the slice within the larger List
        /// </summary>
        public int EndIndex => this.StartIndex + this.SliceSize;

        public int StartOffset { get; }
        public int EndOffset { get; }
        public bool HasPrevious { get; }
        public bool HasNext { get; }

        public ArraySliceMetrics(
            IList<TSource> slice,
            IResolveConnectionContext<TParent> context)
            : this(slice, context, 0, slice.Count)
        {
        }

        public IEnumerable<TSource> Slice => _items.Slice(
            Math.Max(StartOffset - StartIndex, 0),
            SliceSize - (EndIndex - EndOffset));

        public ArraySliceMetrics(
            IList<TSource> slice,
            IResolveConnectionContext<TParent> context,
            int sliceStartIndex,
            int totalCount)
        {
            _items = slice;

            this.SliceSize = slice.Count;
            this.StartIndex = sliceStartIndex;

            var endIndex = this.StartIndex + this.SliceSize;

            var beforeOffset = ConnectionUtils.OffsetOrDefault(context.Before, totalCount);
            var afterOffset = ConnectionUtils.OffsetOrDefault(context.After, defaultOffset: -1);
           
            this.StartOffset =  Math.Max(sliceStartIndex - 1, Math.Max(afterOffset, -1)) + 1;
            this.EndOffset = Math.Max(endIndex - 1, Math.Max(beforeOffset, totalCount));

            if (context.First.HasValue)
            {
                this.EndOffset = Math.Min(this.EndOffset, this.StartOffset + context.First.Value);
            }

            if (context.Last.HasValue)
            {
                this.StartOffset = Math.Min(this.StartOffset, this.EndOffset - context.Last.Value);
            }

            var lowerBound = context.After != null ? afterOffset + 1 : 0;
            var upperBound = context.Before != null ? beforeOffset : totalCount;

            this.HasPrevious = context.Last.HasValue && this.StartOffset > lowerBound;
            this.HasNext = context.First.HasValue && this.EndOffset < upperBound;
            this.TotalCount = totalCount;
        }
    }
}
