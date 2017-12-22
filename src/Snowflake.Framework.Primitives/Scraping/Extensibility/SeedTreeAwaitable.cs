using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Scraping.Extensibility
{
    /// <summary>
    /// Represents an awaitable that returns a <see cref="SeedTree"/>.
    /// Required for SeedBuilder syntax and asynchronous scraping, but
    /// should never be used directly. There is no way to instantiate
    /// a <see cref="SeedTreeAwaitable"/> apart from implicitly coercing a
    /// <see cref="SeedTree"/> or a <see cref="Task{SeedTree}"/>.
    /// </summary>
    public sealed class SeedTreeAwaitable
    {
        private Task<SeedTree> BaseTask { get; }

        /// <summary>
        /// Implicitly coerces a <see cref="SeedContent"/> into a <see cref="SeedTreeAwaitable"/>
        /// </summary>
        /// <param name="seedContent">The <see cref="SeedContent"/> to coerce.</param>
        public static implicit operator SeedTreeAwaitable(SeedContent seedContent)
        {
            return new SeedTreeAwaitable(Task.FromResult<SeedTree>(seedContent));
        }

        /// <summary>
        /// Implicitly coerces a <see cref="SeedTree"/> into a <see cref="SeedTreeAwaitable"/>
        /// </summary>
        /// <param name="seedContent">The <see cref="SeedTree"/> to coerce.</param>
        public static implicit operator SeedTreeAwaitable(SeedTree seedContent)
        {
            return new SeedTreeAwaitable(Task.FromResult(seedContent));
        }

        /// <summary>
        /// Implicitly coerces a <see cref="SeedContent"/> into a <see cref="SeedTreeAwaitable"/>
        /// </summary>
        /// <param name="seedContent">The <see cref="SeedContent"/> to coerce.</param>
        public static implicit operator SeedTreeAwaitable((string type, string value) seedContent)
        {
            return new SeedTreeAwaitable(Task.FromResult<SeedTree>(seedContent));
        }

        /// <summary>
        /// Implicitly coerces a <see cref="SeedTree"/> into a <see cref="SeedTreeAwaitable"/>
        /// </summary>
        /// <param name="seedContent">The <see cref="SeedTree"/> to coerce.</param>
        public static implicit operator SeedTreeAwaitable((SeedContent Content,
            IEnumerable<SeedTree> Children) seedContent)
        {
            return new SeedTreeAwaitable(Task.FromResult<SeedTree>((seedContent.Content, seedContent.Children)));
        }

        /// <summary>
        /// Implicitly coerces a <see cref="SeedTree"/> into a <see cref="SeedTreeAwaitable"/>
        /// </summary>
        /// <param name="seedContent">The <see cref="SeedTree"/> to coerce.</param>
        public static implicit operator SeedTreeAwaitable((string type, string value,
           IEnumerable<SeedTree> Children) seedContent)
        {
            return new SeedTreeAwaitable(Task.FromResult<SeedTree>(((seedContent.type, seedContent.value), seedContent.Children)));
        }

        /// <summary>
        /// Implicitly coerces a <see cref="Task{SeedTree}"/> into a <see cref="SeedTreeAwaitable"/>
        /// </summary>
        /// <param name="seedContent">The <see cref="Task{SeedTree}"/> to coerce.</param>
        public static implicit operator SeedTreeAwaitable(Task<(string type, string value,
            IEnumerable<SeedTree> children)> seedContent)
        {
            var completionSource = new TaskCompletionSource<SeedTree>();
            seedContent.ContinueWith(t => completionSource.SetResult(t.Result));
            seedContent.ContinueWith(t => completionSource.SetException(t.Exception.InnerExceptions),
                TaskContinuationOptions.OnlyOnFaulted);
            seedContent.ContinueWith(t => completionSource.SetCanceled(),
                TaskContinuationOptions.OnlyOnCanceled);
            return new SeedTreeAwaitable(completionSource.Task);
        }

        /// <summary>
        /// Implicitly coerces a <see cref="Task{SeedContent}"/> into a <see cref="SeedTreeAwaitable"/>
        /// </summary>
        /// <param name="seedContent">The <see cref="Task{SeedContent}"/> to coerce.</param>
        public static implicit operator SeedTreeAwaitable(Task<(string type, string value)> seedContent)
        {
            var completionSource = new TaskCompletionSource<SeedTree>();
            seedContent.ContinueWith(t => completionSource.SetResult(t.Result));
            seedContent.ContinueWith(t => completionSource.SetException(t.Exception.InnerExceptions),
                TaskContinuationOptions.OnlyOnFaulted);
            seedContent.ContinueWith(t => completionSource.SetCanceled(),
                TaskContinuationOptions.OnlyOnCanceled);
            return new SeedTreeAwaitable(completionSource.Task);
        }

        /// <summary>
        /// Implicitly coerces a <see cref="Task{SeedTree}"/> into a <see cref="SeedTreeAwaitable"/>
        /// </summary>
        /// <param name="seedContent">The <see cref="Task{SeedTree}"/> to coerce.</param>
        public static implicit operator SeedTreeAwaitable(Task<(SeedContent Content,
            IEnumerable<SeedTree> Children)> seedContent)
        {
            var completionSource = new TaskCompletionSource<SeedTree>();
            seedContent.ContinueWith(t => completionSource.SetResult(t.Result));
            seedContent.ContinueWith(t => completionSource.SetException(t.Exception.InnerExceptions),
                TaskContinuationOptions.OnlyOnFaulted);
            seedContent.ContinueWith(t => completionSource.SetCanceled(),
                TaskContinuationOptions.OnlyOnCanceled);
            return new SeedTreeAwaitable(completionSource.Task);
        }

        /// <summary>
        /// Implicitly coerces a <see cref="Task{SeedTree}"/> into a <see cref="SeedTreeAwaitable"/>
        /// </summary>
        /// <param name="seedContent">The <see cref="Task{SeedTree}"/> to coerce.</param>
        public static implicit operator SeedTreeAwaitable(Task<SeedTree> seedContent)
        {
            var completionSource = new TaskCompletionSource<SeedTree>();
            seedContent.ContinueWith(t => completionSource.SetResult(t.Result));
            seedContent.ContinueWith(t => completionSource.SetException(t.Exception.InnerExceptions),
                TaskContinuationOptions.OnlyOnFaulted);
            seedContent.ContinueWith(t => completionSource.SetCanceled(),
                TaskContinuationOptions.OnlyOnCanceled);
            return new SeedTreeAwaitable(completionSource.Task);
        }

        private SeedTreeAwaitable(Task<SeedTree> baseTask)
        {
            this.BaseTask = baseTask;
        }

        /// <summary>
        /// Gets the awaiter for this <see cref="SeedTreeAwaitable"/>.
        /// </summary>
        /// <returns>The awaiter for this <see cref="SeedTreeAwaitable"/></returns>
        public ConfiguredTaskAwaitable<SeedTree>.ConfiguredTaskAwaiter GetAwaiter()
        {
            return this.BaseTask.ConfigureAwait(false).GetAwaiter();
        }
    }
}
