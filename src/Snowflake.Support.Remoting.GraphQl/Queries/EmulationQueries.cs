using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Execution.Extensibility;
using Snowflake.Execution.Saving;
using Snowflake.Extensibility;
using Snowflake.Records.Game;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Framework.Attributes;
using Snowflake.Support.Remoting.GraphQl.Framework.Query;
using Snowflake.Support.Remoting.GraphQl.Types.Execution;

namespace Snowflake.Support.Remoting.GraphQl.Queries
{
    public class EmulationQueries : QueryBuilder
    {
        public IPluginCollection<IEmulator> Emulators { get; set; }
        public IStoneProvider Stone { get; set; }
        public IContentDirectoryProvider Cdp { get; set; }

        [Field("testEmuTask", "test", typeof(EmulatorTaskResultGraphType))]
        public async Task<IEmulatorTaskResult> TestTask()
        {
            var emu = this.Emulators.First();
            var game = new GameRecord(this.Stone.Platforms["NINTENDO_SNES"], "TestGame");
            var saveLocation = await new SaveLocationProvider(this.Cdp).CreateSaveLocationAsync(game, "sram");
            var task = emu.CreateTask(game, saveLocation, new List<IEmulatedController>());
            return await emu.Runner.ExecuteEmulationAsync(task);
        }
    }
}
