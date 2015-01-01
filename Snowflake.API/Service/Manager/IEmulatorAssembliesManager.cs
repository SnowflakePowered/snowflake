using System;
using System.Collections.Generic;
using Snowflake.Emulator;

namespace Snowflake.Service.Manager
{
    public interface IEmulatorAssembliesManager
    {
        string AssembliesLocation { get; }
        IReadOnlyDictionary<string, IEmulatorAssembly> EmulatorAssemblies { get; }
        string GetAssemblyDirectory(IEmulatorAssembly assembly);
        void LoadEmulatorAssemblies();
    }
}
