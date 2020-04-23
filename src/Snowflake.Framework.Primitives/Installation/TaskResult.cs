using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Installation
{
    /// <summary>
    /// Represents an awaitable placeholder for a final result of type <typeparamref name="T"/>.
    /// A <see cref="TaskResult{T}"/> can be safely awaited multiple times while guaranteeing that
    /// it remains valid, and any side effects are guaranteed to only occur only once.
    /// 
    /// If <see cref="TaskResult{T}.Error"/> is not null, awaiting on this <see cref="TaskResult{T}"/>
    /// will throw the exception set in <see cref="TaskResult{T}.Error"/>. It is highly
    /// recommended to check the error value before awaiting the task.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    public struct TaskResult<T>
           : ITaskResult
    {
        private ValueTask<T> Result => CachedResult?.Value ?? default;

        /// <summary>
        /// A string identifier for the result
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// A description for the execution of the task result.
        /// Because this can be dependent on inputs and artifacts, this value
        /// is awaitable, but can safely be awaited multiple times.
        /// </summary>
        public ValueTask<string> Description { get; }

        /// <summary>
        /// Enumerating or awaiting <see cref="AsyncInstallTaskEnumerable{T}"/> or <see cref="AsyncInstallTask{T}"/>
        /// must not throw. Hence, exceptions are wrapped during the resolution of the asynchronous task.
        /// If an error occurred, it is set here.
        /// </summary>
        public Exception? Error { get; } 

        private readonly Lazy<ValueTask<T>> CachedResult;

        internal TaskResult(string name, ValueTask<string> description, ValueTask<T> result, Exception? error)
        {
            this.Name = name;
            this.Description = description;
            this.CachedResult = new Lazy<ValueTask<T>>(result);
            this.Error = error;
        }

        internal TaskResult(string name, ValueTask<T> result, Exception? error)
            : this(name, new ValueTask<string>($"Executed {name}"), result, error)
        {

        }

        /// <summary>
        /// Gets the awaiter for this result.
        /// <see cref="TaskResult{T}"/> must be context free, so this awaiter is always configured to
        /// not continue on captured context.
        /// </summary>
        /// <returns>The awaiter for this <see cref="TaskResult{T}"/></returns>
        public ConfiguredValueTaskAwaitable<T>.ConfiguredValueTaskAwaiter GetAwaiter()
        {
            return this.Result.ConfigureAwait(false).GetAwaiter();
        }

        /// <inheritdoc />
        ConfiguredTaskAwaitable<object?>.ConfiguredTaskAwaiter ITaskResult.GetAwaiter()
        {
            return this.Result.AsTask().FromDerived<object?, T>().ConfigureAwait(false).GetAwaiter();
        }

        /// <summary>
        /// Converts a value of type <typeparamref name="T"/> to a <see cref="TaskResult{T}"/>.
        /// Awaiting the resultant <see cref="TaskResult{T}"/> returns the value.
        /// </summary>
        /// <param name="t">The value to wrap.</param>
        public static implicit operator TaskResult<T>(T t)
        {
            return new TaskResult<T>($"Value of {t?.GetType().Name}", new ValueTask<T>(t), null);
        }
    }

    /// <summary>
    /// Type erased interface for <see cref="TaskResult{T}"/>.
    /// </summary>
    public interface ITaskResult
    {
        /// <summary>
        /// A string identifier for the result.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// A description for the execution of the task result.
        /// Since this can differ depending on the input
        /// </summary>
        ValueTask<string> Description { get; }

        /// <summary>
        /// Enumerating or awaiting <see cref="AsyncInstallTaskEnumerable{T}"/> or <see cref="AsyncInstallTask{T}"/>
        /// must not throw. Hence, exceptions are wrapped during the resolution of the asynchronous task.
        /// If an error occurred, it is set here.
        /// </summary>
        Exception? Error { get; }

        /// <summary>
        /// Gets the awaiter for this result.
        /// <see cref="ITaskResult"/> must be context free, so this awaiter is always configured to
        /// not continue on captured context.
        /// </summary>
        /// <returns>The awaiter for this <see cref="ITaskResult"/></returns>
        ConfiguredTaskAwaitable<object?>.ConfiguredTaskAwaiter GetAwaiter();
    }

    /// <summary>
    /// Static helper methods to create <see cref="TaskResult{T}"/> instances.
    /// </summary>
    public static class TaskResult
    {
        /// <summary>
        /// Creates a <see cref="TaskResult{TResult}"/> with a non-throwing <see cref="Task{TResult}"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="name">A string identifier for the <see cref="TaskResult{TResult}"/></param>
        /// <param name="result">The task that yields the result.</param>
        /// <returns>A <see cref="TaskResult{TResult}"/> that represents the yielded result.</returns>
        public static TaskResult<TResult> Success<TResult>(string name, Task<TResult> result)
        {
            return new TaskResult<TResult>(name, new ValueTask<TResult>(result), null);
        }

        /// <summary>
        /// Creates a <see cref="TaskResult{TResult}"/> with a non-throwing <see cref="Task{TResult}"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="name">A string identifier for the <see cref="TaskResult{TResult}"/></param>
        /// <param name="description">A description of the task executed.</param>
        /// <param name="result">The task that yields the result.</param>
        /// <returns>A <see cref="TaskResult{TResult}"/> that represents the yielded result.</returns>
        public static TaskResult<TResult> Success<TResult>(string name, ValueTask<string> description, Task<TResult> result)
        {
            return new TaskResult<TResult>(name, description, new ValueTask<TResult>(result), null);
        }

        /// <summary>
        /// Creates a failed <see cref="TaskResult{TResult}"/> with a <see cref="Task{TResult}"/> that may or may not throw.
        /// </summary>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="name">A string identifier for the <see cref="TaskResult{TResult}"/></param>
        /// <param name="result">The task that yields the result.</param>
        /// <param name="ex">The exception to be returned when this <see cref="TaskResult{TResult}"/> is checked for errors.</param>
        /// <returns>A <see cref="TaskResult{TResult}"/> that represents the yielded result, with the provided exception.</returns>
        public static TaskResult<TResult> Failure<TResult>(string name, Task<TResult> result, Exception ex)
        {
            return new TaskResult<TResult>(name, new ValueTask<string>($"Failure occurred at execution of {name}"), new ValueTask<TResult>(result), ex);
        }

        /// <summary>
        /// Creates a failed <see cref="TaskResult{TResult}"/> with a <see cref="Task{TResult}"/> that may or may not throw.
        /// </summary>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="name">A string identifier for the <see cref="TaskResult{TResult}"/></param>
        /// <param name="description">A description of the task failed.</param>
        /// <param name="result">The task that yields the result.</param>
        /// <param name="ex">The exception to be returned when this <see cref="TaskResult{TResult}"/> is checked for errors.</param>
        /// <returns>A <see cref="TaskResult{TResult}"/> that represents the yielded result, with the provided exception.</returns>
        public static TaskResult<TResult> Failure<TResult>(string name, ValueTask<string> description, Task<TResult> result, Exception ex)
        {
            return new TaskResult<TResult>(name, description, new ValueTask<TResult>(result), ex);
        }

        /// <summary>
        /// Creates a <see cref="TaskResult{TResult}"/> with a non-throwing <see cref="ValueTask{TResult}"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="name">A string identifier for the <see cref="TaskResult{TResult}"/></param>
        /// <param name="result">The task that yields the result.</param>
        /// <returns>A <see cref="TaskResult{TResult}"/> that represents the yielded result.</returns>
        public static TaskResult<TResult> Success<TResult>(string name, ValueTask<TResult> result)
        {
            return new TaskResult<TResult>(name, result, null);
        }

        /// <summary>
        /// Creates a <see cref="TaskResult{TResult}"/> with a non-throwing <see cref="ValueTask{TResult}"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="name">A string identifier for the <see cref="TaskResult{TResult}"/></param>
        /// <param name="description">A description of the task executed.</param>
        /// <param name="result">The task that yields the result.</param>
        /// <returns>A <see cref="TaskResult{TResult}"/> that represents the yielded result.</returns>
        public static TaskResult<TResult> Success<TResult>(string name, ValueTask<string> description, ValueTask<TResult> result)
        {
            return new TaskResult<TResult>(name, description, result, null);
        }

        /// <summary>
        /// Creates a failed <see cref="TaskResult{TResult}"/> with a <see cref="ValueTask{TResult}"/> that may or may not throw.
        /// </summary>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="name">A string identifier for the <see cref="TaskResult{TResult}"/></param>
        /// <param name="result">The task that yields the result.</param>
        /// <param name="ex">The exception to be returned when this <see cref="TaskResult{TResult}"/> is checked for errors.</param>
        /// <returns>A <see cref="TaskResult{TResult}"/> that represents the yielded result, with the provided exception.</returns>
        public static TaskResult<TResult> Failure<TResult>(string name, ValueTask<TResult> result, Exception? ex)
        {
            return new TaskResult<TResult>(name, new ValueTask<string>($"Failure occurred at execution of {name}"), result, ex);
        }

        /// <summary>
        /// Creates a failed <see cref="TaskResult{TResult}"/> with a <see cref="ValueTask{TResult}"/> that may or may not throw.
        /// </summary>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="name">A string identifier for the <see cref="TaskResult{TResult}"/></param>
        /// <param name="result">The task that yields the result.</param>
        /// <param name="description">A description of the task failed./param>
        /// <param name="ex">The exception to be returned when this <see cref="TaskResult{TResult}"/> is checked for errors.</param>
        /// <returns>A <see cref="TaskResult{TResult}"/> that represents the yielded result, with the provided exception.</returns>
        public static TaskResult<TResult> Failure<TResult>(string name, ValueTask<string> description, ValueTask<TResult> result, Exception? ex)
        {
            return new TaskResult<TResult>(name, description, result, ex);
        }

        /// <summary>
        /// Downcasts a task of <typeparamref name="TDerived"/> to a task of <typeparamref name="TBase"/>
        /// </summary>
        /// <typeparam name="TBase">The base type.</typeparam>
        /// <typeparam name="TDerived">The derived type.</typeparam>
        /// <param name="task">The task that yields <typeparamref name="TDerived"/>.</param>
        /// <returns>A task that yields <typeparamref name="TBase"/>.</returns>
        internal static Task<TBase> FromDerived<TBase, TDerived>(this Task<TDerived> task) where TDerived : TBase
        {
            var tcs = new TaskCompletionSource<TBase>();

            task.ContinueWith(t => tcs.SetResult(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith(t => tcs.SetException(t.Exception!.InnerExceptions), TaskContinuationOptions.OnlyOnFaulted);
            task.ContinueWith(t => tcs.SetCanceled(), TaskContinuationOptions.OnlyOnCanceled);
            return tcs.Task;
        }
    }
}
