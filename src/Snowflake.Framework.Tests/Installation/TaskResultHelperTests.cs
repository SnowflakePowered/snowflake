using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Installation.Tests
{
    public class TaskResultHelperTests
    {
        [Fact]
        public async Task SuccessTask_Test()
        {
            var vt = TaskResult.Success("test", new ValueTask<string>("Hello"));
            Assert.Equal("Hello", await vt);
            Assert.Equal("test", vt.Name);

            var t = TaskResult.Success("test", Task.FromResult("Hello"));
            Assert.Equal("Hello", await t);
            Assert.Equal("test", t.Name);
        }

        [Fact]
        public async Task FailTask_Test()
        {
            var vt = TaskResult.Failure("test", new ValueTask<string>("Hello"), new Exception());
            Assert.Equal("Hello", await vt);
            Assert.Equal("test", vt.Name);
            Assert.NotNull(vt.Error);

            var t = TaskResult.Failure("test", Task.FromResult("Hello"), new Exception());
            Assert.Equal("Hello", await t);
            Assert.Equal("test", t.Name);
            Assert.NotNull(t.Error);
        }

        [Fact]
        public async Task TypeErasedTask_Test()
        {
            ITaskResult vt = TaskResult.Success("test", new ValueTask<string>("Hello"));
            Assert.Equal("Hello", await vt);
            Assert.Equal("test", vt.Name);

            ITaskResult t = TaskResult.Success("test", Task.FromResult("Hello"));
            Assert.Equal("Hello", await t);
            Assert.Equal("test", t.Name);
        }

    }
}
