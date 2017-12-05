using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Builders;
using GraphQL.Types.Relay.DataObjects;

namespace GraphQL.Relay.Types
{
    internal static class ConnectionUtils
    {
        private const string Prefix = "arrayconnection";

        public static Connection<TSource> ToConnection<TSource, TParent>(
            IEnumerable<TSource> items,
            ResolveConnectionContext<TParent> context)
        {
            var list = items.ToList();
            return ToConnection(list, context, sliceStartIndex: 0, totalCount: list.Count);
        }

        public static Connection<TSource> ToConnection<TSource, TParent>(
            IEnumerable<TSource> slice,
            ResolveConnectionContext<TParent> context,
            int sliceStartIndex,
            int totalCount)
        {
            var sliceList = slice as IList<TSource> ?? slice.ToList();

            var metrics = new ArraySliceMetrics<TSource, TParent>(
                sliceList,
                context,
                sliceStartIndex,
                totalCount);

            var edges = metrics.Slice.Select((item, i) => new Edge<TSource>
            {
                Node = item,
                Cursor = OffsetToCursor(metrics.StartOffset + i),
            })
                .ToList();

            var firstEdge = edges.FirstOrDefault();
            var lastEdge = edges.LastOrDefault();

            return new Connection<TSource>
            {
                Edges = edges,
                TotalCount = totalCount,
                PageInfo = new PageInfo
                {
                    StartCursor = firstEdge?.Cursor,
                    EndCursor = lastEdge?.Cursor,
                    HasPreviousPage = metrics.HasPrevious,
                    HasNextPage = metrics.HasNext,
                },
            };
        }

        internal static object ToConnection(object values, ResolveConnectionContext<object> context)
        {
            throw new NotImplementedException();
        }

        public static string CursorForObjectInConnection<T>(
            IEnumerable<T> slice,
            T item)
        {
            var idx = slice.ToList().IndexOf(item);

            return idx == -1 ? null : OffsetToCursor(idx);
        }

        public static int CursorToOffset(string cursor)
        {
            byte[] data = Convert.FromBase64String(cursor);
            string cursorStr = Encoding.UTF8.GetString(data);
            return int.Parse(
                cursorStr.Substring(Prefix.Length + 1));
        }

        public static string OffsetToCursor(int offset)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Prefix}:{offset}"));
        }

        public static int OffsetOrDefault(string cursor, int defaultOffset)
        {
            if (cursor == null)
            {
                return defaultOffset;
            }

            try
            {
                return CursorToOffset(cursor);
            }
            catch
            {
                return defaultOffset;
            }
        }
    }
}