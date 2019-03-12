using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Snowflake.Installation.TaskResult;

namespace Snowflake.Installation.Tasks
{
    public abstract class InstallTaskAwaitableEnumerable<T>
    {
        private IAsyncEnumerable<TaskResult<T>> BaseTask => this.WrapEnumerator();

        protected abstract IAsyncEnumerable<T> Execute();

        protected abstract string TaskName { get; }

        public IAsyncEnumerator<TaskResult<T>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return this.BaseTask.GetAsyncEnumerator();
        }

        private async IAsyncEnumerable<TaskResult<T>> WrapEnumerator()
        {
            await using var enumerator = this.Execute().GetAsyncEnumerator();
          
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
