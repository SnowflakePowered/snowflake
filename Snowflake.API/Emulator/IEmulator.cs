using System.ComponentModel.Composition;
using Snowflake.Plugin;

namespace Snowflake.Emulator
{
    [InheritedExport(typeof(IEmulator))]
    public interface IEmulator : IPlugin
    {
        void Run(string gameUuid);
    }
}
