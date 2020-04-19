using Snowflake.Extensibility.Queueing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Snowflake
{
    /// <summary>
    /// Creates and manages <see cref="IAsyncJobQueue{T}"/> and <see cref="IAsyncJobQueue{TAsyncEnumerable, T}"/>
    /// instances.
    /// </summary>
    public sealed class AsyncJobQueueFactory
        : IAsyncJobQueueFactory
    {
        /// <summary>
        /// Instantiates a new async job queue factory.
        /// </summary>
        public AsyncJobQueueFactory()
        {
            this.JobQueues = new ConcurrentDictionary<(Type TAsyncEnumerable, Type T, bool disposeEnumerable), object>();
        }

        private ConcurrentDictionary<(Type TAsyncEnumerable, Type T, bool disposeEnumerable), object> JobQueues { get; }

        /// <inheritdoc />
        public IAsyncJobQueue<T> GetJobQueue<T>(bool disposeEnumerable = true) => 
            (IAsyncJobQueue<T>)this.GetJobQueue<IAsyncEnumerable<T>, T>(disposeEnumerable);

        /// <inheritdoc />
        public IAsyncJobQueue<TAsyncEnumerable, T> GetJobQueue<TAsyncEnumerable, T>(bool disposeEnumerable)
            where TAsyncEnumerable : class, IAsyncEnumerable<T>
        {
            if (this.JobQueues.TryGetValue((typeof(TAsyncEnumerable), typeof(T), disposeEnumerable), out var queue))
            {
                if (queue is AsyncJobQueue<T> plainJobQueue && typeof(TAsyncEnumerable) == typeof(IAsyncEnumerable<T>)) 
                    return (IAsyncJobQueue<TAsyncEnumerable, T>)queue;
                if (queue is AsyncJobQueue<TAsyncEnumerable, T>)
                    return (IAsyncJobQueue<TAsyncEnumerable, T>)queue;
            }

            if (typeof(TAsyncEnumerable) == typeof(IAsyncEnumerable<T>)) {
                this.JobQueues.TryAdd((typeof(TAsyncEnumerable), typeof(T), disposeEnumerable), new AsyncJobQueue<T>(disposeEnumerable));
                this.JobQueues.TryGetValue((typeof(TAsyncEnumerable), typeof(T), disposeEnumerable), out var newQueue);
                return (IAsyncJobQueue<TAsyncEnumerable, T>)newQueue;
            }
            this.JobQueues.TryAdd((typeof(TAsyncEnumerable), typeof(T), disposeEnumerable), new AsyncJobQueue<TAsyncEnumerable, T>(disposeEnumerable));
            this.JobQueues.TryGetValue((typeof(TAsyncEnumerable), typeof(T), disposeEnumerable), out var newCustomQueue);
            return (IAsyncJobQueue<TAsyncEnumerable, T>)newCustomQueue;
        }
    }
}
