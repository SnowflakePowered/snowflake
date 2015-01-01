using System;
namespace Snowflake.Emulator
{
    public interface IEmulatorAssembly
    {
        EmulatorAssemblyType AssemblyType { get; }
        string EmulatorId { get; }
        string EmulatorName { get; }
        string MainAssembly { get; }
    }
}
