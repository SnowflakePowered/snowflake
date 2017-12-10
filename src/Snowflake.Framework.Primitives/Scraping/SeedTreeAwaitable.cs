using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Scraping
{
    public sealed class SeedTreeAwaitable
    {
        private Task<SeedTree> BaseTask { get; }

        public static implicit operator SeedTreeAwaitable(SeedContent seedContent)
        {
            return new SeedTreeAwaitable(Task.FromResult<SeedTree>(seedContent));
        }

        public static implicit operator SeedTreeAwaitable(SeedTree seedContent)
        {
            return new SeedTreeAwaitable(Task.FromResult(seedContent));
        }

        public static implicit operator SeedTreeAwaitable((string type, string value) seedContent)
        {
            return new SeedTreeAwaitable(Task.FromResult<SeedTree>(seedContent));
        }

        public static implicit operator SeedTreeAwaitable((SeedContent Content,
            IEnumerable<SeedTree> Children) seedContent)
        {
            return new SeedTreeAwaitable(Task.FromResult<SeedTree>((seedContent.Content, seedContent.Children)));
        }

        public static implicit operator SeedTreeAwaitable((string type, string value,
           IEnumerable<SeedTree> Children) seedContent)
        {
            return new SeedTreeAwaitable(Task.FromResult<SeedTree>(((seedContent.type, seedContent.value), seedContent.Children)));
        }

        public static implicit operator SeedTreeAwaitable(Task<(string type, string value,
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

        public ConfiguredTaskAwaitable<SeedTree>.ConfiguredTaskAwaiter GetAwaiter()
        {
            return this.BaseTask.ConfigureAwait(false).GetAwaiter();
        }
    }
}
