using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Extensibility.Queueing
{
    /// <summary>
    /// Creates and manages <see cref="IAsyncJobQueue{T}"/> and <see cref="IAsyncJobQueue{TAsyncEnumerable, T}"/>
    /// instances.
    /// </summary>
    public interface IAsyncJobQueueFactory
    {
        /// <summary>
        /// Gets or creates a job queue with the given type parameters
        /// </summary>
        /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
        /// <param name="disposeEnumerable">Whether to get a job queue that disposes enumerables.</param>
        /// <returns>An async job queue that works with <see cref="IAsyncEnumerable{T}"/></returns>
        IAsyncJobQueue<T> GetJobQueue<T>(bool disposeEnumerable = true);

        /// <summary>
        /// Gets or creates a job queue with the given type parameters
        /// </summary>
        /// <typeparam name="TAsyncEnumerable">The type of the enumerable that must implement <see cref="IAsyncEnumerable{T}"/></typeparam>
        /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
        /// <param name="disposeEnumerable">Whether to get a job queue that disposes enumerables.</param>
        /// <returns>An async job queue that works with the given TAsyncEnumerable type.</returns>
        IAsyncJobQueue<TAsyncEnumerable, T> GetJobQueue<TAsyncEnumerable, T>(bool disposeEnumerable = true)
            where TAsyncEnumerable : class, IAsyncEnumerable<T>;
    }
}
