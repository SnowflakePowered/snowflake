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
        private sealed class AsyncJobQueueEnumerable : IAsyncEnumerable<T>
        {
            public AsyncJobQueueEnumerable(AsyncJobQueue<T> queue, Guid job)
            {
                this.Enumerator = new AsyncJobQueueWrappingEnumerator(queue, job);
            }

            public IAsyncEnumerator<T> Enumerator { get; }
     
            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return this.Enumerator;
            }
        }

        private sealed class AsyncJobQueueWrappingEnumerator : IAsyncEnumerator<T>
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

        /// <summary>
        /// Creates a new asynchronous job queue.
        /// </summary>
        /// <param name="disposeEnumerable">If false, will not automatically remove the enumerable once the job is complete.</param>
        public AsyncJobQueue(bool disposeEnumerable = true)
        {
            this.Enumerables = new ConcurrentDictionary<Guid, IAsyncEnumerable<T>>();
            this.Enumerators = new ConcurrentDictionary<Guid, IAsyncEnumerator<T>>();
            this.DisposeEnumerable = disposeEnumerable;
        }

        private IDictionary<Guid, IAsyncEnumerable<T>> Enumerables { get; }
        private IDictionary<Guid, IAsyncEnumerator<T>> Enumerators { get; }
        private bool DisposeEnumerable { get; }

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
            bool hasNext = await enumerator.MoveNextAsync();

            var result = (value: enumerator.Current, hasNext);
            if (!result.hasNext)
            {
                await enumerator.DisposeAsync();
                this.Enumerators.Remove(jobId);
                if (this.DisposeEnumerable) this.TryRemoveSource(jobId, out var _);
            }
            return result;
        }

        public ValueTask<Guid> QueueJob(IAsyncEnumerable<T> asyncEnumerable, CancellationToken token = default)
        {
            var guid = Guid.NewGuid();
            var enumerator = asyncEnumerable.GetAsyncEnumerator(token);
            this.Enumerables.Add(guid, asyncEnumerable);
            this.Enumerators.Add(guid, enumerator);
            return new ValueTask<Guid>(guid);
        }

        public async ValueTask<Guid> QueueJob(IAsyncEnumerable<T> asyncEnumerable, Guid guid, CancellationToken token = default)
        {
            if (this.Enumerators.TryGetValue(guid, out IAsyncEnumerator<T> old))
            {
                await old.DisposeAsync();
                this.Enumerators.Remove(guid);
            }

            if (this.Enumerables.TryGetValue(guid, out var _))
            {
                this.Enumerables.Remove(guid);
            }

            var enumerator = asyncEnumerable.GetAsyncEnumerator(token);
            this.Enumerators.Add(guid, enumerator);
            this.Enumerables.Add(guid, asyncEnumerable);
            return guid;
        }

        public IAsyncEnumerable<T> AsEnumerable(Guid jobId)
        {
            return new AsyncJobQueueEnumerable(this, jobId);
        }

        public IAsyncEnumerable<T> GetSource(Guid jobId)
        {
            this.Enumerables.TryGetValue(jobId, out var enumerable);
            return enumerable;
        }

        public bool TryRemoveSource(Guid jobId, out IAsyncEnumerable<T> asyncEnumerable)
        {
            asyncEnumerable = Empty();
            if (this.Enumerators.ContainsKey(jobId)) return false;
            bool result = this.Enumerables.TryGetValue(jobId, out asyncEnumerable);
            this.Enumerables.Remove(jobId);
            return result;
        }
    }
}
