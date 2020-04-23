using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Installation.Tests
{
    public class TaskErrorHandlingTests
    {
        [Fact]
        public async Task TaskFailInTask_Test()
        {
            await foreach(var i in FailInTask())
            {
                Assert.Equal("FAILED", await i.Description);
                Assert.NotNull(i.Error);
                await Assert.ThrowsAsync<Exception>(async () => await i);
            }
        }

        [Fact]
        public async Task TaskFailEnumeratorTask_Test()
        {
            int ctr = 0;
            await foreach (var i in FailInTask())
            {
                Assert.Equal("FAILED", await i.Description);
                Assert.NotNull(i.Error);
                ctr++;
            }
            Assert.Equal(1, ctr);
        }

        public async IAsyncEnumerable<TaskResult<int>> FailInTask()
        {
            yield return await new FailTask();
        }

        public async IAsyncEnumerable<TaskResult<int>> FailInTaskEnumerator()
        {
            var iterator = new FailEnumeratorTask();
            int ctr = 0;
            await foreach (var i in iterator)
            {
                Assert.Equal("FAILED", await i.Description);
                Assert.NotNull(i.Error);
                ctr++;
            }
            Assert.Equal(1, ctr);
            yield break;
        }
    }

    public sealed class FailEnumeratorTask : AsyncInstallTaskEnumerable<int>
    {
        public FailEnumeratorTask()
        {
            this.Counter = 0;
        }

        public int Counter { get; set; }

        protected override string TaskName => "FailEnumerator";
        protected override ValueTask<string> CreateFailureDescription(AggregateException e)
        {
            return new ValueTask<string>(e.InnerException.Message);
        }

        protected async override IAsyncEnumerable<int> ExecuteOnce()
        {
            throw new Exception("FAILED");
            yield break;
        }
    }

    public sealed class FailTask: AsyncInstallTask<int>
    {
        public FailTask()
        {
        }

        protected override string TaskName => "Fail";
        protected override ValueTask<string> CreateFailureDescription(AggregateException e)
        {
            return new ValueTask<string>(e.InnerException.Message);
        }

        protected async override Task<int> ExecuteOnce()
        {
             throw new Exception("FAILED");
        }
    }
}
