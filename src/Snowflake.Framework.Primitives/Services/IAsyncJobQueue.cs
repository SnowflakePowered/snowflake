using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Framework.Scheduling
{
    public interface IAsyncJobQueue<T>
    {
        IDictionary<Guid, IAsyncEnumerator<T>> Enumerators { get; }
        ValueTask<(T value, bool hasNext)> GetNext(Guid jobId);
        IAsyncEnumerator<T> GetEnumerator(Guid jobId);
        Guid QueueJob(IAsyncEnumerable<T> asyncEnumerable);
        Guid QueueJob(IAsyncEnumerable<T> asyncEnumerable, Guid guid);

    }
}
