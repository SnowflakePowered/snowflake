using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Framework.Extensibility
{
    public sealed class AsyncJobQueue<T> : IAsyncJobQueue<T>
    {
        private sealed class AsyncJobQueueEnumerable<T> : IAsyncEnumerable<T>
        {
            public AsyncJobQueueEnumerable(AsyncJobQueue<T> queue, Guid job)
            {
                this.Enumerator = new AsyncJobQueueWrappingEnumerator<T>(queue, job);
            }

            public IAsyncEnumerator<T> Enumerator { get; }
     
            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return this.Enumerator;
            }
        }

        private sealed class AsyncJobQueueWrappingEnumerator<T> : IAsyncEnumerator<T>
        {
            public AsyncJobQueueWrappingEnumerator(AsyncJobQueue<T> queue, Guid job)
            {
                this.Queue = queue;
                this.JobId = job;
            }

            public T Current { get; set; }

            private AsyncJobQueue<T> Queue { get; }
            private Guid JobId { get; }

            // double dispose is no problem.
            public ValueTask DisposeAsync() => this.Queue.GetEnumerator(this.JobId).DisposeAsync();

            public async ValueTask<bool> MoveNextAsync()
            {
                (T val, bool hasNext) = await this.Queue.GetNext(JobId);
                this.Current = val;
                return hasNext;

            }
        }

        public AsyncJobQueue()
        {
            this.Enumerators = new ConcurrentDictionary<Guid, IAsyncEnumerator<T>>();
        }

        private IDictionary<Guid, IAsyncEnumerator<T>> Enumerators { get; }

        private IAsyncEnumerator<T> GetEnumerator(Guid jobId)
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
            var result = (value: enumerator.Current, hasNext: await enumerator.MoveNextAsync());
            if (!result.hasNext)
            {
                await enumerator.DisposeAsync();
                this.Enumerators.Remove(jobId);
            }
            return result;
        }

        public async Task<Guid> QueueJob(IAsyncEnumerable<T> asyncEnumerable, CancellationToken token = default)
        {
            var guid = Guid.NewGuid();
            var enumerator = asyncEnumerable.GetAsyncEnumerator(token);
            await enumerator.MoveNextAsync();
            this.Enumerators.Add(guid, enumerator);
            return guid;
        }

        public async Task<Guid> QueueJob(IAsyncEnumerable<T> asyncEnumerable, Guid guid, CancellationToken token = default)
        {
            if (this.Enumerators.TryGetValue(guid, out IAsyncEnumerator<T> old))
            {
                await old.DisposeAsync();
                this.Enumerators.Remove(guid);
            }

            var enumerator = asyncEnumerable.GetAsyncEnumerator(token);
            await enumerator.MoveNextAsync();
            this.Enumerators.Add(guid, enumerator);
            return guid;
        }

        public IAsyncEnumerable<T> GetEnumerable(Guid jobId)
        {
            return new AsyncJobQueueEnumerable<T>(this, jobId);
        }
    }
}
