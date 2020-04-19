using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Framework.Extensibility
{
    /// <inheritdoc />
    public sealed class AsyncJobQueue<T>
        : AsyncJobQueue<IAsyncEnumerable<T>, T>, IAsyncJobQueue<T>
    {
        /// <summary>
        /// Creates a new asynchronous job queue.
        /// </summary>
        /// <param name="disposeEnumerable">If false, will not automatically remove the enumerable once the job is complete.</param>
        public AsyncJobQueue(bool disposeEnumerable = true)
            : base(disposeEnumerable)
        {
            
        }

        /// <inheritdoc />
        public new bool TryRemoveSource(Guid jobId, out IAsyncEnumerable<T> asyncEnumerable)
        {
            bool result = base.TryRemoveSource(jobId, out asyncEnumerable);
            asyncEnumerable = asyncEnumerable ?? Empty();
            return result;
        }
    }

    /// <inheritdoc />
    public class AsyncJobQueue<TAsyncEnumerable, T> : IAsyncJobQueue<TAsyncEnumerable, T>
        where TAsyncEnumerable : class, IAsyncEnumerable<T>
    {
        private sealed class AsyncJobQueueEnumerable : IAsyncEnumerable<T>
        {
            public AsyncJobQueueEnumerable(AsyncJobQueue<TAsyncEnumerable, T> queue, Guid job)
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
            public AsyncJobQueueWrappingEnumerator(AsyncJobQueue<TAsyncEnumerable, T> queue, Guid job)
            {
                this.Queue = queue;
                this.JobId = job;
            }

            public T Current { get; set; }

            private AsyncJobQueue<TAsyncEnumerable, T> Queue { get; }
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
            this.Enumerables = new ConcurrentDictionary<Guid, TAsyncEnumerable>();
            this.Enumerators = new ConcurrentDictionary<Guid, IAsyncEnumerator<T>>();
            this.DisposeEnumerable = disposeEnumerable;
        }

        private IDictionary<Guid, TAsyncEnumerable> Enumerables { get; }
        private IDictionary<Guid, IAsyncEnumerator<T>> Enumerators { get; }
        private bool DisposeEnumerable { get; }

        private IAsyncEnumerator<T> GetEnumerator(Guid jobId)
        {
            var exists = this.Enumerators.TryGetValue(jobId, out IAsyncEnumerator<T> value);
            if (!exists) return Empty().GetAsyncEnumerator();
            return value;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        /// <summary>
        /// The empty enumerator.
        /// </summary>
        /// <returns>An empty async enumerable</returns>
        protected static async IAsyncEnumerable<T> Empty()
        {
            yield break;
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        /// <inheritdoc />
        public async ValueTask<(T, bool)> GetNext(Guid jobId)
        {
            var enumerator = this.GetEnumerator(jobId);
            bool hasNext = await enumerator.MoveNextAsync();
            
            // hack to ensure that the last value is not duplicated.
            // the expected behavior is to return null after the enumerable is exhausted.
            // this differs from regular direct IAsyncEnumerator access, where
            // current is never updated when hasnext turns false.
            var result = (value: hasNext ? enumerator.Current : default, hasNext);
            if (!result.hasNext)
            {
                await enumerator.DisposeAsync();
                this.Enumerators.Remove(jobId);
                if (this.DisposeEnumerable) this.TryRemoveSource(jobId, out var _);
            }
            return result;
        }

        /// <inheritdoc />
        public ValueTask<Guid> QueueJob(TAsyncEnumerable asyncEnumerable, CancellationToken token = default)
        {
            var guid = Guid.NewGuid();
            var enumerator = asyncEnumerable.GetAsyncEnumerator(token);
            this.Enumerables.Add(guid, asyncEnumerable);
            this.Enumerators.Add(guid, enumerator);
            return new ValueTask<Guid>(guid);
        }

        /// <inheritdoc />
        public async ValueTask<Guid> QueueJob(TAsyncEnumerable asyncEnumerable, Guid guid, CancellationToken token = default)
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

        /// <inheritdoc />
        public IAsyncEnumerable<T> AsEnumerable(Guid jobId)
        {
            return new AsyncJobQueueEnumerable(this, jobId);
        }

        /// <inheritdoc />
        public TAsyncEnumerable GetSource(Guid jobId)
        {
            this.Enumerables.TryGetValue(jobId, out var enumerable);
            return enumerable;
        }

        /// <inheritdoc />
        public bool TryRemoveSource(Guid jobId, out TAsyncEnumerable asyncEnumerable)
        {
            asyncEnumerable = null;
            if (this.Enumerators.ContainsKey(jobId)) return false;
            bool result = this.Enumerables.TryGetValue(jobId, out asyncEnumerable);
            this.Enumerables.Remove(jobId);
            return result;
        }
    }
}
