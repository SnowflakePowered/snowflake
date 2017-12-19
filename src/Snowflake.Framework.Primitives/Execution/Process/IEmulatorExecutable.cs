using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Execution.Process
{
    public interface IEmulatorExecutable
    {
        string EmulatorName { get; }
        Version Version { get; }
        IProcessBuilder GetProcessBuilder();
    }
}
