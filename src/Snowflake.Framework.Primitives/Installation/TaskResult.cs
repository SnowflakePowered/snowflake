using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Installation
{
    public class TaskResult<T>
           : TaskResult
    {
        private ValueTask<T> Result { get; }

        internal TaskResult(string name, ValueTask<T> result, Exception? error)
        {
            this.Name = name;
            this.Result = result;
            this.Error = error;
        }
        public ConfiguredValueTaskAwaitable<T>.ConfiguredValueTaskAwaiter GetAwaiter()
        {
            return this.Result.ConfigureAwait(false).GetAwaiter();
        }

    }

    public abstract class TaskResult
    {
        public string Name { get; protected set; } = "Unknown";
        public Exception? Error { get; protected set; }

        public static TaskResult<TResult> Success<TResult>(string name, Task<TResult> result)
        {
            return new TaskResult<TResult>(name, new ValueTask<TResult>(result), null);
        }

        public static TaskResult<TResult> Failure<TResult>(string name, Task<TResult> result, Exception ex)
        {
            return new TaskResult<TResult>(name, new ValueTask<TResult>(result), ex);
        }

        public static TaskResult<TResult> Success<TResult>(string name, ValueTask<TResult> result)
        {
            return new TaskResult<TResult>(name, result, null);
        }

        public static TaskResult<TResult> Failure<TResult>(string name, ValueTask<TResult> result, Exception ex)
        {
            return new TaskResult<TResult>(name, result, ex);

        }
    }
}
