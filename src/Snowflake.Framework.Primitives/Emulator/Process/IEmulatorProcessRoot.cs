using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Emulator.Process
{
    public interface IEmulatorProcessRoot
    {
        DirectoryInfo ProcessRoot { get; }
        DirectoryInfo SaveDirectory { get; }
        DirectoryInfo ConfigurationDirectory { get; }
        DirectoryInfo SystemDirectory { get; }
    }
}
