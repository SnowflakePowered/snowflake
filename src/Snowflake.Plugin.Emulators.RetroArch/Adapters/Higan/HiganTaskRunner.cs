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
        public IEmulatorTaskRootDirectoryProvider DirectoryProvider { get; }
        public async Task<IEmulatorTaskResult> ExecuteEmulationAsync(IEmulatorTask task)
        {

            IEmulatorTaskRoot taskRoot = new RetroArchTaskRoot(this.DirectoryProvider.GetTaskRoot());

            IProcessBuilder builder = this.RetroArchExecutable.GetProcessBuilder();
            builder.WithArgument("--verbose")
                .WithArgument("-s", taskRoot.SaveDirectory.FullName);

        }
    }
}
