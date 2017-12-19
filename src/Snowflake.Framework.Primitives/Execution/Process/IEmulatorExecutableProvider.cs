using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Execution.Process
{
    public interface IEmulatorExecutableProvider
    {
        IEmulatorExecutable GetEmulator(string name);
        IEmulatorExecutable GetEmulator(string name, Version semver);
    }
}
