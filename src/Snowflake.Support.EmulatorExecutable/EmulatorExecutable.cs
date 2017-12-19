using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Execution.Process;

namespace Snowflake.Support.EmulatorExecutable
{
    internal sealed class EmulatorExecutable : IEmulatorExecutable
    {
        private FileInfo ProcessPath { get; }

        public EmulatorExecutable(FileInfo fullProcessPath,
            string emulatorName,
            Version version)
        {
            this.ProcessPath = fullProcessPath;
            this.EmulatorName = emulatorName;
            this.Version = version;
        }

        public string EmulatorName { get; }

        public Version Version { get; }

        public IProcessBuilder GetProcessBuilder()
        {
            return new ProcessBuilder(this.ProcessPath);
        }
    }
}
