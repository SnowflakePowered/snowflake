using System.ComponentModel.Composition;
using Snowflake.Plugin;

namespace Snowflake.Emulator
{
    [InheritedExport(typeof(IEmulatorBridge))]
    public interface IEmulatorBridge : IPlugin
    {
        void Run(string gameUuid);
    }
}
