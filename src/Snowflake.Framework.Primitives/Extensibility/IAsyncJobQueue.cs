using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Framework.Extensibility
{
    /// <summary>
    /// A queue for long-existing <see cref="IAsyncEnumerable{T}"/> that represent a collection of long running resumable jobs.
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    public interface IAsyncJobQueue<T>
    {
        /// <summary>
        /// Retrieves the next value in the enumerator and whether or not the enumerator is exhausted.
        /// Once the enumerator is exhausted, the value field will always be default(<typeparamref name="T"/>).
        /// </summary>
        /// <param name="jobId">The job token that was returned by <see cref="QueueJob(IAsyncEnumerable{T}, CancellationToken)"/></param>
        /// <returns>
        /// The next value and whether or not there is a value after. 
        /// </returns>
        ValueTask<(T value, bool hasNext)> GetNext(Guid jobId);

        /// <summary>
        /// Gets the remaining values in the enumerator as an <see cref="IAsyncEnumerable{T}"/>
        /// that can be looped on.
        /// 
        /// Looping on this will exhaust the enumerator, and therefore using <see cref="GetNext(Guid)"/> after 
        /// enumerating the returned enumerable here will never return results.
        /// </summary>
        /// <param name="jobId">The job token that was returned by <see cref="QueueJob(IAsyncEnumerable{T}, CancellationToken)"/></param>
        /// <returns>The remaining values in the enumerator as an <see cref="IAsyncEnumerable{T}"</returns>
        IAsyncEnumerable<T> AsEnumerable(Guid jobId);

        /// <summary>
        /// Get the <see cref="IAsyncEnumerable{T}"/> as it was added to the job queue.
        /// </summary>
        /// <param name="jobId">The jobId</param>
        /// <returns>The <see cref="IAsyncEnumerable{T}"/> as it was added to the job queue.</returns>
        IAsyncEnumerable<T> GetSource(Guid jobId);

        /// <summary>
        /// Tries to remove the <see cref="IAsyncEnumerable{T}"/> as it was added to the job queue.
        /// This will only succeed if there are no active jobs for the enumerable in the queue.
        /// </summary>
        /// <param name="jobId">The jobId</param>
        /// <param name="asyncEnumerable">The async enumerable that is removed, or the empty enumerable if the result is false.</param>
        /// <returns>If the enumerable was successfully removed.</returns>
        bool TryRemoveSource(Guid jobId, out IAsyncEnumerable<T> asyncEnumerable);

        /// <summary>
        /// Queues an <see cref="IAsyncEnumerable{T}"/> into the job queue
        /// </summary>
        /// <param name="asyncEnumerable">The <see cref="IAsyncEnumerable{T}"/> to persist in memory.</param>
        /// <param name="token">A <see cref="CancellationToken"/> that will be passed to the enumerator used in <see cref="GetNext(Guid)"/>
        /// and <see cref="AsEnumerable(Guid)"/>.</param>
        /// <returns>A unique job token that can be used to modify the job at a later time.</returns>
        Task<Guid> QueueJob(IAsyncEnumerable<T> asyncEnumerable, CancellationToken token = default);

        /// <summary>
        /// Queues an <see cref="IAsyncEnumerable{T}"/> into the job queue with your own job token.
        /// If this job token already exists, this will replace the old job, which will be disposed.
        /// </summary>
        /// <param name="asyncEnumerable">The <see cref="IAsyncEnumerable{T}"/> to persist in memory.</param>
        /// <param name="guid">A token to refer to the job at a later time.</param>
        /// <param name="token">A <see cref="CancellationToken"/> that will be passed to the enumerator used in <see cref="GetNext(Guid)"/>
        /// and <see cref="AsEnumerable(Guid)"/>.</param>
        /// <returns>The <see cref="Guid"/> token you passed in <paramref name="guid"/>.</returns>
        Task<Guid> QueueJob(IAsyncEnumerable<T> asyncEnumerable, Guid guid, CancellationToken token = default);

    }
}
