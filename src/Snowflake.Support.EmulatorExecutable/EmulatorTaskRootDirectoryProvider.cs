using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Execution.Process;
using Snowflake.Services;

namespace Snowflake.Support.EmulatorExecutable
{
    public sealed class EmulatorTaskRootDirectoryProvider : IEmulatorTaskRootDirectoryProvider
    {
        public EmulatorTaskRootDirectoryProvider(IContentDirectoryProvider cdp)
        {
            this.EmulatorTaskRoot = cdp.ApplicationData.CreateSubdirectory("emulatorapproot");
        }

        private DirectoryInfo EmulatorTaskRoot { get; }

        public DirectoryInfo GetTaskRoot()
        {
            return this.EmulatorTaskRoot.CreateSubdirectory(Guid.NewGuid().ToString());
        }
    }
}
