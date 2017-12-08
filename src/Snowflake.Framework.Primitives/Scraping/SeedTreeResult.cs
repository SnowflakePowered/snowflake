using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Scraping
{
    public sealed class SeedTreeResult
    {
        private Task<SeedTree> BaseTask { get; }

        public static implicit operator SeedTreeResult(SeedContent seedContent)
        {
            return new SeedTreeResult(Task.FromResult<SeedTree>(seedContent));
        }

        public static implicit operator SeedTreeResult((string type, string value) seedContent)
        {
            return new SeedTreeResult(Task.FromResult<SeedTree>(seedContent));
        }

        public static implicit operator SeedTreeResult((SeedContent Content,
            IEnumerable<SeedTree> Children) seedContent)
        {
            return new SeedTreeResult(Task.FromResult<SeedTree>((seedContent.Content, seedContent.Children)));
        }

        public static implicit operator SeedTreeResult((string type, string value,
           IEnumerable<SeedTree> Children) seedContent)
        {
            return new SeedTreeResult(Task.FromResult<SeedTree>(((seedContent.type, seedContent.value), seedContent.Children)));
        }

        public static implicit operator SeedTreeResult(Task<(string type, string value,
            IEnumerable<SeedTree> Children)> seedContent)
        {
            var completionSource = new TaskCompletionSource<SeedTree>();
            seedContent.ContinueWith(t => completionSource.SetResult(t.Result));
            seedContent.ContinueWith(t => completionSource.SetException(t.Exception.InnerExceptions),
                TaskContinuationOptions.OnlyOnFaulted);
            seedContent.ContinueWith(t => completionSource.SetCanceled(),
                TaskContinuationOptions.OnlyOnCanceled);
            return new SeedTreeResult(completionSource.Task);
        }

        public static implicit operator SeedTreeResult(Task<(string type, string value)> seedContent)
        {
            var completionSource = new TaskCompletionSource<SeedTree>();
            seedContent.ContinueWith(t => completionSource.SetResult(t.Result));
            seedContent.ContinueWith(t => completionSource.SetException(t.Exception.InnerExceptions),
                TaskContinuationOptions.OnlyOnFaulted);
            seedContent.ContinueWith(t => completionSource.SetCanceled(),
                TaskContinuationOptions.OnlyOnCanceled);
            return new SeedTreeResult(completionSource.Task);
        }

        public static implicit operator SeedTreeResult(Task<(SeedContent Content,
            IEnumerable<SeedTree> Children)> seedContent)
        {
            var completionSource = new TaskCompletionSource<SeedTree>();
            seedContent.ContinueWith(t => completionSource.SetResult(t.Result));
            seedContent.ContinueWith(t => completionSource.SetException(t.Exception.InnerExceptions),
                TaskContinuationOptions.OnlyOnFaulted);
            seedContent.ContinueWith(t => completionSource.SetCanceled(),
                TaskContinuationOptions.OnlyOnCanceled);
            return new SeedTreeResult(completionSource.Task);
        }

        public static implicit operator SeedTreeResult(Task<SeedTree> seedContent)
        {
            var completionSource = new TaskCompletionSource<SeedTree>();
            seedContent.ContinueWith(t => completionSource.SetResult(t.Result));
            seedContent.ContinueWith(t => completionSource.SetException(t.Exception.InnerExceptions),
                TaskContinuationOptions.OnlyOnFaulted);
            seedContent.ContinueWith(t => completionSource.SetCanceled(),
                TaskContinuationOptions.OnlyOnCanceled);
            return new SeedTreeResult(completionSource.Task);
        }

        private SeedTreeResult(Task<SeedTree> baseTask)
        {
            this.BaseTask = baseTask;
        }

        public async Task<SeedTree> Run()
        {
            return await this.BaseTask;
        }
    }
}
