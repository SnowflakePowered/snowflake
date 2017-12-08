using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Scraping
{
    public sealed class ReturnSeedContent
    {
        private Task<SeedContent> BaseTask { get; }

        public static implicit operator ReturnSeedContent((string Type, string Value) seedContent)
        {
            return new ReturnSeedContent(Task.FromResult<SeedContent>((seedContent.Type, seedContent.Value)));
        }

        public static implicit operator ReturnSeedContent(Task<(string Type, string Value)> seedContent)
        {
            var completionSource = new TaskCompletionSource<SeedContent>();
            seedContent.ContinueWith(t => completionSource.SetResult(t.Result));
            seedContent.ContinueWith(t => completionSource.SetException(t.Exception.InnerExceptions),
                TaskContinuationOptions.OnlyOnFaulted);
            seedContent.ContinueWith(t => completionSource.SetCanceled(),
                TaskContinuationOptions.OnlyOnCanceled);
            return new ReturnSeedContent(completionSource.Task);
        }

        public static implicit operator ReturnSeedContent(Task<SeedContent> seedContent)
        {
            var completionSource = new TaskCompletionSource<SeedContent>();
            seedContent.ContinueWith(t => completionSource.SetResult(t.Result));
            seedContent.ContinueWith(t => completionSource.SetException(t.Exception.InnerExceptions),
                TaskContinuationOptions.OnlyOnFaulted);
            seedContent.ContinueWith(t => completionSource.SetCanceled(),
                TaskContinuationOptions.OnlyOnCanceled);
            return new ReturnSeedContent(completionSource.Task);
        }

        private ReturnSeedContent(Task<SeedContent> baseTask)
        {
            this.BaseTask = baseTask;
        }

        public async Task<SeedContent> Run()
        {
            return await this.BaseTask;
        }
    }
}
