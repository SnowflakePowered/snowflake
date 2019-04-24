using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Framework.Scheduling;

namespace Snowflake.Framework.Remoting
{
    public sealed class AsyncJobQueue<T> : IAsyncJobQueue<T>
    {
        public AsyncJobQueue()
        {
            this.Enumerators = new ConcurrentDictionary<Guid, IAsyncEnumerator<T>>();
        }

        public IDictionary<Guid, IAsyncEnumerator<T>> Enumerators { get; }

        public IAsyncEnumerator<T> GetEnumerator(Guid jobId)
        {
            var exists = this.Enumerators.TryGetValue(jobId, out IAsyncEnumerator<T> value);
            if (!exists) return Empty().GetAsyncEnumerator();
            return value;
        }

        private static async IAsyncEnumerable<T> Empty()
        {
            yield break;
        }

        public async ValueTask<(T, bool)> GetNext(Guid jobId)
        {
            var enumerator = this.GetEnumerator(jobId);
            var result =  (value: enumerator.Current, hasNext: await enumerator.MoveNextAsync());
            if (!result.hasNext)
            {
                await enumerator.DisposeAsync();
                this.Enumerators.Remove(jobId);
            }
            return result;
        }

        public async Task<Guid> QueueJob(IAsyncEnumerable<T> asyncEnumerable)
        {
            var guid = Guid.NewGuid();
            var enumerator = asyncEnumerable.GetAsyncEnumerator();
            await enumerator.MoveNextAsync();
            this.Enumerators.Add(guid, enumerator);
            return guid;
        }

        public async Task<Guid> QueueJob(IAsyncEnumerable<T> asyncEnumerable, Guid guid)
        {
            var enumerator = asyncEnumerable.GetAsyncEnumerator();
            await enumerator.MoveNextAsync();
            this.Enumerators.Add(guid, enumerator);
            return guid;
        }

    }
}
