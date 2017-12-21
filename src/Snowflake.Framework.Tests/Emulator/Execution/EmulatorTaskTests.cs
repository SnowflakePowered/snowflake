using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Snowflake.Execution.Extensibility;
using Snowflake.Records.Game;
using Xunit;

namespace Snowflake.Emulator.Execution
{
    public class EmulatorTaskTests
    {
        [Fact]
        public void EmulatorTaskPragma_Test()
        {
            var gameRecord = new Mock<IGameRecord>();
            var emulatorTask = new EmulatorTask(gameRecord.Object);
            emulatorTask.AddPragma("testpragma", "test");
            Assert.Equal("test", (emulatorTask as IEmulatorTask)?.Pragmas["testpragma"]);
        }
    }
}
