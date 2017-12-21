using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Execution.Extensibility;
using Xunit;

namespace Snowflake.Emulator.Execution
{
    public class EmulatorTaskResultTests
    {
        [Fact]
        public void EmulatorTaskResultCreation_Test()
        {
            var datetime = DateTimeOffset.UtcNow;
            var result = new TestTaskResult(datetime);
            Assert.Equal("TestTask", result.EmulatorName);
            Assert.Equal(datetime, result.StartTime);
            Assert.False(result.IsRunning);
            Assert.Throws<InvalidOperationException>(() => result.Closed());
        }
    }

    public class TestTaskResult : EmulatorTaskResult
    {
        public TestTaskResult()
            : base("TestTask")
        {
        }

        public TestTaskResult(DateTimeOffset startTime)
            : base("TestTask", startTime)
        {
        }

        public override void Closed()
        {
            throw new InvalidOperationException();
        }
    }
}
