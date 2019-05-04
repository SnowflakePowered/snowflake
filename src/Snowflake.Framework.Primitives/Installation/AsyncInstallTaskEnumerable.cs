using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Snowflake.Installation.TaskResult;

namespace Snowflake.Installation
{
    /// <summary>
    /// Represents an installation task that runs at most once, and yields multiple results of type 
    /// <typeparamref name="T"/>. It can also be passed to other installation tasks as a placeholder
    /// for the final result of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the yielded results.</typeparam>
    public abstract class AsyncInstallTaskEnumerable<T>
    {
        private IAsyncEnumerable<TaskResult<T>> BaseTask => this.WrapEnumerator();


        /// <summary>
        /// Runs when the <see cref="AsyncInstallTaskEnumerable{T}"/> is evaluated.
        /// </summary>
        /// <returns>The result of the <see cref="AsyncInstallTaskEnumerable{T}"/>.</returns>
        protected abstract IAsyncEnumerable<T> ExecuteOnce();


        /// <summary>
        /// A string identifier for the task.
        /// </summary>
        protected abstract string TaskName { get; }

        /// <summary>
        /// Gets the <see cref="IAsyncEnumerator{T}"/> for the <see cref="AsyncInstallTaskEnumerable{T}"/>.
        /// </summary>
        /// <returns>The <see cref="IAsyncEnumerator{T}"/> that results the <see cref="AsyncInstallTaskEnumerable{T}"/>.</returns>
        public IAsyncEnumerator<TaskResult<T>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return this.BaseTask.GetAsyncEnumerator(cancellationToken);
        }

        private async IAsyncEnumerable<TaskResult<T>> WrapEnumerator()
        {
            await using var enumerator = this.ExecuteOnce().GetAsyncEnumerator();
          
            // Nasty hack to wrap our enumerator into TaskResults.
            while (true)
            {
                ValueTask<bool> result = enumerator.MoveNextAsync();
                if (result.IsFaulted)
                {
                    yield return Failure(this.TaskName, new ValueTask<T>(enumerator.Current), result.AsTask().Exception);
                }
                else if (await result)
                {
                    yield return Success(this.TaskName, new ValueTask<T>(enumerator.Current));
                }
                else
                {
                    break;
                }
            }
            yield break;
        }
    }
}
