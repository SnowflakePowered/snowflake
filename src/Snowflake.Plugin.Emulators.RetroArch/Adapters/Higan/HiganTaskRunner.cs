using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Execution.Extensibility;
using Snowflake.Execution.Process;

namespace Snowflake.Adapters.Higan
{
    public class HiganTaskRunner : IEmulatorTaskRunner
    {
        public IEmulatorExecutable RetroArchExecutable { get; }
        public async Task<IEmulatorTaskResult> ExecuteEmulationAsync(IEmulatorTask task)
        {
            IProcessBuilder builder = this.RetroArchExecutable.GetProcessBuilder();
        }
    }
}
