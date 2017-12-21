using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Execution.Extensibility;
using Snowflake.Execution.Process;
using Snowflake.Services;

namespace Snowflake.Support.Execution
{
    public sealed class EmulatorTaskRootDirectoryProvider : IEmulatorTaskRootDirectoryProvider
    {
        public EmulatorTaskRootDirectoryProvider(IContentDirectoryProvider cdp)
        {
            this.EmulatorTaskRoot = cdp.ApplicationData.CreateSubdirectory("emulatorapproot");
        }

        private DirectoryInfo EmulatorTaskRoot { get; }

        public DirectoryInfo GetTaskRoot(IEmulatorTask task)
        {
            return this.EmulatorTaskRoot.CreateSubdirectory(task.TaskIdentifier.ToString());
        }
    }
}
