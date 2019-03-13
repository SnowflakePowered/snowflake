using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Installation
{
    public class TaskResult<T>
           : ITaskResult
    {
        private ValueTask<T> Result => CachedResult.Value;

        public string Name { get; } = "Unknown";

        public Exception? Error { get; } = null;


        private readonly Lazy<ValueTask<T>> CachedResult;

        internal TaskResult(string name, ValueTask<T> result, Exception? error)
        {
            this.Name = name;
            this.CachedResult = new Lazy<ValueTask<T>>(result);
            this.Error = error;
        }

        public ConfiguredValueTaskAwaitable<T>.ConfiguredValueTaskAwaiter GetAwaiter()
        {
            return this.Result.ConfigureAwait(false).GetAwaiter();
        }

        ConfiguredTaskAwaitable<object?>.ConfiguredTaskAwaiter ITaskResult.GetAwaiter()
        {
            return this.Result.AsTask().FromDerived<object?, T>().ConfigureAwait(false).GetAwaiter();
        }

        public static implicit operator TaskResult<T>(T t)
        {
            return new TaskResult<T>("Value", new ValueTask<T>(t), null);
        }
    }

    public interface ITaskResult
    {
        string Name { get; }
        Exception? Error { get;}

        ConfiguredTaskAwaitable<object?>.ConfiguredTaskAwaiter GetAwaiter();
    }

    public static class TaskResult
    {
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
        internal static Task<TBase> FromDerived<TBase, TDerived>(this Task<TDerived> task) where TDerived : TBase
        {
            var tcs = new TaskCompletionSource<TBase>();

            task.ContinueWith(t => tcs.SetResult(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith(t => tcs.SetException(t.Exception.InnerExceptions), TaskContinuationOptions.OnlyOnFaulted);
            task.ContinueWith(t => tcs.SetCanceled(), TaskContinuationOptions.OnlyOnCanceled);

            return tcs.Task;
        }
    }
}
