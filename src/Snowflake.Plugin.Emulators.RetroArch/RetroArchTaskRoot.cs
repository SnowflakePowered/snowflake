using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Execution.Process;

namespace Snowflake
{
    public class RetroArchTaskRoot : IEmulatorTaskRoot
    {
        public RetroArchTaskRoot(DirectoryInfo taskRoot)
        {
            this.TaskRoot = taskRoot;
            this.SaveDirectory = taskRoot.CreateSubdirectory("save");
            this.ConfigurationDirectory = taskRoot.CreateSubdirectory("config");
            this.SystemDirectory = taskRoot.CreateSubdirectory("system");
        }

        public DirectoryInfo TaskRoot { get; }

        public DirectoryInfo SaveDirectory { get; }

        public DirectoryInfo ConfigurationDirectory { get; }

        public DirectoryInfo SystemDirectory { get; }
    }
}
